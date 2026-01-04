# Demo1Web Dockerfile Notes / Dockerfile 说明

This file explains the Dockerfile used by the Demo1Web project. It is written in both English and Chinese.

---

## English

### Overview
The Dockerfile builds and publishes the ASP.NET Core 8 app, then runs it from a smaller runtime image. It uses a multi-stage build to keep the final image lightweight and secure.

### Stages and Steps
1. **base**
   - `FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base`
   - Uses the ASP.NET Core runtime image (no SDK).
   - `WORKDIR /app` and `EXPOSE 8080` prepare the runtime container.

2. **build**
   - `FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build`
   - Copies the project file and restores dependencies.
   - Copies the rest of the source and builds the app.

3. **publish**
   - Publishes the app to `/app/publish` with `UseAppHost=false`.
   - Produces a trimmed output for deployment.

4. **final**
   - Starts from `base` runtime image.
   - Copies published output into `/app`.
   - Switches to the non-root `app` user for better security.
   - Runs the app with `dotnet Demo1Web.dll`.

### Why multi-stage
- Keeps runtime image smaller.
- Avoids shipping the SDK in production.
- Improves security posture.

---

## 中文

### 概览
该 Dockerfile 先使用 SDK 镜像构建并发布 ASP.NET Core 8 应用，然后用更小的运行时镜像来运行应用。多阶段构建可以让最终镜像更轻量、更安全。

### 阶段与步骤
1. **base**
   - `FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base`
   - 使用 ASP.NET Core 运行时镜像（不包含 SDK）。
   - `WORKDIR /app` 和 `EXPOSE 8080` 准备运行环境。

2. **build**
   - `FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build`
   - 先复制项目文件并还原依赖。
   - 再复制全部源码并构建应用。

3. **publish**
   - 将应用发布到 `/app/publish`，并设置 `UseAppHost=false`。
   - 输出更适合部署的发布产物。

4. **final**
   - 基于 `base` 运行时镜像。
   - 复制发布产物到 `/app`。
   - 切换到非 root 的 `app` 用户以提升安全性。
   - 通过 `dotnet Demo1Web.dll` 启动应用。

### 为什么使用多阶段构建
- 最终镜像更小。
- 避免在生产镜像中包含 SDK。
- 安全性更好。

