# Resume Builder App

An intelligent cross-platform application built using .NET MAUI (Desktop & Mobile) and a Web App (Next.js), designed to fetch real-time jobs from multiple APIs, analyze job matches with user profiles using OpenAI, and generate tailored resumes and cover letters — all downloadable in `.docx` format.

> **Tech Stack:** .NET MAUI • C# • SQLite • REST APIs • OpenAI API • Xceed DocX • Next.js • React • Tailwind CSS • Vercel

![Main DashBoard Tab](Resume%20Builder%20Pictures/DashBoard.png)

![Job Generating Tab](Resume%20Builder%20Pictures/JobGenTab.png)

![User Setting Tab](Resume%20Builder%20Pictures/UserTab.png)

## ✨ Features

### Job Search

- Fetches job listings from:
  - [Remotive API](https://remotive.io)
  - [JSearch API (RapidAPI)](https://rapidapi.com)
- Filters jobs by title, location, and category.
- Displays job details including company, description, and application link.

### Profile Builder

- Allows users to create and manage personal profiles.
- Collects information such as name, contact details, skills, experience, and education.
- Saves profiles locally using SQLite (Desktop & Mobile) or a database in the Web App.

### Resume & Cover Letter Generation

- Analyzes job descriptions and user profiles using OpenAI API.
- Generates tailored resumes and cover letters in `.docx` format.
- Provides download options for both documents.

### Document Management

- Saves generated documents locally (Desktop & Mobile).
- Web App version allows document management and viewing online.

## 🛠️ Technologies Used

### Desktop & Mobile App

- **Frontend:** .NET MAUI, C#
- **Backend:** SQLite, REST APIs
- **AI Integration:** OpenAI API
- **Document Generation:** Xceed DocX
- **Architecture:** MVVM

### Web App

- **Frontend:** Next.js, React, Tailwind CSS
- **Backend:** Node.js, API Routes
- **AI Integration:** OpenAI API
- **Deployment:** Vercel

## 📦 Setup Instructions

### Desktop & Mobile App

1. Clone the repository:
   ```bash
   git clone https://github.com/NareshKoirala/Resume-Builder-App.git
