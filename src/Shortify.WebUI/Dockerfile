# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory to /src
WORKDIR /src

# Copy the project files and restore dependencies
COPY ../Shortify.Common/*.csproj Shortify.Common/
COPY Shortify.WebUI/*.csproj Shortify.WebUI/
RUN dotnet restore Shortify.WebUI/Shortify.WebUI.csproj

# Copy the entire project and build it
COPY ../Shortify.Common Shortify.Common/
COPY Shortify.WebUI Shortify.WebUI/
WORKDIR /src/Shortify.WebUI
RUN dotnet build Shortify.WebUI.csproj -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish Shortify.WebUI.csproj -c Release -o /app/publish

# Stage 3: Run
FROM mcr.microsoft.com/dotnet/aspnet:9.0
ENV ASPNETCORE_HTTP_PORTS=5000
EXPOSE 5000
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shortify.WebUI.dll"]