FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["src/Remby.Api/Remby.Api.csproj", "src/Remby.Api/"]
COPY ["src/Remby.Domain/Remby.Domain.csproj", "src/Remby.Domain/"]
COPY ["src/Remby.Application/Remby.Application.csproj", "src/Remby.Application/"] 
COPY ["src/Remby.Infrastructure/Remby.Infrastructure.csproj", "src/Remby.Infrastructure/"]
RUN dotnet restore "src/Remby.Api/Remby.Api.csproj"
COPY . .
WORKDIR "/src/src/Remby.Api"
RUN dotnet build "./Remby.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./Remby.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Remby.Api.dll"]
