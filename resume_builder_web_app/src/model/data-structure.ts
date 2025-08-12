// DTO interfaces for API communication

export interface CertificateEntryDto {
  certificateName: string;
  details: string;
}

export interface EducationEntryDto {
  institutionName: string;
  date: string;
  location?: string;
  details?: string;
}

export interface ProjectEntryDto {
  projectName: string;
  description?: string;
}

export interface SkillsEntryDto {
  skillName: string;
}

export interface WorkEntryDto {
  companyName: string;
  date: string;
  location?: string;
  details?: string;
}

// User Registration DTO
export interface UserRegisterDto {
  pass: string;
  firstName: string;
  lastName: string;
  email: string;
  mobile: string;
  location: string;
  province: string;
  jobField: string;
  portfolioUrl?: string;
  linkedInUrl?: string;
  userSummary?: string;
  education: EducationEntryDto[];
  workExperience: WorkEntryDto[];
  certificates: CertificateEntryDto[];
  skills: SkillsEntryDto[];
  projects: ProjectEntryDto[];
}

// User Update DTO
export interface UpdateUserDto {
  publicId: string;
  firstName: string;
  lastName: string;
  email: string;
  mobile: string;
  location: string;
  province: string;
  jobField: string;
  portfolioUrl?: string;
  linkedInUrl?: string;
  userSummary?: string;
  education: EducationEntryDto[];
  workExperience: WorkEntryDto[];
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
