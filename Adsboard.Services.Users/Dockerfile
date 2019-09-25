FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Adsboard.Services.Users/Adsboard.Services.Users.csproj", "Adsboard.Services.Users/"]
RUN dotnet restore "Adsboard.Services.Users/Adsboard.Services.Users.csproj"
COPY . .
WORKDIR "/src/Adsboard.Services.Users"
RUN dotnet build "Adsboard.Services.Users.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Adsboard.Services.Users.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Adsboard.Services.Users.dll"]