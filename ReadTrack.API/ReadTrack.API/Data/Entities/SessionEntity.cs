using System;

namespace ReadTrack.API.Data.Entities;

public class SessionEntity : BaseEntity
{
    public int? NumberOfPages { get; set; }
    public TimeSpan Time { get; set; }
    public int? StartPage { get; set; }
    public int? EndPage { get; set; }

    // FKs
    public int BookId { get; set; }
    public BookEntity Book { get; set; }
}