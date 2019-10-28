FROM mcr.microsoft.com/dotnet/core/sdk AS build
WORKDIR /app
COPY ["Adsboard.Services.Categories/src/Adsboard.Services.Categories/Adsboard.Services.Categories.csproj", "Adsboard.Services.Categories/src/Adsboard.Services.Categories/"]
COPY ["Adsboard.Common/src/Adsboard.Common/Adsboard.Common.csproj", "Adsboard.Common/src/Adsboard.Common/"]
COPY . .
RUN dotnet publish "Adsboard.Services.Categories/src/Adsboard.Services.Categories/Adsboard.Services.Categories.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS http://*:5005
ENV ASPNETCORE_ENVIRONMENT docker
EXPOSE 5005
ENTRYPOINT dotnet Adsboard.Services.Categories.dll