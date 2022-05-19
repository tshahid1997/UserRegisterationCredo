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
           
                var getData = await _identityService.RegisterAsync(data);
                getData.Messages.Add("Registered Successfully!");
                return Ok(getData);
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
