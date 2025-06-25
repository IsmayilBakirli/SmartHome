using SmartHome.Infrastructure.Filters;
using Microsoft.AspNetCore.Identity;
using SmartHome.Domain.Entities.Identity;
using SmartHome.Infrastructure.Data;
using SmartHome.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers((options) => options.Filters.Add<ValidationFilter>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddAuthenticationService(builder.Configuration);
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();

    await DbInitializer.SeedRolesAndAdminAsync(roleManager, userManager);
}

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