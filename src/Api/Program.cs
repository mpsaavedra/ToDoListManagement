using Bootler.Infrastructure.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddBootlerServices(builder.Configuration);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo Tasks Management", Version = "v1" });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseBootlerServices();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.MapDefaultControllerRoute();
// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
