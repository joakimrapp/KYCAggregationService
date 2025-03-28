using AzureFunctionAPI.Clients;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
builder.Services.AddSingleton<PersonalDetailsClient>();
builder.Services.AddSingleton<ContactDetailsClient>();
builder.Services.AddSingleton<KYCFormDataClient>();

builder.Build().Run();

