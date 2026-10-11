using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Core.Entities
{
    public class Category : AuditableEntity
    {
        [Key]
        public int  Id { get; set; }

        [Required]
        public string Nombre { get; set; }
    }
}
