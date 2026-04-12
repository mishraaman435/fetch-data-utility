Fetch Data Utility

A secure full-stack data retrieval system built using .NET Core, Angular, and PostgreSQL.

This application allows users to safely fetch and explore database data without direct database access or risk of data modification.

--------------------------------------------------

PROJECT OVERVIEW

Fetch Data Utility provides controlled, read-only access to database data through APIs.

Users can:
- Execute only SELECT queries
- Access functions, views, and stored procedures
- Work in a secure and restricted environment

This project was developed during my internship to solve real-world data access and debugging challenges.

--------------------------------------------------

FEATURES

- Secure API access using JWT authentication
- Read-only architecture (no INSERT/UPDATE/DELETE)
- Execute custom SELECT queries safely
- Access database functions, views, stored procedures
- Base64 encryption for request payloads
- Swagger for API testing

--------------------------------------------------

SCREENSHOTS

Screenshot1 - Landing Page
Screenshot2 - Script Viewer (Functions, Views, Stored Procedure)
Screenshot3 - Query Executor 1
Screenshot4 - Query Executor 2
Screenshot5 - Query Executor 3
Screenshot6 - Executed Query Viewer

--------------------------------------------------

TECH STACK

Frontend:
- Angular 12
- TypeScript

Backend:
- .NET Core Web API
- C#

Database:
- PostgreSQL

Tools:
- Swagger
- PgAdmin

--------------------------------------------------

SECURITY DESIGN

- Only SELECT queries allowed
- No direct database modification
- API-level validation
- Base64 encrypted payloads
- JWT authentication

--------------------------------------------------

SETUP INSTRUCTIONS

1. Clone Repository
git clone https://github.com/your-username/fetch-data-utility.git
cd fetch-data-utility

2. Backend Setup
cd backend
dotnet restore
dotnet run

Update connection string in appsettings.json

3. Frontend Setup
cd frontend
npm install
ng serve

App runs on http://localhost:4200

--------------------------------------------------

USE CASES

- Secure data access
- Debugging queries
- Internal reporting tools
- Controlled data sharing

--------------------------------------------------

AUTHOR

Aman Mishra
Full Stack Developer (.NET Core + Angular)
