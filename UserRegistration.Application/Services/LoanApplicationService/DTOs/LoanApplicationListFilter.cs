using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistration.Application.Interfaces.Marker;

namespace UserRegistration.Application.Services.LoanApplicationService.DTOs
{
   

    public class LoanApplicationListFilter : PaginationFilter
    {
        public string? Keyword { get; set; }
    }

}
