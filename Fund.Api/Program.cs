using Fund.Api;
using Fund.Api.Common.Api;
using Fund.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();
builder.AddDataContext();
builder.AddCorsOrigin();
builder.AddDocumentation();
builder.AddServices();




var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.ConfigureDevEnviroment();
}

app.UseCors(ApiConfiguration.CorsPolicyName);
app.MapEndpoints();

app.Run();
