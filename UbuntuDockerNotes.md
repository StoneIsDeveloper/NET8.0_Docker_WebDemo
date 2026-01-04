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

## Container practice commands (inside demo1web)
Enter the container first:
```bash
docker exec -it demo1web /bin/bash
```

### Basic info
```bash
pwd
whoami
id
uname -a
cat /etc/os-release
```

### App files and permissions
```bash
ls -la
ls -la wwwroot
ls -la /app
stat /app
```

### Environment and ports
```bash
printenv | sort | head -n 20
echo $ASPNETCORE_URLS
ss -ltnp
```
Notes:
- `ss` = socket statistics (replacement for `netstat`).
- `-l` show listening sockets, `-t` TCP only, `-n` numeric output, `-p` show process info.

### Install tools to use `ps aux` (inside container)
If `ps` or `head` is missing in the container, install them as root:
```bash
docker exec -it -u root demo1web /bin/bash
apt-get update
apt-get install -y procps coreutils
ps aux | head -n 10
```

### Processes and resources
```bash
ps aux | head -n 10
top -b -n 1 | head -n 20
df -h
free -m
```

### Self-test from inside the container
```bash
curl -I http://localhost:8080
curl http://localhost:8080 | head -n 5
```

### Write test (permissions check)
```bash
echo "hello from container" > /app/test.txt
cat /app/test.txt
```

### Exit container
```bash
exit
```
