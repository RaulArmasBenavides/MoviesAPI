namespace ApiMovies.Core.Entities
{
    /// <summary>
    /// Base for entities that keep track of who created or last changed them.
    /// Values are filled automatically by the DbContext on SaveChanges.
    /// </summary>
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
