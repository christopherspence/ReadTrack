using System.Collections.Generic;

namespace ReadTrack.Shared;

public class User : BaseModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ProfilePicture { get; set; }
    public bool IsLocked { get; set; }

    // FKs
    public IEnumerable<Book> Books { get; set; }
}