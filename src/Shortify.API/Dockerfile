# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory to /src
WORKDIR /src

# Copy the project files and restore dependencies
COPY ../Shortify.Common/*.csproj Shortify.Common/
COPY ../Shortify.Persistence.EfCore/*.csproj Shortify.Persistence.EfCore/
COPY ../Shortify.Persistence/*.csproj Shortify.Persistence/
COPY Shortify.API/*.csproj Shortify.API/
RUN dotnet restore Shortify.API/Shortify.API.csproj

# Copy the entire project and build it
COPY ../Shortify.Common Shortify.Common/
COPY ../Shortify.Persistence.EfCore Shortify.Persistence.EfCore/
COPY ../Shortify.Persistence Shortify.Persistence/
COPY Shortify.API Shortify.API/
WORKDIR /src/Shortify.API
RUN dotnet build Shortify.API.csproj -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish Shortify.API.csproj -c Release -o /app/publish

# Stage 3: Run
FROM mcr.microsoft.com/dotnet/aspnet:9.0
ENV ASPNETCORE_HTTP_PORTS=5001
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shortify.API.dll"]