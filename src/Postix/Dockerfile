FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-arm64v8 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Postix/Postix.csproj", "Postix/"]
RUN dotnet restore "Postix/Postix.csproj"
COPY . .
WORKDIR "/src/Postix"
RUN dotnet build "Postix.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Postix.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Postix.dll"]

#HEALTHCHECK CMD curl --fail http://localhost:8080/healthz || exit
#HEALTHCHECK CMD wget --quiet --tries=1 --spider http://localhost:8080/healthz || exit 1
