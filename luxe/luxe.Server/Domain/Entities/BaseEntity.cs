namespace luxe.Server.Domain.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
