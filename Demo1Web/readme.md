### ssh to linux
ssh stone@127.0.0.1 -p 2222

### mount folder web
/media/sf_WinShare/NETCore/Demo1Web

### Build dockerfile
cd /media/sf_WinShare/NETCore/Demo1Web
docker build -t demo1web:dev -f Demo1Web/Dockerfile .

### docker run
docker run --rm -d --name demo1web \
  -p 8080:8080 \
  -e ASPNETCORE_URLS=http://0.0.0.0:8080 \
  demo1web:dev

### test url 8080
curl http://localhost:8080

### docker stop
docker stop demo1web && docker rm demo1web
docker run --rm -d --name demo1web -p 8080:8080 -e ASPNETCORE_URLS=http://0.0.0.0:8080 demo1web:dev

### check containers 
docker ps -a


docker stop demo1web

docker rm demo1web

### list images
docker image ls

----------------------------
## Test Buld Docker for Net8


This repository contains the Demo1Web ASP.NET Core 8 sample that can be containerized and
run from an Ubuntu guest VM while being accessed from a Windows 11 host.

## Running the site inside Ubuntu (VirtualBox guest)

1. **Verify the shared folder mount**
   - In VirtualBox share `F:\SharedToLinux` (for example) with the guest and confirm it is
     mounted inside Ubuntu (commonly `/media/sf_WinShare`).
   - The repository should therefore appear in Ubuntu at something like
     `/media/sf_WinShare/NETCore/NET8.0_Docker_WebDemo`.

2. **Build the Docker image**
   ```bash
   cd /media/sf_WinShare/NETCore/NET8.0_Docker_WebDemo
   docker build -t demo1web -f Demo1Web/Dockerfile .
   ```
   - Run the command from the repository root so the Docker build context contains the
     solution files referenced in the Dockerfile.

3. **Run the container and publish port 8080**
   ```bash
   docker run --rm -it -p 8080:8080 demo1web
   ```
   - The container will log `Now listening on: http://0.0.0.0:8080` when the site is ready.

4. **Expose the guest port to Windows 11**
   - If the VirtualBox VM uses a **Bridged Adapter**, Windows can reach the container
     directly via the Ubuntu VM IP (check with `ip addr show`). Browse to
     `http://<ubuntu-ip>:8080` from Windows.
   - If the VM uses the default **NAT** adapter, add a port-forwarding rule in VirtualBox:
     Host Port `8080` → Guest Port `8080`. Windows can then access the site via
     `http://localhost:8080`.

5. **Optional verification from Ubuntu**
   - Before switching to Windows, test the site from inside Ubuntu:
     ```bash
     curl http://localhost:8080
     ```
   - Use `Ctrl+C` in the terminal to stop the container when finished.

Following the above steps lets you build the Docker image in the Ubuntu VM and browse the
ASP.NET Core 8 site from your Windows 11 host.