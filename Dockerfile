# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

ENV ASPNETCORE_URLS=http://+:8010

EXPOSE 8010

# copy csproj and restore as distinct layers
COPY *.sln .
COPY EventBus-WebSocket/*.csproj ./EventBus-WebSocket/
RUN dotnet restore

# copy everything else and build app
COPY EventBus-WebSocket/. ./EventBus-WebSocket/
WORKDIR /source/EventBus-WebSocket
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "EventBus-WebSocket.dll"]