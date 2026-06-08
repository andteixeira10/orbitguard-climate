FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY OrbitGuard.Api/OrbitGuard.Api.csproj OrbitGuard.Api/

RUN dotnet restore OrbitGuard.Api/OrbitGuard.Api.csproj

COPY OrbitGuard.Api/ OrbitGuard.Api/

WORKDIR /src/OrbitGuard.Api

RUN dotnet publish OrbitGuard.Api.csproj -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

RUN groupadd -r appgroup && useradd -r -g appgroup -d /app -s /usr/sbin/nologin appuser

WORKDIR /app

COPY --from=build /app/publish .

RUN chown -R appuser:appgroup /app

USER appuser

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "OrbitGuard.Api.dll"]