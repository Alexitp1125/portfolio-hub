# Stage 1: Build — use the full SDK image to compile the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files and restore dependencies first (better layer caching)
COPY PortfolioHub/PortfolioHub.sln ./PortfolioHub/
COPY PortfolioHub/PortfolioHub/*.csproj ./PortfolioHub/PortfolioHub/
COPY PortfolioHub/PortfolioHub.Client/*.csproj ./PortfolioHub/PortfolioHub.Client/
RUN dotnet restore PortfolioHub/PortfolioHub.sln

# Copy the rest of the source and publish
COPY PortfolioHub/ ./PortfolioHub/
RUN dotnet publish PortfolioHub/PortfolioHub/PortfolioHub.csproj -c Release -o /app/publish

# Stage 2: Runtime — lightweight image with only what's needed to run
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# The app listens on port 8080 inside the container
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "PortfolioHub.dll"]