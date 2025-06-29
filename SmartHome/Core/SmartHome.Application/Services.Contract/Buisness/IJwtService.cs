using SmartHome.Domain.Entities.Identity;

public interface IJwtService
{
    string GenerateToken(AppUser user, IList<string> roles);
}
