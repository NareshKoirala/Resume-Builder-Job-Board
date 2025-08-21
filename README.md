# 📄 Resume Builder & Job Board (Full-Stack)

A **full-stack portfolio project** that combines a **Job Board** with a **Resume Builder powered by AI**.  
Users can search for jobs, create tailored resumes & cover letters, and manage their applications — across **Web** and **Mobile/Desktop apps** — all backed by a robust **ASP.NET Core Web API**.

---

## 📊 System Architecture

![Architecture Diagram](./assets/resume_builder_schema.png)

*Architecture: PostgreSQL + EF Core → ASP.NET Core Web API → Next.js Web App + MAUI App (Mobile & Desktop). Integrated with OpenAI API for resume tailoring.*

---

## 🚀 Features

### 🔧 Backend (ASP.NET Core Web API)
- RESTful endpoints for **Users** & **Jobs**
- AI-powered **Resume & Cover Letter generation**
- **Job fit scoring** (keywords + % match)
- **Swagger docs** for testing

### 🌐 Frontend (Next.js Web App)
- Responsive **dashboard** for:
  - Job browsing & filtering
  - Resume tailoring UI
  - Job application tracking
- SSR (Server-Side Rendering) for SEO & performance
- API integration with backend

### 📱 Mobile/Desktop (MAUI App)
- Cross-platform app (iOS, Android, Windows, macOS)
- **On-the-go job browsing & resume editing**
- Syncs with the same backend as the web app
- Native look & feel across devices

---

## 🌐 Live Services

- **Backend API**: [https://resume-builder-api-8m2c.onrender.com](https://resume-builder-api-8m2c.onrender.com)  
- **API Docs (Swagger)**: [Swagger UI](https://resume-builder-api-8m2c.onrender.com/swagger)  
- **Frontend Demo**: _Coming soon_ (Next.js deployment)  
- **MAUI App**: _Available as local build_  

---

## 📂 API Overview

### **UserController**
- `POST /api/User/register` → Register new user  
- `PUT /api/User/update` → Update profile  
- `DELETE /api/User/delete/{publicId}` → Soft delete user  
- `GET /api/User/getUser/{publicId}` → Fetch user profile  

### **JobController**
- `GET /api/Job/fetch-rcl-json` → Resume + Cover Letter JSON  
- `GET /api/Job/get-matchKeys` → Fit % + keywords  
- `GET /api/Job/get-all` → Full job analysis  

---

## 🛠️ Tech Stack

### Backend
- ASP.NET Core Web API  
- EF Core + PostgreSQL  
- Swagger / OpenAPI  
- OpenAI API  

### Frontend (Web)
- Next.js (React, SSR, TailwindCSS)  
- Axios for API calls  
- Deployed via Vercel/Render  

### Mobile/Desktop
- .NET MAUI  
- Shared codebase for Android, iOS, Windows, macOS  
- API integration with backend  

---

## ⚡ Quick Start

### 1. Clone the repo
```bash
git clone https://github.com/NareshKoirala/Resume-Builder-Job-Board.git
cd Resume-Builder-Job-Board
