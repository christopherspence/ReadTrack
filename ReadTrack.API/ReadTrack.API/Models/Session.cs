using System;

namespace ReadTrack.API.Models;

public class Session : BaseModel
{
    public int? NumberOfPages { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Duration { get; set; }
    public int? StartPage { get; set; }
    public int? EndPage { get; set; }
    public int UserId { get; set; }
}