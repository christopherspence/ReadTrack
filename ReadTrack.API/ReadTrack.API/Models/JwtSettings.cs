namespace ReadTrack.API.Models;

public class JwtSettings
{
    public string? SecretKey { get; set; }
    public string? Audience { get; set; }
    public string? Issuer { get; set; }
    public int ExpirationDays { get; set; }
}