using System.ComponentModel.DataAnnotations;

namespace Aperta_web_app.Models.Group
{
    public abstract class BaseGroupDto
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
    }
}
