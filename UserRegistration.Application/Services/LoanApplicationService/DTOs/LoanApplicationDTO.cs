using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistration.Application.Interfaces.Marker;

namespace UserRegistration.Application.Services.LoanApplicationService.DTOs
{
  

    public class LoanApplicationDTO : IDto
    {
        public string ApplicationName { get; set; }
        public Guid Id { get; set; }
        public string LoanTypeString { get; set; }

        public int LoanType { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Period { get; set; }
        public string StatusTypeString { get; set; }

        public int StatusType { get; set; }
    }
}
