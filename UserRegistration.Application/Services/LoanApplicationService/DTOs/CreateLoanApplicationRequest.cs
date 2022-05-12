using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistration.Application.Interfaces.Marker;

namespace UserRegistration.Application.Services.LoanApplicationService.DTOs
{
 
    public class CreateLoanApplicationRequest : IDto
    {
        public string ApplicationName { get; set; }
        public int LoanType { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Period { get; set; }
        public int Status { get; set; }
    }


    public class CreateLoanApplicationValidator : FluentValidation.AbstractValidator<CreateLoanApplicationRequest>
    {
        public CreateLoanApplicationValidator()
        {
            RuleFor(x => x.ApplicationName).NotEmpty();
            RuleFor(x => x.Amount).NotEmpty();
            RuleFor(x => x.Currency).NotEmpty();
            RuleFor(x => x.Period).NotEmpty();
            RuleFor(x => x.LoanType).InclusiveBetween(0, 2);
            RuleFor(x => x.Status).InclusiveBetween(0,3);
        }
    }


}
