using FluentValidation;
using OfferNegotiatorApi.Configurations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithJwt();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddIdentity();
builder.Services.AddDbContexts(builder.Configuration);
builder.Services.AddAutoMapper(Assembly.Load("OfferNegotiatorLogic"));
builder.Services.AddValidatorsFromAssembly(Assembly.Load("OfferNegotiatorLogic"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.SeedOfferNegotiatorDatabase().Wait();
app.SeedUserDatabase().Wait();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
