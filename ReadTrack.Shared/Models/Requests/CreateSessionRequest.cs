using System;

namespace ReadTrack.Shared.Models.Requests;

public class CreateSessionRequest
{
    public int BookId { get; set; }
    public int? NumberOfPages { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Duration { get; set; }
    public int? StartPage { get; set; }
    public int? EndPage { get; set; }
}