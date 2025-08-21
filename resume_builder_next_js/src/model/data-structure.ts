// DTO interfaces for API communication

export interface CertificateEntryDto {
  certificate_name: string;
  details: string;
}

export interface EducationEntryDto {
  institution_name: string;
  date: string;
  location?: string;
  details?: string;
}

export interface ProjectEntryDto {
  project_name: string;
  description?: string;
}

export interface SkillsEntryDto {
  skill_name: string;
}

export interface WorkEntryDto {
  company_name: string;
  date: string;
  location?: string;
  details?: string;
}

// User Registration DTO
export interface UserRegisterDto {
  pass: string;
  first_name: string;
  last_name: string;
  email: string;
  mobile: string;
  location: string;
  province: string;
  job_field: string;
  portfolio_url?: string;
  linkedin_url?: string;
  user_summary?: string;
  education: EducationEntryDto[];
  work_experience: WorkEntryDto[];
  certificates: CertificateEntryDto[];
  skills: SkillsEntryDto[];
  projects: ProjectEntryDto[];
}

// User Update DTO
export interface UpdateUserDto {
  publicId?: string;
  first_name: string;
  last_name: string;
  email: string;
  mobile: string;
  location: string;
  province: string;
  job_field: string;
  portfolio_url?: string;
  linkedin_url?: string;
  user_summary?: string;
  education: EducationEntryDto[];
  work_experience: WorkEntryDto[];
  certificates: CertificateEntryDto[];
  skills: SkillsEntryDto[];
  projects: ProjectEntryDto[];
}

// Job Return Models

export interface ProjectsModel {
  projectName: string;
  description: string;
  bullet1: string;
  bullet2: string;
  bullet3: string;
}

export interface ResumeModel {
  titleKeyword: string;
  summary: string;
  programingLanguage: string;
  frameworks: string;
  relevantKeywords: string;
  projects: ProjectsModel[];
  workExperience: string;
  education: string;
  certificates: string;
}

export interface CoverLetterModel {
  title: string;
  body1: string;
  body2: string;
  body3: string;
}

export interface ReturnJob {
  jobName?: string;
  resume?: ResumeModel;
  coverLetter?: CoverLetterModel;
  jobKeywords?: string;
  jobPercentage?: string;
}

export interface credentials {
  email: string;
  password_hash: string;
  user_id: string;
}