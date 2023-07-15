using Microsoft.EntityFrameworkCore;

namespace ReadTrack.API.Data;

public class ReadTrackContext : DbContext
{
    public ReadTrackContext(DbContextOptions<ReadTrackContext> options) : base(options) { }
}