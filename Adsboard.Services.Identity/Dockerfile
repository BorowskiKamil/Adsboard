FROM mcr.microsoft.com/dotnet/core/sdk AS build
WORKDIR /app
COPY ["Adsboard.Services.Identity/src/Adsboard.Services.Identity/Adsboard.Services.Identity.csproj", "Adsboard.Services.Identity/src/Adsboard.Services.Identity/"]
COPY ["Adsboard.Common/src/Adsboard.Common/Adsboard.Common.csproj", "Adsboard.Common/src/Adsboard.Common/"]
COPY . .
RUN dotnet publish "Adsboard.Services.Identity/src/Adsboard.Services.Identity/Adsboard.Services.Identity.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS http://*:5001
ENV ASPNETCORE_ENVIRONMENT docker
EXPOSE 5001
ENTRYPOINT dotnet Adsboard.Services.Identity.dll