
using System.ComponentModel.DataAnnotations;

namespace Aperta_web_app.Models.Group
{
    public class GroupDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        //public List<UserDto> Users { get; set; }
    }
}
