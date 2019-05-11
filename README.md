# CyberPlus Angular-WebAPI project

This repository contains Web App built on Angular 7 that interacts with WebAPI which has MySQL database

## Getting Started

### Prerequisites

- Node.js
- Angular CLI
- .NET Core Framework

### Edit WebAPI config

Settings are located in [appsettings.json](WebApi/appsettings.json). Change `insert_here` to your own keys.
 
- ConnectionString
- JWT SecretKey
- Email/SendGridAPIKey [how to create SendGrid?](https://docs.microsoft.com/en-us/azure/sendgrid-dotnet-how-to-send-email)

### Code-first database migration

Generate database `dotnet ef migrations add InitialMigration` then `dotnet ef database update`

### Run project

Run `ASPNETCORE_Environment=Development dotnet run` to build project.

## Functionality Overview

### Features

* Cross Platform
* CRUD operations
* Entity Framework Core MySQL
* JWT Authentication
* Swagger API documentation
* Responsive Design

### WebAPI documentation

Online API documentation is located on [/Swagger](http://ideashareapp.azurewebsites.net/swagger/)

### Run Mysql Database in Docker Container.

* # docker-compose up -d

## Built With

* **ASP.NET Core 2.2 WebAPI**
* **Angular 7**
* **MySQL**

## Roadmap

- [ ] Localization
- [ ] WebAPI.Tests
- [ ] Angular Client App

## Authors

* **Ing. Yasmany Santalla Pereda** - [ysantalla](https://github.com/ysantalla)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details



