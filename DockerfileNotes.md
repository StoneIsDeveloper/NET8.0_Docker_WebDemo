# Demo1Web Dockerfile Notes / Dockerfile 说明

This file explains the Dockerfile used by the Demo1Web project, with the full Dockerfile and a clearer layout in both English and Chinese.

---

## Dockerfile (Full Content)

```dockerfile
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Demo1Web.csproj", "."]
RUN dotnet restore "Demo1Web.csproj"
COPY . .
RUN dotnet build "Demo1Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Demo1Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
USER root
WORKDIR /app
COPY --from=publish /app/publish .
RUN chown -R app:app /app
USER app
ENTRYPOINT ["dotnet", "Demo1Web.dll"]
```

---

## English

### Overview
This Dockerfile builds and publishes the ASP.NET Core 8 app, then runs it from a smaller runtime image. It uses a multi-stage build to keep the final image lightweight and secure.

### Stage Breakdown
**base**
- Runtime-only image: `mcr.microsoft.com/dotnet/aspnet:8.0`.
- `WORKDIR /app` sets the working directory.
- `EXPOSE 8080` documents the listening port.

**build**
- SDK image: `mcr.microsoft.com/dotnet/sdk:8.0`.
- `ARG BUILD_CONFIGURATION=Release` controls build configuration.
- Restore dependencies from `Demo1Web.csproj`.
- Build output to `/app/build`.

**publish**
- Publishes to `/app/publish` with `UseAppHost=false` to avoid platform-specific host binaries.

**final**
- Starts from `base` runtime image.
- Uses `root` briefly to set ownership, then switches to non-root `app`.
- Runs the app with `dotnet Demo1Web.dll`.

### Why Multi-Stage
- Smaller final image.
- SDK stays out of production.
- Better security posture.

### Why base and build use different images
- `build` uses the SDK image to restore packages, compile, and publish.
- `base` uses the smaller runtime image to run the already-built app.
- This keeps the final image smaller, faster to pull, and reduces attack surface.

---

## 中文

### 概览
该 Dockerfile 先使用 SDK 镜像构建并发布 ASP.NET Core 8 应用，然后用更小的运行时镜像来运行应用。多阶段构建可以让最终镜像更轻量、更安全。

### 阶段说明
**base**
- 运行时镜像：`mcr.microsoft.com/dotnet/aspnet:8.0`。
- `WORKDIR /app` 设置工作目录。
- `EXPOSE 8080` 标明对外端口。

**build**
- SDK 镜像：`mcr.microsoft.com/dotnet/sdk:8.0`。
- `ARG BUILD_CONFIGURATION=Release` 控制构建配置。
- 根据 `Demo1Web.csproj` 还原依赖。
- 构建输出到 `/app/build`。

**publish**
- 发布到 `/app/publish`，并设置 `UseAppHost=false`，避免生成平台相关宿主二进制。

**final**
- 基于 `base` 运行时镜像。
- 先用 `root` 调整目录权限，再切换为非 root 的 `app` 用户。
- 通过 `dotnet Demo1Web.dll` 启动应用。

### 为什么使用多阶段构建
- 最终镜像更小。
- 生产镜像不包含 SDK。
- 安全性更好。

### 为什么 base 和 build 使用不同镜像
- `build` 使用 SDK 镜像进行还原、编译和发布。
- `base` 使用更小的运行时镜像来运行已构建的应用。
- 最终镜像更小、拉取更快、攻击面更低。
