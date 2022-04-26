FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UrlShortener.App/UrlShortener.App.csproj", "UrlShortener.App/"]
COPY ["UrlShortener.Persistence/UrlShortener.Persistence.csproj", "UrlShortener.Persistence/"]
COPY ["UrlShortener/UrlShortener.csproj", "UrlShortener/"]
RUN dotnet restore "UrlShortener.App/UrlShortener.App.csproj"
COPY . .
WORKDIR "/src/UrlShortener.App"
RUN dotnet build "UrlShortener.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UrlShortener.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UrlShortener.App.dll"]
