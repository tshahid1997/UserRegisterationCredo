using UserRegisteration.WebApi.ConfigurationHelper;
using UserRegisteration.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.ConfigureApplicationServices(builder.Configuration);  // configuring application services

builder.Logging.ConfigureLogging(builder.Configuration); //registering the logger


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
