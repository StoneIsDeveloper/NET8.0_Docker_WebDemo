# Linux Docker 学习计划（基于 Demo1Web）

本计划以当前 Demo1Web 为练习项目，目标是让你掌握在 Linux (Ubuntu VM) 中构建、运行、优化、排错和部署 Docker 化的 ASP.NET Core 应用。内容以可执行步骤为主。

---

## 学习目标（Targets）
1. 熟练使用 Docker CLI：构建、运行、停止、查看日志、清理资源。
2. 理解 Dockerfile 多阶段构建并能解释每一段。
3. 掌握容器网络与端口映射（宿主机/虚拟机/Windows 访问）。
4. 能够排查容器启动失败、端口不通、权限问题等常见故障。
5. 会使用 docker compose 管理应用（可扩展数据库/缓存）。
6. 了解镜像优化、缓存利用与安全最佳实践。

---

## 执行计划（Step-by-step）

### 阶段 1：环境确认与基础操作（1 天）
**目标**：能在 Ubuntu VM 中顺利构建并运行 Demo1Web 容器。

1. **SSH 连接与共享目录确认**
   - 连接：`ssh stone@127.0.0.1 -p 2222`
   - 验证共享目录：`ls /media/sf_WinShare/NETCore/Demo1Web`

2. **构建镜像**
   ```bash
   cd /media/sf_WinShare/NETCore/Demo1Web
   docker build -t demo1web:dev -f Demo1Web/Dockerfile .
   ```

3. **运行容器并访问页面**
   ```bash
   docker run --rm -d --name demo1web \
     -p 8080:8080 \
     -e ASPNETCORE_URLS=http://0.0.0.0:8080 \
     demo1web:dev
   ```
   - Ubuntu 内部验证：`curl http://localhost:8080`
   - Windows 浏览器访问：`http://localhost:8080`

4. **常用命令熟练**
   ```bash
   docker ps
   docker logs demo1web
   docker stop demo1web
   docker image ls
   ```

**验收标准**：在 Windows 浏览器能看到 Demo1Web 首页并显示系统信息表格。

---

### 阶段 2：理解 Dockerfile 与镜像结构（1-2 天）
**目标**：能解释 Dockerfile 每一行，知道多阶段构建的意义。

1. 阅读 `Demo1Web/Dockerfile` 与 `DockerfileNotes.md`。
2. 逐段理解：`base`、`build`、`publish`、`final`。
3. 解释以下关键点：
   - `FROM` 选择不同镜像的原因
   - `COPY` 与 `WORKDIR` 的作用
   - `RUN dotnet publish` 和 `/p:UseAppHost=false`
   - `RUN chown -R app:app /app` 的目的

**验收标准**：能用自己的话说明 Dockerfile 的每个阶段在做什么。

---

### 阶段 3：端口与网络理解（1 天）
**目标**：理解端口映射与虚拟机访问路径。

1. 了解三层访问链路：
   - Windows 浏览器 -> VirtualBox NAT 端口转发 -> Ubuntu VM -> Docker 容器
2. 修改容器端口测试：
   ```bash
   docker run --rm -d --name demo1web \
     -p 18080:8080 \
     -e ASPNETCORE_URLS=http://0.0.0.0:8080 \
     demo1web:dev
   ```
   - Windows 访问 `http://localhost:18080`

**验收标准**：能解释端口映射，并成功使用不同 Host 端口访问。

---

### 阶段 4：排错与日志（1-2 天）
**目标**：能排查容器启动失败/端口不通/权限问题。

1. 常见排错命令：
   ```bash
   docker ps -a
   docker logs demo1web
   docker inspect demo1web
   ```
2. 故意制造错误并修复：
   - 改坏 `ASPNETCORE_URLS` 看日志报错
   - 修改端口映射冲突并找到原因

**验收标准**：能基于日志定位问题并修复。

---

### 阶段 5：Docker Compose（2-3 天）
**目标**：学会用 compose 运行 Demo1Web。

1. 新建 `docker-compose.yml`（可选）
2. 使用 `docker compose up -d` 启动
3. 使用 `docker compose logs` 查看日志
4. 使用 `docker compose down` 停止清理

**验收标准**：能使用 compose 启动并访问 Demo1Web。

---

### 阶段 6：镜像优化与实践（长期）
**目标**：镜像更小、更快、更安全。

1. 利用 Docker build cache：理解 `COPY csproj` 与 `COPY . .` 的顺序。
2. 尝试 `--no-cache` 对比构建时间。
3. 了解 `USER app` 的安全意义。
4. 学习 `docker image prune` 清理不用镜像。

**验收标准**：能总结出镜像优化的 3 条做法。

---

## 建议学习节奏
- 每个阶段完成后做一次小结，记录命令与问题。
- 不追求一次性完成，优先保证每一步能复现。

---

## 下一步（可选拓展）
1. 加入一个数据库（如 PostgreSQL）并用 compose 管理。
2. 学习 Docker 网络与 volume 挂载。
3. 尝试在云服务器上部署该镜像。