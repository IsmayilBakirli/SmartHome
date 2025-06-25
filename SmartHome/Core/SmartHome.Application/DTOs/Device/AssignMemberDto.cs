using System.ComponentModel.DataAnnotations;

namespace SmartHome.Application.DTOs.Device
{
    public class AssignMemberDto
    {
        [Required(ErrorMessage = "DeviceId  is required.")]
        public int DeviceId { get; set; }

        [Required(ErrorMessage = "MemberId  is required.")]
        public string? MemberId { get; set; }
    }
}
