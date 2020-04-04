FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ZZTCoreAPI/ZZTCoreAPI.csproj", "ZZTCoreAPI/"]
RUN dotnet restore "ZZTCoreAPI/ZZTCoreAPI.csproj"
COPY . .
WORKDIR "/src/ZZTCoreAPI"
RUN dotnet build "ZZTCoreAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZZTCoreAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZZTCoreAPI.dll"]