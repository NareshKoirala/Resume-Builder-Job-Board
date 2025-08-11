# Use the official ASP.NET Core runtime image for .NET 9
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy all source code and publish release build
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Final stage: runtime image
FROM base AS final
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Resume_Builder_API.dll"]

