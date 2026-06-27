# syntax=docker/dockerfile:1.7
# ============================================================
# Backend — ASP.NET Core 10 Minimal API
# Build  : RekomendasiPekerjaanDSS/BackEnd
# Context: RekomendasiPekerjaanDSS/   (parent dir, karena ada .slnx di situ)
# ============================================================

# ---------- Stage 1 : build & publish ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy file proyek dulu untuk caching layer dependency
COPY ["BackEnd/BackEnd.csproj", "BackEnd/"]
RUN dotnet restore "BackEnd/BackEnd.csproj"

# Copy seluruh source lalu publish
COPY ["BackEnd/", "BackEnd/"]
WORKDIR /src/BackEnd
RUN dotnet publish "BackEnd.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# ---------- Stage 2 : runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Non-root user untuk keamanan
USER $APP_UID

COPY --from=build /app/publish .

# Port & URL diset via env (di-inject oleh docker-compose)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BackEnd.dll"]