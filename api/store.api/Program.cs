

using Store.Api.Modules;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterModules();
var app = builder.Build();

app.MapEndpoints();
//app.MapGet("/", () => "Hello World!");

app.Run();
