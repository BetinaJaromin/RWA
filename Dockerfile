FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["RWA.Api/RWA.Api.csproj", "RWA.Api/"]
RUN dotnet restore "RWA.Api/RWA.Api.csproj"
COPY . .
WORKDIR "/src/RWA.Api"
RUN dotnet build "RWA.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RWA.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RWA.Api.dll"]