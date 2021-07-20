#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/client/ShenNius.Layui.Admin/ShenNius.Layui.Admin.csproj", "src/client/ShenNius.Layui.Admin/"]
RUN dotnet restore "src/client/ShenNius.Layui.Admin/ShenNius.Layui.Admin.csproj"
COPY . .
WORKDIR "/src/src/client/ShenNius.Layui.Admin"
RUN dotnet build "ShenNius.Layui.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShenNius.Layui.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShenNius.Layui.Admin.dll"]