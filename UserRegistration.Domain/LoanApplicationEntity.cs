using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistration.Domain.Common;
using UserRegistration.Enums;

namespace UserRegistration.Domain
{
    public class LoanApplicationEntity : AuditableEntity
    {
        public string ApplicationName { get; set; }
        public LoanTypeEnum LoanType { get; set; }  
        public decimal Amount { get; set;}
        public string Currency { get; set;}
        public string Period { get; set;}
        public StatusEnum Status { get; set; }
    }
}
