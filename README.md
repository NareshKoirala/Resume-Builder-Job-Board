# 📄 Resume Builder MAUI App

An intelligent cross-platform application built using **.NET MAUI** and **MVVM architecture**, designed to fetch real-time jobs from multiple APIs, analyze job matches with user profiles using **OpenAI**, and generate tailored resumes and cover letters — all downloadable in `.docx` format.

> **Tech Stack**: .NET MAUI • C# • SQLite • REST APIs • OpenAI API • Xceed DocX

---

## ✨ Features

### 🔍 Job Search
- Fetches job listings from:
  - [Remotive API](https://remotive.io/api-documentation)
  - [JSearch API (RapidAPI)](https://rapidapi.com/)
- Smooth infinite scroll with threshold loading to avoid UI overload.

### 👤 User Profile
- Add, update, or delete user information.
- Stored securely in local `SQLite` database.
- Personal data includes name, experience, skills, and education.

### 🧠 OpenAI Resume Generator
- Calculates **job-to-user match percentage**.
- Identifies **missing job requirements** based on job description and user profile.
- Uses **custom-engineered OpenAI prompts** to:
  - Generate tailored **resume** and **cover letter**.
  - Download output as `.docx` using **Xceed DocX**.

### 📄 Job Interaction Flow
- Tap **Details** to view job information.
- Tap **Generate** to move the job to the Resume tab and begin customization.
- Includes:
  - "Apply Now" button with direct job link.
  - "Generate Docs" to trigger resume and cover letter creation.

### 🌐 Job Scraping
- **LinkedIn**: Paste job URL and scrape relevant information.
- **Indeed**: Manually input title, company, description, and link (due to scraping protection).

---

## 🖼️ Screenshots
![Main DashBoard Tab](Resume%20Builder%20Pictures/DashBoard.png)

![Job Generating Tab](Resume%20Builder%20Pictures/JobGenTab.png)

![User Setting Tab](Resume%20Builder%20Pictures/UserTab.png)

---

## 🧱 Architecture

Follows the **MVVM (Model-View-ViewModel)** pattern:
- ├── Models # User, Job, Response models
- ├── ViewModels # UI logic and command binding
- ├── Views # XAML UI pages
- ├── Services # API requests, SQLite DB, OpenAI integration
- ├── Utils # Helper functions, converters
- └── Resources # Images, styles

- **SQLite**: For local persistent user data.
- **HTTPClient + JSON**: For calling external APIs.
- **OpenAIService**: For prompt creation and response handling.
- **DocX Generation**: Create and export `.docx` documents locally.

---

📈 Future Improvements
- 🔐 Encrypt SQLite user data
- 🧪 Add unit tests for services and viewmodels
- 🌍 Deploy to Android/iOS stores
- 🧩 Add more job board integrations (e.g., Workopolis, Monster)
- 🖼️ Add light/dark theme toggle and advanced UI animations

---

🙌 Acknowledgements
Thanks to OpenAI for enabling intelligent resume generation.
Inspired by modern job hunting frustrations and the need for automated personalization tools (jobright.ai).

---

#🧑‍💻 Author
##Naresh Koirala
###💼 [LinkedIn](www.linkedin.com/in/naresh-koirala-6205582b3)
###📫 [Email](mailto:chelseanaresh10@gmail.com)



