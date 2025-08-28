FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["resume_builder_api.csproj", "./"]
RUN dotnet restore "./resume_builder_api.csproj"
COPY . .
RUN dotnet build "resume_builder_api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "resume_builder_api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "resume_builder_api.dll"]