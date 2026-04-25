namespace ReadTrack.API.Data.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public int SortOrder { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public bool IsDeleted { get; set; }
}