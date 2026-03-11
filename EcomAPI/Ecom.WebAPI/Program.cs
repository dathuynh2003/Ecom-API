using Ecom.Application.Extensions;
using Ecom.Infrastructure.Extensions;
using Ecom.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
_ = builder.Services.AddApplication();
_ = builder.Services.AddInfrastructure(builder.Configuration);
_ = builder.Services.AddWebAPIServices(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
