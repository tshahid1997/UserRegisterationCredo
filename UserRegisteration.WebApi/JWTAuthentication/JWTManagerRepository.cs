using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserRegistration.Application.Interfaces;
using UserRegistration.Application.Services.IdentityService.DTOs;
using UserRegistration.Domain;
using UserRegistration.Infrastructure.Persistance;
using BC = BCrypt.Net.BCrypt;
namespace UserRegisteration.WebApi.JWTAuthentication
{
	public class JWTManagerRepository : IJWTManagerRepository
	{
		

		private readonly IConfiguration iconfiguration;
		private readonly ApplicationDbContext _context;
		public JWTManagerRepository(IConfiguration iconfiguration, ApplicationDbContext context)
		{
			this.iconfiguration = iconfiguration;
			_context = context;
		}
		public async Task<Tokens> Authenticate(LoginRequest users)
		{
			UserEntity userData=new UserEntity();
			 userData = _context.UserEntities.Where(x=>x.Username==users.Username).FirstOrDefault();
			 
			if (userData == null) 
				return null;


			bool isValidPassword = BC.Verify(users.Password, userData.Password);
			if(!isValidPassword)
				return null;

			// Else we generate JSON Web Token
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
			  {
			  new Claim("uid", userData.Id.ToString()),
			  }),
				Expires = DateTime.UtcNow.AddMinutes(10),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return new Tokens { Token = tokenHandler.WriteToken(token) };

		}
	}
}
