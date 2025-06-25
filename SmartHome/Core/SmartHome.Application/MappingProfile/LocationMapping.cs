using SmartHome.Application.DTOs.Location;
using SmartHome.Domain.Entities;

namespace SmartHome.Application.MappingProfile
{
    public static class LocationMapping
    {
        public static IQueryable<LocationGetDto> MapToLocationDtos(this IQueryable<Location> locations)
        {
            return locations.Select((location) =>
            new LocationGetDto
            {
                Id = location.Id,
                Name = location.Name,
                Description = location.Description,
                Floor = location.Floor,
                Zone = location.Zone
            });
        }
        public static Location MapToLocation(this LocationCreateDto locationCreateDto)
        {
            return new Location
            {
                Name = locationCreateDto.Name,
                Description = locationCreateDto.Description,
                Floor = locationCreateDto.Floor,
                Zone = locationCreateDto.Zone
            };
        }
    }
}
