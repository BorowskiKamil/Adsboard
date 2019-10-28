FROM mcr.microsoft.com/dotnet/core/sdk AS build
WORKDIR /app
COPY ["Adsboard.Services.Adverts/src/Adsboard.Services.Adverts/Adsboard.Services.Adverts.csproj", "Adsboard.Services.Adverts/src/Adsboard.Services.Adverts/"]
COPY ["Adsboard.Common/src/Adsboard.Common/Adsboard.Common.csproj", "Adsboard.Common/src/Adsboard.Common/"]
COPY . .
RUN dotnet publish "Adsboard.Services.Adverts/src/Adsboard.Services.Adverts/Adsboard.Services.Adverts.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS http://*:5004
ENV ASPNETCORE_ENVIRONMENT docker
EXPOSE 5004
ENTRYPOINT dotnet Adsboard.Services.Adverts.dll