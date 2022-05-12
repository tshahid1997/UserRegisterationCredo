using AutoMapper;
using BCrypt.Net;
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

        public IdentityService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<string> GetCredoToken()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://test.api.auth.credo.ge/api/Authorize");
            request.Method = HttpMethod.Get;


            var formList = new List<KeyValuePair<string, string>>();
            formList.Add(new KeyValuePair<string, string>("client_id", "css"));
            formList.Add(new KeyValuePair<string, string>("client_secret", "secret"));
            formList.Add(new KeyValuePair<string, string>("grant_type", "password"));
            formList.Add(new KeyValuePair<string, string>("username", "system"));
            formList.Add(new KeyValuePair<string, string>("password", "kajWHdRskq9ZU2Cu"));
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
        public async Task<IResponse<Guid>> RegisterAsync(RegisterRequest request)
        {
            bool isUserExists = await _repository.ExistsAsync<UserEntity>(x => x.Username == request.Username);

            if (isUserExists)
                return Response<Guid>.Fail("User already exists");

            //Mapping the values
            UserEntity user = new UserEntity();
            request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user = _mapper.Map(request, user);


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
