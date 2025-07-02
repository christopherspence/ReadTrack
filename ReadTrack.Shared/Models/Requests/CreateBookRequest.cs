using System;

namespace ReadTrack.Shared.Models.Requests;

public class CreateBookRequest
{
    public string? Name { get; set; }
    public string? Author { get; set; }
    public BookCategory Category { get; set; }
    public DateTime? Published { get; set; }
    public int NumberOfPages { get; set; }
}