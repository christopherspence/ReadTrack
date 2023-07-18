FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80
EXPOSE 443

WORKDIR /src
COPY ["ReadTrack.API/ReadTrack.API/ReadTrack.API.csproj", "ReadTrack.API/"]
RUN dotnet restore "ReadTrack.API/ReadTrack.API/ReadTrack.API.csproj"
COPY . . 
WORKDIR "/src/ReadTrack.API"
RUN dotnet build "ReadTrack.API.csproj" -c Release -o /app

RUN dotnet publish "ReadTrack.API.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ReadTrack.API.dll"]