using System;
using System.Collections.Generic;

namespace ReadTrack.Shared.Models;

public class Book : BaseModel
{
    public string? Name { get; set; }
    public string? Author { get; set; }
    public BookCategory Category { get; set; }
    public DateTime? Published { get; set; }
    public int NumberOfPages { get; set; }
    public bool Finished { get; set; }
<<<<<<< HEAD:ReadTrack.Shared/Book.cs

=======
>>>>>>> master:ReadTrack.Shared/Models/Book.cs
    public IEnumerable<Session>? Sessions { get; set; }
}