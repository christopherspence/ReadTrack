using System;
using System.Collections.Generic;
using RandomNameGeneratorLibrary;
using ReadTrack.API.Data.Entities;
using ReadTrack.Shared;
using ReadTrack.Shared.Requests;

namespace ReadTrack.API.Tests.Utilities;

public static class RandomGenerator
{
    private static PersonNameGenerator PersonNameGenerator { get; } = new PersonNameGenerator();
    private static PlaceNameGenerator PlaceNameGenerator { get; } = new PlaceNameGenerator();

    public static Random Random { get; } = new Random();

    public static int CreateNumber(int start, int end) => Random.Next(start, end);
    public static string CreateFirstName() => PersonNameGenerator.GenerateRandomFirstName();
    public static string CreateLastName() => PersonNameGenerator.GenerateRandomLastName();
    public static string CreatePlaceName() => PlaceNameGenerator.GenerateRandomPlaceName();
    public static string CreateAuthorName() => $"{CreateLastName()}, {CreateFirstName()}";
    public static string CreateEmailAddress() => $"{CreateFirstName().ToLower()}.{CreateLastName().ToLower()}@gmail.com";

    public static CreateUserRequest GenerateRandomCreateUserRequest()
        => new()
        {
            Email = CreateEmailAddress(),
            FirstName = CreateFirstName(),
            LastName = CreateLastName(),
            Password = CreatePlaceName()
        };

    public static UserEntity GenerateRandomUser(int id = 1)
        => new()
        {
            Id = id,
            Email = CreateEmailAddress(),
            Password = Guid.NewGuid().ToString(),
            FirstName = CreateFirstName(),
            LastName = CreateLastName(),
            ProfilePicture = Guid.NewGuid().ToString(),
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow,
            SortOrder = 0
        };

    public static User GenerateRandomUserModel(int id = 1)
        => new()
        {
            Id = id,
            Email = CreateEmailAddress(),
            Password = Guid.NewGuid().ToString(),
            FirstName = CreateFirstName(),
            LastName = CreateLastName(),
            ProfilePicture = Guid.NewGuid().ToString()
        };

    public static CreateBookRequest GenerateRandomCreateBookRequest()
        => new ()
        {
            Name = CreatePlaceName(),
            Author = CreateAuthorName(),
            Category = (BookCategory)CreateNumber(0, 15),
            Published = DateTime.UtcNow.AddYears(CreateNumber(1, 30))
        };

    public static BookEntity GenerateRandomBook(int id = 1, int userId = 1)
        => new()
        {
            Id = id,
            Name = CreatePlaceName(),
            Author = CreateAuthorName(),
            Category = (BookCategory)Random.Next(1, 12),
            Published = DateTime.UtcNow.AddYears(CreateNumber(1, 30)),
            UserId = userId,
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow
        };

    public static Book GenerateRandomBookModel(int id = 1)
        => new()
        {
            Id = id,
            Name = CreatePlaceName(),
            Author = CreateAuthorName(),
            Category = (BookCategory)Random.Next(1, 12),
            Published = DateTime.UtcNow.AddYears(CreateNumber(1, 30)),
            Finished = CreateNumber(0, 1) == 1
        };

    public static IEnumerable<BookEntity> GenerateOneHundredRandomBooks(int userId = 1)
    {
        var books = new List<BookEntity>();

        for (var i = 1; i <= 100; i++)
        {
            books.Add(GenerateRandomBook(i, userId));
        }

        return books;
    } 

    public static CreateSessionRequest GenerateRandomCreateSessionRequest(int bookId = 1)
    {
        var startPage = CreateNumber(1, 100);
        var endPage = startPage + CreateNumber(1, 100);
        return new()
        {
            BookId = bookId,
            StartPage = startPage,
            EndPage = endPage,
            NumberOfPages = endPage - startPage,
            Date = DateTime.UtcNow,
            Duration = new TimeSpan(CreateNumber(1, 10), CreateNumber(1, 60), CreateNumber(1, 60))
        };
    }


    public static SessionEntity GenerateRandomSession(int id = 1, int bookId = 1, int userId = 1)       
        => new()
        {
            Id = id,
            BookId = bookId,
            StartPage = CreateNumber(1, 1000),
            EndPage = CreateNumber(1, 1000),
            Date = DateTime.UtcNow,
            Duration = TimeSpan.FromTicks(CreateNumber(1, 100000)),
            NumberOfPages = CreateNumber(1, 100),
            UserId = userId,
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow
        };

        public static Session GenerateRandomSessionModel(int id = 1, int bookId = 1, int userId = 1)       
        => new()
        {
            Id = id,
            StartPage = CreateNumber(1, 1000),
            EndPage = CreateNumber(1, 1000),
            Date = DateTime.UtcNow,
            Duration = TimeSpan.FromTicks(CreateNumber(1, 100000)),
            NumberOfPages = CreateNumber(1, 100),
            UserId = userId
        };


    public static IEnumerable<SessionEntity> GenerateOneHundredRandomSessions(int bookId = 1)
    {
        var sessions = new List<SessionEntity>();

        for (var i = 1; i <= 100; i++)
        {
            sessions.Add(GenerateRandomSession(i, bookId));
        }

        return sessions;
    } 
}