using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserRegistration.Application.Services.LoanApplicationService;
using UserRegistration.Application.Services.LoanApplicationService.DTOs;

namespace UserRegisteration.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
  
    public class LoanApplication : ControllerBase
    {
        private readonly ILoanApplicationService _loanApplicationService;

        public LoanApplication(ILoanApplicationService loanApplicationService)
        {
            _loanApplicationService = loanApplicationService;
        }


        [HttpPost("ViewApplication")]
        public async Task<IActionResult> GetAllLoanApplicationsAsync(LoanApplicationListFilter filter)
        {
            var venues = await _loanApplicationService.GetAllLoanApplicationsAsync(filter);
            return Ok(venues);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoanApplication(Guid id)
        {
            var venues = await _loanApplicationService.GetLoanApplication(id);
            return Ok(venues);
        }


        [HttpPost("SendApplication")]
        public async Task<IActionResult> CreateAsync(CreateLoanApplicationRequest request)
        {
                var result = await _loanApplicationService.CreateLoanApplicationAsync(request);
                return Ok(result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(UpdateLoanApplicationRequest request, Guid id)
        {
         
                var result = await _loanApplicationService.UpdateLoanApplicationAsync(request, id);
                return Ok(result);
        }

    }
}
