using System;

namespace ReadTrack.API.Models.Requests;

public class CreateSessionRequest
{
    public int BookId { get; set; }
    public int? NumberOfPages { get; set; }
    public TimeSpan Time { get; set; }
    public int? StartPage { get; set; }
    public int? EndPage { get; set; }
}