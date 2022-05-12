using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UserRegisteration.WebApi.JWTAuthentication;
using UserRegisteration.WebApi.Mapper;
using UserRegistration.Application.Interfaces.Marker;
using UserRegistration.Infrastructure.Persistance;

namespace UserRegisteration.WebApi.ConfigurationHelper
{

        public static class RegisterServices
        {

            public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
            {

            #region [-- REGISTERING SERVICES --]

            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy)); //makes so that all the controllers require authorization

            }).AddFluentValidation(fv =>
            {
                fv.ImplicitlyValidateChildProperties = true;
                fv.ImplicitlyValidateRootCollectionElements = true;

                fv.RegisterValidatorsFromAssemblyContaining<IRequestValidator>(); //auto register all fluent validation validator classes
            });
            services.AddEndpointsApiExplorer();
                services.AddAutoMapper(typeof(MappingProfiles));
                services.AddServices();
            #endregion



            #region [-- SWAGGER SETTINGS --]
            services.AddSwaggerGen(setup =>
                {
                    // Include 'SecurityScheme' to use JWT Authentication
                    var jwtSecurityScheme = new OpenApiSecurityScheme
                    {
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Name = "JWT Authentication",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                        Reference = new OpenApiReference
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme
                        }
                    };

                    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

                });
            #endregion


            #region [-- REGISTERING DB CONTEXT SERVICE --]
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(databaseName: "UserRegistationDb"));
            #endregion

            #region [-- REGISTERING JWT Authentication --]



            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });
            #endregion






          
        }

    }
    }

