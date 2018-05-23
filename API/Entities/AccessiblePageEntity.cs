using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class AccessiblePageEntity : BaseEntity
    {
        [Required]
        public int Index { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ValidRoleNames { get; set; }
        // list of role names which can access a page
    }

}
