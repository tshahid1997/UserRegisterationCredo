using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Text;
using UserRegisteration.WebApi.JWTAuthentication;
using UserRegistration.Application.Services.IdentityService;
using UserRegistration.Application.Services.IdentityService.DTOs;
using UserRegistration.Application.Wrapper;

namespace UserRegisteration.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {

        private readonly IJWTManagerRepository _jWTManager;
        private readonly IIdentityService _identityService;
        public IdentityController(IIdentityService identityService, IJWTManagerRepository jWTManager)
        {
            _identityService = identityService;
            _jWTManager = jWTManager;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequestDTO data)
        {
            try
            {
                //get token from CSS
                string credoToken = await _identityService.GetCredoToken();
                if (credoToken == null)
                    return BadRequest("Could not connect to CSS Api !");


                RegisterRequest registerRequest = new RegisterRequest();

                // find person by passing token to PersonFind API
                registerRequest = await _identityService.PersonFind(data, credoToken);

                if (registerRequest.PersonalNumber == null)
                    return BadRequest("Cannot register because Personal Number does not exist in CSS database !");

                
                var getData = await _identityService.RegisterAsync(registerRequest);
                getData.Messages.Add("Registered Successfully!");
                return Ok(getData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginRequest usersdata)
        {
            var token = _jWTManager.Authenticate(usersdata);

            if (token.Result == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
     


    }
}
