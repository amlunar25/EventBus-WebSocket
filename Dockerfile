FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["YourAspNetCoreProject.csproj", "YourAspNetCoreProject/"]
RUN dotnet restore "YourAspNetCoreProject/YourAspNetCoreProject.csproj"
COPY . .
WORKDIR "/src/YourAspNetCoreProject"
RUN dotnet build "YourAspNetCoreProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YourAspNetCoreProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YourAspNetCoreProject.dll"]