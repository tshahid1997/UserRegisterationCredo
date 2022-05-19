using AutoMapper;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistration.Application.Interfaces;
using UserRegistration.Application.Services.IdentityService.DTOs;
using UserRegistration.Application.Wrapper;
using UserRegistration.Domain;

namespace UserRegistration.Application.Services.IdentityService
{
 


    public class IdentityService : IIdentityService
    {

        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public IdentityService(IRepositoryAsync repository, IMapper mapper,IConfiguration configuration)
        {
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<string> GetCredoToken()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://test.api.auth.credo.ge/api/Authorize");
            request.Method = HttpMethod.Get;


            var formList = new List<KeyValuePair<string, string>>();
            formList.Add(new KeyValuePair<string, string>("client_id", _configuration.GetSection("CSS").GetSection("client_id").Value));
            formList.Add(new KeyValuePair<string, string>("client_secret", _configuration.GetSection("CSS").GetSection("client_secret").Value));
            formList.Add(new KeyValuePair<string, string>("grant_type", _configuration.GetSection("CSS").GetSection("grant_type").Value));
            formList.Add(new KeyValuePair<string, string>("username", _configuration.GetSection("CSS").GetSection("username").Value));
            formList.Add(new KeyValuePair<string, string>("password", _configuration.GetSection("CSS").GetSection("password").Value));
            request.Content = new FormUrlEncodedContent(formList);

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            dynamic readData = JObject.Parse(result);

            string token = readData.access_token;


            return token;
        }


        public async Task<RegisterRequest> PersonFind(RegisterRequestDTO request, string token)
        {



            var getDataclient = new HttpClient();
            var getDatarequest = new HttpRequestMessage();
            getDatarequest.RequestUri = new Uri("http://test.api.css.credo.ge/api/Person/PersonFind");
            getDatarequest.Method = HttpMethod.Post;

            getDatarequest.Headers.Add("Authorization", "Bearer " + token);
            var bodyString = "{personalN: \"" + request.PersonalNumber + "\"}";
            var content = new StringContent(bodyString, Encoding.UTF8, "application/json");
            getDatarequest.Content = content;

            var getDataresponse = await getDataclient.SendAsync(getDatarequest);
            var getDataresult = await getDataresponse.Content.ReadAsStringAsync();

            dynamic readData = JObject.Parse(getDataresult);


            RegisterRequest user = new RegisterRequest();


            if (readData.StatusCode==200) { 
            user.Name = readData.Result.FirstName;
            user.Surname = readData.Result.LastName;
            user.PersonalNumber = readData.Result.PersonalN;
            user.DateOfBirth = readData.Result.BirthDate;
            user.Username = request.Username;
            user.Password = request.Password;
            }

            return user;
        }






        //Create
        public async Task<IResponse<Guid>> RegisterAsync(RegisterRequestDTO request)
        {

            string credoToken = await GetCredoToken();
            if (credoToken == null)
                return Response<Guid>.Fail("Could not connect to CSS Api !");

            RegisterRequest registerRequest = new RegisterRequest();

            // find person by passing token to PersonFind API
            registerRequest = await PersonFind(request, credoToken);

            if (registerRequest.PersonalNumber == null)
                return Response<Guid>.Fail("Cannot register because Personal Number does not exist in CSS database !");



            bool isUserExists = await _repository.ExistsAsync<UserEntity>(x => x.Username == registerRequest.Username);

            if (isUserExists)
                return Response<Guid>.Fail("User already exists");

            //Mapping the values
            UserEntity user = new UserEntity();
            registerRequest.Password = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
            user = _mapper.Map(registerRequest, user);


            try
            {
                var userId = await _repository.CreateAsync(user);
                await _repository.SaveChangesAsync();

                return Response<Guid>.Success(userId);
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }

        }


    }
}
