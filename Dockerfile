FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SW.Mtm.Web/SW.Mtm.Web.csproj", "SW.Mtm.Web/"]
COPY ["SW.Mtm.Api/SW.Mtm.Api.csproj", "SW.Mtm.Api/"]
COPY ["SW.Mtm.Sdk/SW.Mtm.Sdk.csproj", "SW.Mtm.Sdk/"]
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
 
