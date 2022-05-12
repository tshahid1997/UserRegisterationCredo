using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistration.Application.Interfaces.Marker;
using UserRegistration.Application.Services.LoanApplicationService.DTOs;
using UserRegistration.Application.Wrapper;

namespace UserRegistration.Application.Services.LoanApplicationService
{
  

    public interface ILoanApplicationService : ITransientService
    {
        Task<PaginatedResponse<LoanApplicationDTO>> GetAllLoanApplicationsAsync(LoanApplicationListFilter filter);
        Task<IResponse<LoanApplicationDTO>> GetLoanApplication(Guid id);

        Task<IResponse<Guid>> CreateLoanApplicationAsync(CreateLoanApplicationRequest modal);
        Task<IResponse<Guid>> UpdateLoanApplicationAsync(UpdateLoanApplicationRequest modal, Guid id);
    }
}
