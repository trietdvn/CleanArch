# CleanArch
A solution starter for creating a instant ASP.NET Core Api following the principles of Onion Architecture

## Architecture
- Onion Architecture

## Technique Stack
- Asp.Net Core 5
- Entity Framework Core 5
- Identity Server 4
- Fluent Validation
- Auto Mapper
- Swagger
- Serilog
- Sendgrid

## Feature
- Web API
- Swagger UI 
- API Versioning
- Identity with JWT Authentication
- Role based Authorization
- Response Wrappers
- Entity Framework Core – Code First
- Repository Pattern
- Database Seeding
- Custom Exception Handling Middleware
- Account Management (Register, Forgot Password, Confirmation Mail)

## Getting Start
1. Migrate database
```
update-database -context IdentityContext
update-database -context ApplicationDbContext
```
2. Run Api
3. Get token
POST <your-host>/connect/token with following params(x-www-form-urlencoded)
grant_type=password
username=cleanarch@mailinator.com
password=Admin%40123
client_id=MyClientId
client_secret=MyClientSecret