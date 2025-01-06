FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY Shortify.Common/*.csproj ./Shortify.Common/
COPY Shortify.Persistence/*.csproj ./Shortify.Persistence/
COPY Shortify.Persistence.EfCore/*.csproj ./Shortify.Persistence.EfCore/
COPY Shortify.API/*.csproj ./Shortify.API/
RUN dotnet restore ./Shortify.API/Shortify.API.csproj

RUN dotnet clean ./Shortify.API/Shortify.API.csproj

COPY Shortify.Common ./Shortify.Common
COPY Shortify.Persistence ./Shortify.Persistence
COPY Shortify.Persistence.EfCore ./Shortify.Persistence.EfCore
COPY Shortify.API ./Shortify.API

RUN dotnet publish /app/Shortify.API/Shortify.API.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "Shortify.API.dll"]
