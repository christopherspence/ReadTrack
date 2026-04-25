using AutoMapper;
using ReadTrack.API.Data.Entities;
using ReadTrack.Shared.Models;

namespace ReadTrack.API.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookEntity, Book>();
        CreateMap<Book, BookEntity>();

        CreateMap<SessionEntity, Session>();
        CreateMap<Session, SessionEntity>();

        CreateMap<UserEntity, User>();
        CreateMap<User, UserEntity>();
    }
}