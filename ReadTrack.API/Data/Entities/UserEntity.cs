using System.Collections.Generic;

namespace ReadTrack.API.Data.Entities;

public class UserEntity : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ProfilePicture { get; set; }
    public bool IsLocked { get; set; }

    // FKs
    public IEnumerable<BookEntity> Books { get; set; }
}