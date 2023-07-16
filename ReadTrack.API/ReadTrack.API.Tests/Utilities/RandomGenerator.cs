using System;
using RandomNameGeneratorLibrary;
using ReadTrack.API.Data.Entities;
using ReadTrack.API.Models;

namespace ReadTrack.API.Tests.Utilities;

public static class RandomGenerator
{
    private static PersonNameGenerator PersonNameGenerator { get; } = new PersonNameGenerator();
    private static PlaceNameGenerator PlaceNameGenerator { get; } = new PlaceNameGenerator();

    private static Random Random { get; } = new Random();

    private static string CreateFirstName() => PersonNameGenerator.GenerateRandomFirstName();
    private static string CreateLastName() => PersonNameGenerator.GenerateRandomLastName();
    private static string CreatePlaceName() => PlaceNameGenerator.GenerateRandomPlaceName();

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
}