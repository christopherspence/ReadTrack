using System;

namespace ReadTrack.API.Data.Entities;

public class SessionEntity : BaseEntity
{
    public int? NumberOfPages { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Duration { get; set; }
    public int? StartPage { get; set; }
    public int? EndPage { get; set; }

    // FKs
    public int BookId { get; set; }
    public BookEntity Book { get; set; }
}