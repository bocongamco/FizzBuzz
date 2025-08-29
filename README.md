# FizzBuzz Pro

A full-stack, customizable FizzBuzz web application. Unlike the classic FizzBuzz challenge, this version allows users to define their own games with unique rules, play timed sessions, and track high scores. Built with a .NET 8 backend and a React + Vite + TypeScript frontend, containerized with Docker Compose for simple deployment.

---

## 🌟 Features

- Define custom FizzBuzz game rules
- Timed gameplay sessions
- Persistent scores and game history
- Game dashboard with stats and high scores
- RESTful API with Swagger documentation
- Dockerized for easy setup and deployment

---

## 🧱 Tech Stack

| Layer      | Technology                          |
|------------|--------------------------------------|
| Frontend   | React, TypeScript, Vite              |
| Backend    | .NET 8 Web API (C#)                  |
| DevOps     | Docker, Docker Compose               |
| Docs       | Swagger / OpenAPI                    |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Node.js](https://nodejs.org/)
- [Docker](https://www.docker.com/)

---

## 🐳 Run with Docker Compose

```bash
docker-compose up --build
```

- **Frontend**: [http://localhost:5173](http://localhost:5173)  
- **Backend API**: [http://localhost:8080/swagger](http://localhost:8080/swagger)

---

## 🛠 Manual Build (Optional)

### Backend

```bash
cd Api
dotnet build
dotnet run
```

### Frontend

```bash
cd frontend
npm install
npm run dev
```

---

## ⚙️ Configuration

To update ports or environment settings, modify the `docker-compose.yml` file.

Example:
```yaml
environment:
  - VITE_API_BASE=http://localhost:8080
```

---

## 📂 Folder Structure

```
FizzBuzz/
├── Api/                    # .NET Backend
├── frontend/               # React Frontend (Vite + TS)
├── docker-compose.yml      # Docker orchestration
├── Dockerfile              # API Dockerfile
├── Dockerfile.dev          # Frontend Dockerfile
```

---

## 🧪 Testing

Unit and integration tests are currently under development.  
Test coverage for core components (API endpoints, game rules engine, etc.) is planned for a future release.

---

## 👨‍💻 Author

Created by [@bocongamco](https://github.com/bocongamco)  