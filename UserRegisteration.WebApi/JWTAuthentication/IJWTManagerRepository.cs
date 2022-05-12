using UserRegistration.Application.Interfaces.Marker;
using UserRegistration.Application.Services.IdentityService.DTOs;

namespace UserRegisteration.WebApi.JWTAuthentication
{
    public interface IJWTManagerRepository:ITransientService
    {
        Task<Tokens> Authenticate(LoginRequest users);
    }
}
