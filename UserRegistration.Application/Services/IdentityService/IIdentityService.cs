using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistration.Application.Interfaces.Marker;
using UserRegistration.Application.Services.IdentityService.DTOs;
using UserRegistration.Application.Wrapper;

namespace UserRegistration.Application.Services.IdentityService
{


    public interface IIdentityService : ITransientService
    {
        Task<IResponse<Guid>> RegisterAsync(RegisterRequestDTO modal);
        Task<String> GetCredoToken();
        Task<RegisterRequest> PersonFind(RegisterRequestDTO request,string token);


    }


}
