# Resume Builder API 📄

Welcome to the **Resume Builder API**, a robust ASP.NET Core-based API designed to programmatically create, manage, and export professional resumes and cover letters. This API enables developers to build tailored resumes and cover letters based on user-provided job details, calculate job-matching percentages, and identify key skills for improvement.

[![GitHub license](https://img.shields.io/github/license/NareshKoirala/Resume-Builder-API)](https://github.com/NareshKoirala/Resume-Builder-API/blob/main/LICENSE)
[![Portfolio](https://img.shields.io/badge/Portfolio-Visit-blue)](https://www.nareshkoirala.dev)

## Table of Contents
- [Overview](#overview)
- [Authentication](#authentication)
- [Model Structure](#model-structure)
- [API Endpoints](#api-endpoints)
- [Code Examples](#code-examples)
- [Support](#support)

## Overview
The Resume Builder API simplifies the creation of professional resumes and cover letters by allowing developers to:
- **Generate Resumes and Cover Letters**: Build customized documents with sections like personal info, experience, and skills.
- **Job Matching**: Calculate a job-matching percentage to show how well a user’s profile aligns with job requirements and highlight skills to improve.
- **User Management**: Manage user profiles and their associated resumes.
- **Export Options**: Currently supports JSON output, with plans for PDF, Word, and HTML exports.
- **Template Management**: Planned feature for selecting industry-specific resume and cover letter templates (currently unavailable).

**Base URL**: `https://resume-builder-api-8m2c.onrender.com`

## Authentication
Access to the API requires an admin passcode. To obtain one:
1. Contact Naresh Koirala for the passcode.
2. Submit general inquiries or feature requests.
3. Save the provided passcode for API authentication.

📧 **Contact**: [chelseanaresh10@gmail.com](mailto:chelseanaresh10@gmail.com)

## Model Structure
The API uses specific data models for requests and responses. Below are the key structures:

### Job Data Model (`returnJob`)
```csharp
{
    string? JobName { get; set; } = string.Empty;
    ResumeModel? Resume { get; set; } = new ResumeModel();
    CoverLetterModel? CoverLetter { get; set; } = new CoverLetterModel();
    string? JobKeywords { get; set; } = string.Empty;
    string? JobPercentage { get; set; } = string.Empty;
}
```

#### Resume Model (`ResumeModel`)
```csharp
{
    string TitleKeyword { get; set; } = string.Empty;
    string Summary { get; set; } = string.Empty;
    string ProgramingLanguage { get; set; } = string.Empty;
    string Frameworks { get; set; } = string.Empty;
    string RelevantKeywords { get; set; } = string.Empty;
    List<ProjectsModel> Projects { get; set; } = new List<ProjectsModel>();
    string WorkExperience { get; set; } = string.Empty;
    string Education { get; set; } = string.Empty;
    string Certificates { get; set; } = string.Empty;
}
```

#### Projects Model (`ProjectsModel`)
```csharp
{
    string ProjectName { get; set; } = string.Empty;
    string Description { get; set; } = string.Empty;
    string Bullet1 { get; set; } = string.Empty;
    string Bullet2 { get; set; } = string.Empty;
    string Bullet3 { get; set; } = string.Empty;
}
```

#### Cover Letter Model (`CoverLetterModel`)
```csharp
{
    string Title { get; set; } = string.Empty;
    string Body1 { get; set; } = string.Empty;
    string Body2 { get; set; } = string.Empty;
    string Body3 { get; set; } = string.Empty;
}
```

### User Registration/Update Models
#### User Registration (`UserRegisterDto`)
```csharp
{
    string pass { get; set; } = string.Empty;
    string firstName { get; set; } = string.Empty;
    string lastName { get; set; } = string.Empty;
    string email { get; set; } = string.Empty;
    string mobile { get; set; } = string.Empty;
    string location { get; set; } = string.Empty;
    string province { get; set; } = string.Empty;
    string jobField { get; set; } = string.Empty;
    string? portfolioUrl { get; set; } = null;
    string? linkedInUrl { get; set; } = null;
    string? userSummary { get; set; } = null;
    List<EducationEntryDto> education { get; set; } = new List<EducationEntryDto>();
    List<WorkEntryDto> workExperience { get; set; } = new List<WorkEntryDto>();
    List<CertificateEntryDto> certificates { get; set; } = new List<CertificateEntryDto>();
    List<SkillsEntryDto> skills { get; set; } = new List<SkillsEntryDto>();
    List<ProjectEntryDto> projects { get; set; } = new List<ProjectEntryDto>();
}
```

#### User Update (`UpdateUserDto`)
```csharp
{
    string publicId { get; set; } = string.Empty;
    string firstName { get; set; } = string.Empty;
    string lastName { get; set; } = string.Empty;
    string email { get; set; } = string.Empty;
    string mobile { get; set; } = string.Empty;
    string location { get; set; } = string.Empty;
    string province { get; set; } = string.Empty;
    string jobField { get; set; } = string.Empty;
    string? portfolioUrl { get; set; } = null;
    string? linkedInUrl { get; set; } = null;
    string? userSummary { get; set; } = null;
    List<EducationEntryDto> education { get; set; } = new List<EducationEntryDto>();
    List<WorkEntryDto> workExperience { get; set; } = new List<WorkEntryDto>();
    List<CertificateEntryDto> certificates { get; set; } = new List<CertificateEntryDto>();
    List<SkillsEntryDto> skills { get; set; } = new List<SkillsEntryDto>();
    List<ProjectEntryDto> projects { get; set; } = new List<ProjectEntryDto>();
}
```

#### Supporting Models
- **CertificateEntryDto**:
```csharp
{
    string certificateName { get; set; } = string.Empty;
    string details { get; set; } = string.Empty;
}
```
- **EducationEntryDto**:
```csharp
{
    string institutionName { get; set; } = string.Empty;
    string date { get; set; } = string.Empty;
    string? location { get; set; } = null;
    string? details { get; set; } = null;
}
```
- **ProjectEntryDto**:
```csharp
{
    string projectName { get; set; } = string.Empty;
    string? description { get; set; } = null;
}
```
- **SkillsEntryDto**:
```csharp
{
    string skillName { get; set; } = string.Empty;
}
```
- **WorkEntryDto**:
```csharp
{
    string companyName { get; set; } = string.Empty;
    string date { get; set; } = string.Empty;
    string? location { get; set; } = null;
    string? details { get; set; } = null;
}
```

## API Endpoints
The API is organized into `JobController` and `UserController`. Below are the available endpoints:

### JobController
| Endpoint                     | Method | Description                                      | Request Body                              | Response Body                              |
|------------------------------|--------|--------------------------------------------------|-------------------------------------------|--------------------------------------------|
| `/Job/fetch-rcl-json`        | GET    | Fetch resume and cover letter JSON data          | `{ publicId, jobName, jobDescription }`   | `{ jobName, resumeJson, coverLetterJson }` |
| `/Job/get-matchKeys`         | GET    | Get job match percentage and key skills to improve | `{ publicId, jobName, jobDescription }`   | `{ jobName, matchPercentage, keySkills }`  |
| `/Job/get-all`               | GET    | Get complete job analysis with resume, cover letter, and match data | `{ publicId, jobName, jobDescription }` | `{ jobName, resume, coverLetter, jobKeywords, jobPercentage }` |

### UserController
| Endpoint                     | Method | Description                                      | Parameters/Body                           | Response Body                              |
|------------------------------|--------|--------------------------------------------------|-------------------------------------------|--------------------------------------------|
| `/User/register`             | POST   | Register a new user                              | `UserRegisterDto` JSON                    | Success message and public ID             |
| `/User/update`               | PUT    | Update an existing user                          | `UpdateUserDto` JSON                      | Success message                           |
| `/User/delete/{publicId}`    | DELETE | Delete a user by public ID                       | `publicId` (path)                         | Success message                           |
| `/User/getUser/{publicId}`   | GET    | Retrieve a user by public ID                     | `publicId` (path)                         | User details JSON                         |

## Code Examples
> **Note**: Code examples will be added in the future. For now, use tools like Postman or `curl` to test endpoints with the provided model structures.

Example `curl` request for `/Job/fetch-rcl-json`:
```bash
curl -X GET "https://resume-builder-api-8m2c.onrender.com/Job/fetch-rcl-json" \
-H "Authorization: Bearer <your-passcode>" \
-H "Content-Type: application/json" \
-d '{"publicId": "user123", "jobName": "Software Engineer", "jobDescription": "Requires C# and ASP.NET Core experience"}'
```

## Support
For issues, feature requests, or support, contact:

- **GitHub**: [@NareshKoirala](https://github.com/NareshKoirala)
- **Email**: [chelseanaresh10@gmail.com](mailto:chelseanaresh10@gmail.com)
- **Portfolio**: [nareshkoirala.dev](https://www.nareshkoirala.dev/contact)

---

Thank you for exploring the Resume Builder API! 🌟
