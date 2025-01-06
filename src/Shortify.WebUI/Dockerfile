FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY Shortify.Common/*.csproj ./Shortify.Common/
COPY Shortify.API/*.csproj ./Shortify.API/
COPY Shortify.WebUI/*.csproj ./Shortify.WebUI/
RUN dotnet restore ./Shortify.WebUI/Shortify.WebUI.csproj

RUN dotnet clean ./Shortify.WebUI/Shortify.WebUI.csproj

COPY Shortify.Common ./Shortify.Common
COPY Shortify.WebUI ./Shortify.WebUI

RUN dotnet publish /app/Shortify.WebUI/Shortify.WebUI.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "Shortify.WebUI.dll"]