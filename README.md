# 🚀 Fetch Data Utility

> ⚠️ This is a personal project created for demonstration and learning purposes.

A secure, read-only data retrieval system built using **.NET Core Web API**, **Angular**, and **PostgreSQL**.
Designed to allow safe data access without exposing direct database connections or modification capabilities.

---

## 📌 Project Overview

Fetch Data Utility is a full-stack application that enables controlled data access through secure APIs.
It allows users to fetch and analyze data without risking database integrity.

The system is designed with a **read-only architecture**, ensuring that no direct database modifications are possible.

---

## ✨ Key Features

* 🔐 JWT-based authentication for secure API access
* 📊 Read-only access via PostgreSQL **functions and views**
* 🔒 Base64 encrypted request payloads
* 📄 Swagger integration for API testing
* 🚫 No direct database access from frontend
* ⚡ Optimized API performance for faster data retrieval

---

## 🛠️ Tech Stack

**Frontend**

* Angular
* TypeScript
* Angular Material

**Backend**

* .NET Core Web API
* C#

**Database**

* PostgreSQL

**Tools**

* Swagger
* PgAdmin

---

## 🏗️ Project Structure

```
backend/     → .NET Core API
frontend/    → Angular UI
database/    → SQL scripts (functions, views)
```

---

## ⚙️ Quick Setup

### Backend

cd backend/FetchDataUtility.API
dotnet run

### Frontend

cd frontend/fetch-data-ui
npm install
ng serve

### Database

Run SQL scripts from `/database` folder

> Note: Update connection string and JWT keys before running the project.

---

## 🔑 Authentication Flow

1. User logs in and receives a JWT token
2. Token is sent in API request headers
3. Backend validates token before processing request

---

## 🔍 API Example

### Request

```
POST /api/data/fetch
Authorization: Bearer <token>

{
  "payload": "Base64EncodedString"
}
```

### Response

```
{
  "status": "success",
  "data": [...]
}
```

---

## 🔐 Security Design

* Read-only database access using functions and views
* No direct table exposure
* JWT-based authentication
* Encrypted request payload handling

---

## 📈 Use Cases

* Secure internal data sharing
* Debugging without DB access
* Reporting dashboards
* Controlled API-based data access

---

## 👨‍💻 Author

**Aman Mishra**
Full Stack Developer (.NET + Angular)

---

## ⭐ Support

If you found this project useful, consider giving it a ⭐ on GitHub!
