#!/bin/bash

# Function to check if Docker Desktop is running
check_docker() {
    if ! docker info > /dev/null 2>&1; then
        return 1
    fi
    return 0
}

# Function to start Docker Desktop on macOS
start_docker_desktop() {
    echo "Starting Docker Desktop..."
    open -a Docker
}

# For macOS, ensure Docker Desktop is running
if [[ "$OSTYPE" == "darwin"* ]]; then
    if ! check_docker; then
        start_docker_desktop
        echo "Waiting for Docker Desktop to start..."
        while ! check_docker; do
            echo "Docker Desktop is not ready yet... waiting 5 seconds"
            sleep 5
        done
        echo "Docker Desktop is now running!"
    fi
fi

case "$OSTYPE" in
solaris*) echo "SOLARIS" ;;
darwin*)
    echo "OSX"
    open "http://localhost:8888"
    open "http://localhost:8081"
    open "http://localhost:8025"
    open "http://localhost:15672"
    open "http://localhost:8080"
    ;;
linux*) echo "LINUX" ;;
bsd*) echo "BSD" ;;
msys*)
    echo "WINDOWS"
    start "http://localhost:8888"
    start "http://localhost:8081"
    start "http://localhost:8025"
    start "http://localhost:15672"
    start "http://localhost:8080"
    ;;
*) echo "unknown: $OSTYPE" ;;
esac

docker-compose up
