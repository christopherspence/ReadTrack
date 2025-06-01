using System;
using System.Collections.Generic;
using RandomNameGeneratorLibrary;
using ReadTrack.Shared.Models;
using ReadTrack.Shared.Models.Requests;

namespace ReadTrack.App.MAUI.Tests.Utilities;

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

    public static AuthRequest GenerateRandomAuthRequest()
        => new()
        {
            Email = CreateEmailAddress(),
            Password = CreatePlaceName()
        };

    private static Book GenerateRandomBook(int id = 1)
        => new()
        {
            Id = id,
            Name = CreatePlaceName(),
            Author = CreateAuthorName(),
            Category = (BookCategory)Random.Next(1, 12),
            Published = DateTime.UtcNow.AddYears(CreateNumber(1, 30)),
            Finished = CreateNumber(0, 1) == 1,            
        };

    public static IEnumerable<Book> GenerateOneHundredRandomBooks()
    {
        var books = new List<Book>();

        for (var i = 1; i <= 100; i++)
        {
            books.Add(GenerateRandomBook(i));
        }

        return books;
    } 

    public static TokenResponse GenerateRandomTokenResponse()
        => new()
        {
            Issued = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(CreateNumber(1, 30)),
            Token = CreatePlaceName(),
            Type = "Bearer",
            User = GenerateRandomUser()
        };

    public static User GenerateRandomUser(int id = 1)
        => new()
        {
            Id = id,
            Email = CreateEmailAddress(),
            Password = Guid.NewGuid().ToString(),
            FirstName = CreateFirstName(),
            LastName = CreateLastName()
        };
}