FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN --mount=type=secret,id=hangfire_ace_url \
    --mount=type=secret,id=hangfire_ace_user \
    --mount=type=secret,id=hangfire_ace_token \
    --mount=type=secret,id=hangfire_pro_url \
    --mount=type=secret,id=hangfire_pro_user \
    --mount=type=secret,id=hangfire_pro_token \
    --mount=type=secret,id=github_user \
    --mount=type=secret,id=github_token \
    dotnet nuget add source https://nuget.pkg.github.com/SaveApis/index.json --name GitHub -u $(cat /run/secrets/github_user) -p $(cat /run/secrets/github_token) --store-password-in-clear-text && \
    dotnet nuget add source $(cat /run/secrets/hangfire_ace_url) --name HangfireAce -u $(cat /run/secrets/hangfire_ace_user) -p $(cat /run/secrets/hangfire_ace_token) --store-password-in-clear-text && \
    dotnet nuget add source $(cat /run/secrets/hangfire_pro_url) --name HangfirePro -u $(cat /run/secrets/hangfire_pro_user) -p $(cat /run/secrets/hangfire_pro_token) --store-password-in-clear-text
COPY ["src/Backend/Backend.csproj", "src/Backend/"]
RUN --mount=type=cache,target=/root/.nuget/packages dotnet restore "src/Backend/Backend.csproj"
COPY . .
WORKDIR "/src/src/Backend"
RUN --mount=type=cache,target=/root/.nuget/packages dotnet build "Backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN --mount=type=cache,target=/root/.nuget/packages dotnet publish "Backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.dll"]
