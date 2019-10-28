FROM mcr.microsoft.com/dotnet/core/sdk AS build
WORKDIR /app
COPY ["Adsboard.Api/src/Adsboard.Api/Adsboard.Api.csproj", "Adsboard.Api/src/Adsboard.Api/"]
COPY ["Adsboard.Common/src/Adsboard.Common/Adsboard.Common.csproj", "Adsboard.Common/src/Adsboard.Common/"]
COPY . .
RUN dotnet publish "Adsboard.Api/src/Adsboard.Api/Adsboard.Api.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker
EXPOSE 5000
ENTRYPOINT dotnet Adsboard.Api.dll