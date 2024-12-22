FROM golang:1.21

WORKDIR /app

# Copy source code
COPY . .

# Initialize Go modules and download dependencies
RUN go mod tidy

# Build the application
RUN go build -o main .

# Command to run the executable
CMD ["./main", "Go Listener"] 