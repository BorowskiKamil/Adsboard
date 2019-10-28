FROM mcr.microsoft.com/dotnet/core/sdk AS build
WORKDIR /app
COPY ["Adsboard.Services.Operations/src/Adsboard.Services.Operations/Adsboard.Services.Operations.csproj", "Adsboard.Services.Operations/src/Adsboard.Services.Operations/"]
COPY ["Adsboard.Common/src/Adsboard.Common/Adsboard.Common.csproj", "Adsboard.Common/src/Adsboard.Common/"]
COPY . .
RUN dotnet publish "Adsboard.Services.Operations/src/Adsboard.Services.Operations/Adsboard.Services.Operations.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS http://*:5005
ENV ASPNETCORE_ENVIRONMENT docker
EXPOSE 5004
ENTRYPOINT dotnet Adsboard.Services.Operations.dll