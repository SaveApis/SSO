FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

RUN --mount=type=secret,id=nuget_user,required=true \
    --mount=type=secret,id=nuget_token,required=true \
    --mount=type=secret,id=hangfire_pro_url,required=true \
    --mount=type=secret,id=hangfire_pro_user,required=true \
    --mount=type=secret,id=hangfire_pro_token,required=true \
    --mount=type=secret,id=hangfire_ace_url,required=true \
    --mount=type=secret,id=hangfire_ace_user,required=true \
    --mount=type=secret,id=hangfire_ace_token,required=true \
    dotnet nuget add source $(cat /run/secrets/hangfire_pro_url) -n hangfire-pro -u $(cat /run/secrets/hangfire_pro_user) -p $(cat /run/secrets/hangfire_pro_token) --store-password-in-clear-text && \
    dotnet nuget add source $(cat /run/secrets/hangfire_ace_url) -n hangfire-ace -u $(cat /run/secrets/hangfire_ace_user) -p $(cat /run/secrets/hangfire_ace_token) --store-password-in-clear-text && \
    dotnet nuget add source https://nuget.pkg.github.com/SaveApis/index.json -n github -u $(cat /run/secrets/nuget_user) -p $(cat /run/secrets/nuget_token) --store-password-in-clear-text

COPY ["src/Backend/Backend.csproj", "src/Backend/"]
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