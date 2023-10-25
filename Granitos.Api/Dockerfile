FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Granitos.Api/Granitos.Api.csproj", "Granitos.Api/"]
RUN dotnet restore "Granitos.Api/Granitos.Api.csproj"
COPY . .
WORKDIR "/src/Granitos.Api"
RUN dotnet build "Granitos.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Granitos.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Granitos.Api.dll"]
