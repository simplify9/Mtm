FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
# .NET 8 runtime images changed the default listening port from 80 to 8080
# (aspnet:6.0 shipped ASPNETCORE_URLS=http://+:80, aspnet:8.0 ships
# ASPNETCORE_HTTP_PORTS=8080). The Helm chart hardcodes containerPort 80 and
# the Service targets the named "http" port, so without this the app listens
# on 8080, the Service has no reachable endpoint and the gateway returns 503
# -- silently, because the chart ships probes.enabled=false so the pod still
# reports Ready. Pin the port back to 80 to match the chart and the 6.0 image.
ENV ASPNETCORE_HTTP_PORTS=80
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["SW.Mtm.Web/SW.Mtm.Web.csproj", "SW.Mtm.Web/"]
COPY ["SW.Mtm.Api/SW.Mtm.Api.csproj", "SW.Mtm.Api/"]
COPY ["SW.Mtm.Sdk/SW.Mtm.Sdk.csproj", "SW.Mtm.Sdk/"]
COPY ["SW.Mtm.MsSql/SW.Mtm.MsSql.csproj", "SW.Mtm.MsSql/"]
COPY ["SW.Mtm.MySql/SW.Mtm.MySql.csproj", "SW.Mtm.MySql/"]
COPY ["SW.Mtm.PgSql/SW.Mtm.PgSql.csproj", "SW.Mtm.PgSql/"]




RUN dotnet restore "SW.Mtm.Web/SW.Mtm.Web.csproj"
COPY . .
WORKDIR "/src/SW.Mtm.Web"
RUN dotnet build "SW.Mtm.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SW.Mtm.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .




ENTRYPOINT ["dotnet", "SW.Mtm.Web.dll"]
 
