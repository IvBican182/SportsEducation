using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aperta_web_app.Data
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        public required string Name { get; set; }

        [ForeignKey("Club")]
        public int ClubId { get; set; }
        public virtual Club Club { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
