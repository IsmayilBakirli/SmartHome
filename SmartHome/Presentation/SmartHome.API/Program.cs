using SmartHome.Infrastructure.Extensions;
using SmartHome.Infrastructure.Filters;
using SmartHome.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers((options) => options.Filters.Add<ValidationFilter>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddAuthenticationService(builder.Configuration)
    .AddPersistenceServices(builder.Configuration);

var app = builder.Build();

await app.Services.SeedRolesAndAdminAsync();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
