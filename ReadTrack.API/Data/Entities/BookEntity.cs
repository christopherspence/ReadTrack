using System;
using System.Collections.Generic;
using ReadTrack.Shared;

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

    public int UserId { get; set; }
    public UserEntity User { get; set; }
}