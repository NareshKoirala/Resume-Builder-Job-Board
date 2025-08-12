# Stage 1: Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Stage 2: Build and publish
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the csproj and restore dependencies
# This handles both cases: building from repo root or from project folder
COPY resume_builder_api/*.csproj ./ 
RUN dotnet restore

# Copy all files and publish
COPY resume_builder_api/. ./
RUN dotnet publish -c Release -o /app/publish

# Stage 3: Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# IMPORTANT: Use the actual dll name from your project output
ENTRYPOINT ["dotnet", "resume_builder_api.dll"]
