using System;

namespace ReadTrack.API.Models;

public class Session : BaseModel
{
    public int? NumberOfPages { get; set; }
    public TimeSpan Time { get; set; }
    public int? StartPage { get; set; }
    public int? EndPage { get; set; }
}