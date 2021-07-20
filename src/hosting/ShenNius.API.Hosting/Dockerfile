#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/hosting/ShenNius.API.Hosting/ShenNius.API.Hosting.csproj", "src/hosting/ShenNius.API.Hosting/"]
COPY ["src/module/ShenNius.Shop.API/ShenNius.Shop.API.csproj", "src/module/ShenNius.Shop.API/"]
COPY ["src/ShenNius.Share.BaseController/ShenNius.Share.BaseController.csproj", "src/ShenNius.Share.BaseController/"]
COPY ["src/ShenNius.Share.Domain/ShenNius.Share.Domain.csproj", "src/ShenNius.Share.Domain/"]
COPY ["src/ShenNius.Share.Models/ShenNius.Share.Models.csproj", "src/ShenNius.Share.Models/"]
COPY ["src/ShenNius.Share.Infrastructure/ShenNius.Share.Infrastructure.csproj", "src/ShenNius.Share.Infrastructure/"]
COPY ["src/framework/core/ShenNius.ModuleCore/ShenNius.ModuleCore.csproj", "src/framework/core/ShenNius.ModuleCore/"]
COPY ["src/module/ShenNius.Sys.API/ShenNius.Sys.API.csproj", "src/module/ShenNius.Sys.API/"]
COPY ["src/module/ShenNius.Cms.API/ShenNius.Cms.API.csproj", "src/module/ShenNius.Cms.API/"]
RUN dotnet restore "src/hosting/ShenNius.API.Hosting/ShenNius.API.Hosting.csproj"
COPY . .
WORKDIR "/src/src/hosting/ShenNius.API.Hosting"
RUN dotnet build "ShenNius.API.Hosting.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShenNius.API.Hosting.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShenNius.API.Hosting.dll"]