# 📄 Resume Builder & Job Board

A **production-style portfolio project** that combines a **Job Board** with a **Resume Builder powered by AI**.  
It provides APIs for managing users, tailoring resumes to job descriptions, generating cover letters, and evaluating job fit.  

Built with **ASP.NET Core Web API**, **PostgreSQL + EF Core**, **MAUI** for mobile, and **Next.Js** for web.

---

## 📊 System Architecture

![Architecture Diagram](./assets/resume_builder_schema.png)

*Diagram shows API controllers, endpoints, integrations (OpenAI API, Swagger, PostgreSQL), and clients (Web App + MAUI App).*

---

## 🚀 Features

- 🔑 **Authentication**
  - Secure login system (JWT planned, currently simple auth).
  - Role separation for users/admins.

- 👤 **User Management**
  - Create, update, delete, and retrieve user profiles.
  - Rich schema: skills, education, experience, projects, certificates, summary.

- 📂 **Resume & Cover Letter Builder**
  - Tailors documents to job descriptions.
  - Generates structured **JSON** for resumes and cover letters.
  - Planned: export to **PDF/DOCX/HTML**.

- 🎯 **Job Matching**
  - Analyzes job descriptions and calculates **fit percentage**.
  - Extracts keywords & suggests improvements.

- 📱 **Clients**
  - **Web App** (for browsing, testing, tailoring).
  - **MAUI Mobile App** (for resume editing and applying on the go).

---

## 🌐 Live API

The project is hosted and publicly accessible:  

👉 **Base URL**: `https://resume-builder-api-8m2c.onrender.com`  

Example endpoints:  
- `GET /api/User/getUser/{publicId}` → Retrieve user profile  
- `POST /api/User/register` → Register new user  
- `GET /api/Job/fetch-rcl-json` → Generate Resume + Cover Letter JSON  
- `GET /api/Job/get-matchKeys` → Job fit analysis  

Full docs available via Swagger at:  
👉 [API Docs](https://resume-builder-api-8m2c.onrender.com/swagger)

---

## 📂 API Overview

### **UserController**
| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/User/register` | POST | Register new user |
| `/api/User/update` | PUT | Update user profile |
| `/api/User/delete/{publicId}` | DELETE | Soft-delete user |
| `/api/User/getUser/{publicId}` | GET | Get user profile |

### **JobController**
| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/Job/fetch-rcl-json` | GET | Generate resume + cover letter JSON |
| `/api/Job/get-matchKeys` | GET | Get job fit % + keywords |
| `/api/Job/get-all` | GET | Full job analysis (resume + cover + fit data) |

---

## 🛠️ Tech Stack

- **Backend**: ASP.NET Core Web API  
- **Database**: PostgreSQL + EF Core  
- **AI Integration**: OpenAI API  
- **Docs**: Swagger / OpenAPI  
- **Clients**:  
  - Web App (React planned)  
  - MAUI Mobile App  

---

## ⚡ Quick Start

### Clone & Run Locally
```bash
git clone https://github.com/NareshKoirala/Resume-Builder-Job-Board.git
cd Resume-Builder-Job-Board/resume_builder_api
dotnet restore
dotnet run
