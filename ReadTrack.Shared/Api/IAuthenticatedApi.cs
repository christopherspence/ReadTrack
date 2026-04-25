using Refit;

namespace ReadTrack.Shared.Api;

[Headers("Authorization: Bearer")]
public interface IAuthenticatedApi : IApi { }