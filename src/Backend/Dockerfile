FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG NUGET_USER
ARG NUGET_TOKEN
ARG HANGFIRE_PRO_TOKEN
ARG HANGFIRE_PRO_USER
ARG HANGFIRE_ACE_TOKEN
ARG HANGFIRE_ACE_USER

WORKDIR /src
COPY ["src/Backend/Backend.csproj", "src/Backend/"]
RUN dotnet nuget add source https://nuget.pkg.github.com/SaveApis/index.json -n SaveApis -u $NUGET_USER -p $NUGET_TOKEN --store-password-in-clear-text
RUN dotnet nuget add source https://nuget.hangfire.io/nuget/hangfire-pro/v3/index.json --name hangfire-pro --username $HANGFIRE_PRO_USER --password $HANGFIRE_PRO_TOKEN --store-password-in-clear-text
RUN dotnet nuget add source https://nuget.hangfire.io/nuget/hangfire-ace/v3/index.json --name hangfire-ace --username $HANGFIRE_ACE_USER --password $HANGFIRE_ACE_TOKEN --store-password-in-clear-text

RUN dotnet restore "src/Backend/Backend.csproj"
COPY . .
WORKDIR "/src/src/Backend"
RUN dotnet build "Backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.dll"]
