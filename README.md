# docker-dotnetcore-mongo-rabbit-smtp
A sample project using Docker Compose to host a Dotnet Core WebApi, a RabbitMQ and MailHog SMTP server.

## What does it do ?

* Exposes an API on your localhost at http://localhost:8888/swagger
* Builds a MongoDB, with some data seeded with 100 book recomendations
* Mongo admin is accessable at http://localhost:8081/
* Builds a MailHog SMTP server, and a mail box interface at http://localhost:8025
* Invokes a RabbitMQ service, the queue can be administrated at http://localhost:15672/#/queues

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. 

My setup is : an old Dell Windows 10 Pro laptop, 4GB ram,  Docker Desktop running Linux containers

There are known issues with RabbitMQ and Windows 10 Home

I have run it on a fresh Macbook Pro and all the services run well, but there may be some issues with the dotnet core queue listeners - specifically, with parsing the path to the email template

### Building the services

From the terminal, the services can all be started and admin pages exposed in the browser by invoking
```
sh up.sh
```
Or by simply runing docker-compose up --build in the root folder should get the main services up and running

### Queue Listeners

In the Emailer folder, there is a dotnet core console application which listens to the queue and when items arrive, it emails the details the specified mail box

Items are added to the queue by calling the api end point (can use swagger @  :8888/swagger), passing an integer param which is the number of recomendations to make.

To test multiple Dotnet queue listeners, open two consoles invoking with the command:

First Console:
```
dotnet run --no-build "First Listener"
```
Second Console:
```
dotnet run --no-build "First Listener"
```

For the Go Lang listener,  Go needs to be installed along with this message queue listener package
```
go get github.com/streadway/amqp
```
The listener is invoked by
```
go run receive.go
```

When the emails are recieved, the listener name is appended to the subject text, for testing purposes.