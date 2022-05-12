using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistration.Application.Interfaces.Marker;

namespace UserRegistration.Application.Services.IdentityService.DTOs
{
    public class RegisterRequestDTO : IDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? PersonalNumber { get; set; }
    }

    public class RegisterRequestValidator : FluentValidation.AbstractValidator<RegisterRequestDTO>
    {
        public RegisterRequestValidator()
        {
            RuleFor(p => p.Username).NotEmpty();
            RuleFor(p => p.Password).NotEmpty();
            RuleFor(p => p.PersonalNumber).NotEmpty();
        }
    }

}
