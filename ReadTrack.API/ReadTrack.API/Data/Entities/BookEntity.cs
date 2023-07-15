using System;
using System.Collections.Generic;
using ReadTrack.API.Models;

namespace ReadTrack.API.Data.Entities;

public class BookEntity : BaseEntity
{
    public string Name { get; set; }
    public string Author { get; set; }
    public BookCategory Category { get; set; }
    public DateTime? Published { get; set; }
    public int NumberOfPages { get; set; }
    public bool Finished { get; set; }

    // FKs 
    public IEnumerable<SessionEntity> Sessions { get; set; }
}