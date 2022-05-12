using FluentValidation;
using UserRegistration.Application.Interfaces.Marker;

namespace UserRegistration.Application.Services.IdentityService.DTOs
{
   

    public class RegisterRequest : IDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
    


}
