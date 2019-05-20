# Angular-WebAPI project

This repository contains Web App built on Angular 7 that interacts with WebAPI in .Net Core which has MySQL database

## Getting Started

### Prerequisites

- Node.js
- Angular CLI
- .NET Core Framework

### Edit WebAPI config

Settings are located in [appsettings.json](WebApi/appsettings.json). Change `insert_here` to your own keys.
 
- ConnectionString
- JWT SecretKey

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

## Built With

* **ASP.NET Core 2.2 WebAPI**
* **Angular 7**
* **MySQL**

## Authors

* **Raydel Alvarez Ramirez**

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details



