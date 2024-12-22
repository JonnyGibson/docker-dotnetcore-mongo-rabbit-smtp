FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .

# Copy templates
COPY Templates /app/Templates

# Default environment variables that can be overridden
ENV RABBITMQ_HOST=JGMQ \
    RABBITMQ_USER=store \
    RABBITMQ_PASSWORD=password \
    MAIL_HOST=mail \
    MAIL_PORT=1025

ENTRYPOINT ["dotnet", "Emailer.dll", "Dotnet Listener"]