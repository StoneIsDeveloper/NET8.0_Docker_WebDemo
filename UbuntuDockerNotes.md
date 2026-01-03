# Ubuntu VM Docker Notes

## SSH Into VM
- Use Oracle VirtualBox NAT port forwarding: `ssh stone@127.0.0.1 -p 2222`
- Credentials should match the Ubuntu user configured during VM setup.

## Shared Folder Mapping
- Windows host folder: `F:\SharedToLinux`
- Mounted inside Ubuntu via VirtualBox Guest Additions: `/media/sf_WinShare`
- Repository path from Ubuntu: `/media/sf_WinShare/NETCore/Demo1Web`
- Verify it with `ls /media/sf_WinShare/NETCore/Demo1Web`

## Docker Workflow (run inside the VM)
```bash
cd /media/sf_WinShare/NETCore/Demo1Web
```

### Build image
```bash
docker build -t demo1web:dev -f Demo1Web/Dockerfile .
```

### Run container
```bash
docker run --rm -d --name demo1web \
  -p 8080:8080 \
  -e ASPNETCORE_URLS=http://0.0.0.0:8080 \
  demo1web:dev
```
- Once running, browse from Windows: `http://localhost:8080`
- From Ubuntu you can also test: `curl http://localhost:8080`

### Stop container
```bash
docker stop demo1web
```

### Optional cleanup / inspection
```bash
docker rm demo1web          # remove stopped container (if not using --rm)
docker ps -a                # list containers
docker image ls             # list images
```
