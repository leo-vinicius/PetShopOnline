using PetShopOnline.Application.UseCases.Clientes;
using PetShopOnline.Application.UseCases.Clientes.Create;
using PetShopOnline.Domain;
using PetShopOnline.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<CreateClienteUseCase>();
builder.Services.AddScoped<IClienteRepository, ClienteRepositoryEmMemoria>();
builder.Services.AddScoped<GetClientesUseCase>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "PetShop API",
        Description = "API do sistema de e-commerce PetShop"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
