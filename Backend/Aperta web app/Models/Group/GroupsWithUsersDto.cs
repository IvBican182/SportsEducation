using Aperta_web_app.Data;
using Aperta_web_app.Models.User;
using System.ComponentModel.DataAnnotations;

namespace Aperta_web_app.Models.Group
{
    public class GroupsWithUsersDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Navigation property
        public List<UserInfoDto> Users { get; set; } = new();
    }
}
