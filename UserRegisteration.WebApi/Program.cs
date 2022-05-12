using UserRegisteration.WebApi.ConfigurationHelper;

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
app.UseAuthorization();

app.MapControllers();

app.Run();
