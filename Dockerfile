FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SW.Mtm.Web/SW.Mtm.Web.csproj", "SW.Mtm.Web/"]
COPY ["SW.Mtm.Api/SW.Mtm.Api.csproj", "SW.Mtm.Api/"]
COPY ["SW.Mtm.Sdk/SW.Mtm.Sdk.csproj", "SW.Mtm.Sdk/"]



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
 