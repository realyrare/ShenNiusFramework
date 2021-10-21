
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8816  #端口号（将容器 5000 端口暴露出来， 允许外部连接这个端口。）
COPY . . #将当前目录下的所有文件（除了.dockerignore排除的路径），都拷贝进入 image 文件的/app目录
ENTRYPOINT ["dotnet", "ShenNius.Mvc.Admin.dll"] #运行的程序集