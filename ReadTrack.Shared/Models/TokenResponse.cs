using System;

namespace ReadTrack.Shared.Models;

public class TokenResponse
{
    public string? Type { get; set; }
    public string? Token { get; set; }
    public User? User { get; set; }
    public DateTime Issued { get; set; }
    public DateTime Expires { get; set; }
}