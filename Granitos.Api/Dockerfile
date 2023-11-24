FROM mcr.microsoft.com/dotnet/aspnet:8.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV DEBIAN_FRONTEND noninteractive

FROM mcr.microsoft.com/dotnet/sdk:8.0-bullseye-slim AS build
WORKDIR /src

COPY ["Granitos.Services.Infrastructure", "Granitos.Services.Infrastructure/"]
COPY ["Granitos.Services.Domain", "Granitos.Services.Domain/"]
COPY ["Granitos.Common.Mongo", "Granitos.Common.Mongo/"]
COPY ["Granitos.Common", "Granitos.Common/"]
COPY ["Granitos.Api", "Granitos.Api/"]

WORKDIR "/src/Granitos.Api"

RUN dotnet build "Granitos.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Granitos.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Granitos.Api.dll"]
