# Ubuntu Docker Installation Notes

This document captures the commands for installing Docker Engine inside the Ubuntu VirtualBox VM used for the Demo1Web project. Run everything from the SSH session (`ssh stone@127.0.0.1 -p 2222`).

## 1. Prerequisites
```bash
sudo apt-get update
sudo apt-get install -y ca-certificates curl gnupg lsb-release
```

## apt vs apt-get (update)
- `apt-get update` is the stable, script-friendly command with consistent output.
- `apt update` is the newer, user-friendly front end with nicer progress and upgrade hints.
- Both do the same package index refresh; prefer `apt-get` in scripts and `apt` interactively.

## 2. Add Docker GPG key and repository
```bash
sudo install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg \
  | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
sudo chmod a+r /etc/apt/keyrings/docker.gpg

echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] \
  https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" |
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
```

## 3. Install Docker Engine + CLI + Compose plugin
```bash
sudo apt-get update
sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
```

## 4. Enable and test
```bash
sudo systemctl enable --now docker
sudo docker run --rm hello-world
```

## 5. Run docker without sudo (optional)
```bash
sudo usermod -aG docker $USER
# log out / in or: exec su -l $USER
```

After installation, continue with the project workflow in `UbuntuDockerNotes.md` to build and run the Demo1Web container.
