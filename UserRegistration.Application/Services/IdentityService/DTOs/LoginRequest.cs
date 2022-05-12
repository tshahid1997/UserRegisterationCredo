using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistration.Application.Interfaces.Marker;

namespace UserRegistration.Application.Services.IdentityService.DTOs
{
    public class LoginRequest : IDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
      
    }


    public class LoginRequestValidator : FluentValidation.AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(p => p.Username).NotEmpty();
            RuleFor(p => p.Password).NotEmpty();
        }
    }
}
