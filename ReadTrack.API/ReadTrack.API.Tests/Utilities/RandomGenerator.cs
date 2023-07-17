using System;
using System.Collections;
using System.Collections.Generic;
using RandomNameGeneratorLibrary;
using ReadTrack.API.Data.Entities;
using ReadTrack.API.Models;

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

    private static string CreatePhoneNumber()
    {
        var str = string.Empty;
        for (int i = 0; i < 10; i++)
        {
            str = string.Concat(str, Random.Next(10).ToString());
        }

        return str;
    }

    private static string CreateEmailAddress() => $"{CreateFirstName().ToLower()}.{CreateLastName().ToLower()}@gmail.com";

    public static UserEntity GenerateRandomUser(int id = 1)
        => new UserEntity
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

    public static BookEntity GenerateRandomBook(int id = 1, int userId = 1)
        => new BookEntity
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

    public static IEnumerable<BookEntity> GenerateOneHundredRandomBooks(int userId = 1)
    {
        var books = new List<BookEntity>();

        for (var i = 1; i <= 100; i++)
        {
            books.Add(GenerateRandomBook(i, userId));
        }

        return books;
    }        
}