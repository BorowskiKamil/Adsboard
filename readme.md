# Adsboard 📋
The goal of this project is just a showcase of one of many approches to microservices in .NET. It's written in .NET Core 2.2 with communication over RabbitMQ.

### How to use:
The recommended way of running this project is using 🐳 docker compose. You can find compose files [here](https://github.com/BorowskiKamil/Adsboard/tree/master/Adsboard/compose) and run them directly from this folder in following order:
```
docker-compose -f mysql-rabbitmq-redis.yml up
docker-compose -f services.yml up
```
It's required to run both of commands. By executing first one you'll run dependencies, infrastructural services like mysql, rabbitmq and redis. Second command runs project's services.

There's also possibility to run services by going to particual service's project and executing ```dotnet run``` in ```src/[PROJECT_NAME]```.


### Used technologies:
- ASP.NET Core 2.2
- ASP.NET WebApi Core
- Entity Framework Core 2.2
- RawRabbit
- AutoMapper
- FluentValidation
- MySQL
- Polly
- RestEase
- Swagger
- Docker

### Covered topics:
- RESTful API implementation
- Asynchronous communication across internal microservices
- Microservices communication based on message broker RabbitMQ 
- Retries with exponential backoff
- Integration tests
- DDD (Domain Driven Design) fundamentals
- CQRS
- Integration events
- JWT authentication

### Disclamer
This project uses a lot of architecture implementations that might be over-engineering for such a small project. Beware, it's not a definitive solution and production-ready code.