# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

RUN ls -la

# Copy solution/project files and restore dependencies
COPY ["SocialMedia.Api/SocialMedia.Api.csproj", "SocialMedia.Api/"]
#COPY ["SocialMedia.Application/SocialMedia.Application.csproj", "SocialMedia.Application/"]
#COPY ["SocialMedia.Domain/SocialMedia.Domain.csproj", "SocialMedia.Domain/"]
#COPY ["SocialMedia.Infrastructure/SocialMedia.Infrastructure.csproj", "SocialMedia.Infrastructure/"]

RUN dotnet restore "SocialMedia.Api/SocialMedia.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/SocialMedia.Api"
RUN dotnet build "SocialMedia.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "SocialMedia.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy published app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "SocialMedia.Api.dll"]

