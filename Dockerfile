# FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
# WORKDIR /app
# EXPOSE 80

# FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
# WORKDIR /src
# COPY ["EventBus-WebSocket/EventBus-WebSocket.csproj", "EventBus-WebSocket/"]
# RUN dotnet restore "EventBus-WebSocket/EventBus-WebSocket.csproj"
# COPY . .
# WORKDIR "/src/EventBus-WebSocket"
# RUN dotnet build "EventBus-WebSocket.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "EventBus-WebSocket.csproj" -c Release -o /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "EventBus-WebSocket.dll"]

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]