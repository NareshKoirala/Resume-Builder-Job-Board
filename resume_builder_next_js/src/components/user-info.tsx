'use client';

import {
    CertificateEntryDto, 
    EducationEntryDto,
    ProjectEntryDto,
    SkillsEntryDto,
    WorkEntryDto,
    UserRegisterDto,
    UpdateUserDto
} from '../model/data-structure'

import React, { useState, useEffect } from 'react';
import styles from '../css/user-info.module.css';

interface UserInfoProps {
    mode: 'register' | 'update';
    userInfo?: UpdateUserDto | null;
    onSubmit?: (data: UserRegisterDto | UpdateUserDto) => void;
}

const UserInfo: React.FC<UserInfoProps> = ({ mode = 'register', userInfo = null, onSubmit }) => {
    const [formData, setFormData] = useState<UserRegisterDto | UpdateUserDto>(() => {
        if (mode === 'update' && userInfo) {
            return { ...userInfo };
        }
        return {
            pass: '',
            first_name: '',
            last_name: '',
            email: '',
            mobile: '',
            location: '',
            province: '',
            job_field: '',
            portfolio_url: '',
            linkedin_url: '',
            user_summary: '',
            education: [],
            work_experience: [],
            certificates: [],
            skills: [],
            projects: []
        };
    });

    // Update form data when userInfo prop changes
    useEffect(() => {
        if (mode === 'update' && userInfo) {
            setFormData({ ...userInfo });
        }
    }, [mode, userInfo]);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        
        if (onSubmit) {
            onSubmit(formData);
        } else {
            alert(`${mode === 'update' ? 'User updated' : 'User registered'}! Check console for data.`);
        }
    };

    // Education handlers
    const addEducation = () => {
        setFormData(prev => ({
            ...prev,
            education: [...prev.education, { institution_name: '', date: '', location: '', details: '' }]
        }));
    };

    const updateEducation = (index: number, field: keyof EducationEntryDto, value: string) => {
        setFormData(prev => ({
            ...prev,
            education: prev.education.map((edu, i) =>
                i === index ? { ...edu, [field]: value } : edu
            )
        }));
    };    const removeEducation = (index: number) => {
        setFormData(prev => ({
            ...prev,
            education: prev.education.filter((_, i) => i !== index)
        }));
    };

    // Work Experience handlers
    const addWorkExperience = () => {
        setFormData(prev => ({
            ...prev,
            work_experience: [...prev.work_experience, { company_name: '', date: '', location: '', details: '' }]
        }));
    };

    const updateWorkExperience = (index: number, field: keyof WorkEntryDto, value: string) => {
        setFormData(prev => ({
            ...prev,
            work_experience: prev.work_experience.map((work, i) =>
                i === index ? { ...work, [field]: value } : work
            )
        }));
    };

    const removeWorkExperience = (index: number) => {
        setFormData(prev => ({
            ...prev,
            work_experience: prev.work_experience.filter((_, i) => i !== index)
        }));
    };    // Certificate handlers
    const addCertificate = () => {
        setFormData(prev => ({
            ...prev,
            certificates: [...prev.certificates, { certificate_name: '', details: '' }]
        }));
    };

    const updateCertificate = (index: number, field: keyof CertificateEntryDto, value: string) => {
        setFormData(prev => ({
            ...prev,
            certificates: prev.certificates.map((cert, i) => 
                i === index ? { ...cert, [field]: value } : cert
            )
        }));
    };

    const removeCertificate = (index: number) => {
        setFormData(prev => ({
            ...prev,
            certificates: prev.certificates.filter((_, i) => i !== index)
        }));
    };

    // Skills handlers
    const addSkill = () => {
        setFormData(prev => ({
            ...prev,
            skills: [...prev.skills, { skill_name: '' }]
        }));
    };

    const updateSkill = (index: number, value: string) => {
        setFormData(prev => ({
            ...prev,
            skills: prev.skills.map((skill, i) => 
                i === index ? { ...skill, skill_name: value } : skill
            )
        }));
    };

    const removeSkill = (index: number) => {
        setFormData(prev => ({
            ...prev,
            skills: prev.skills.filter((_, i) => i !== index)
        }));
    };

    // Project handlers
    const addProject = () => {
        setFormData(prev => ({
            ...prev,
            projects: [...prev.projects, { project_name: '', description: '' }]
        }));
    };

    const updateProject = (index: number, field: keyof ProjectEntryDto, value: string) => {
        setFormData(prev => ({
            ...prev,
            projects: prev.projects.map((project, i) => 
                i === index ? { ...project, [field]: value } : project
            )
        }));
    };

    const removeProject = (index: number) => {
        setFormData(prev => ({
            ...prev,
            projects: prev.projects.filter((_, i) => i !== index)
        }));
    };

    return (
        <div className={styles.userInfoContainer}>
            <form onSubmit={handleSubmit} className={styles.userInfoForm}>
                <h2 className={styles.userInfoTitle}>
                    {mode === 'update' ? 'Update User Information' : 'Register - User Information'}
                </h2>
                
                {/* Personal Information */}
                <section className={styles.formSection}>
                    <h3>Personal Information</h3>
                    {mode === 'register' && (
                        <div className={styles.formRow}>
                            <input 
                                type="password" 
                                name="pass" 
                                placeholder="Pass Key" 
                                value={(formData as UserRegisterDto).pass || ''}
                                onChange={handleInputChange}
                                className={styles.userInfoInput} 
                                required
                            />
                        </div>
                    )}
                    <div className={styles.formRow}>
                        <input 
                            type="text" 
                            name="first_name" 
                            placeholder={mode === 'update' && formData.first_name ? formData.first_name : "First Name"} 
                            value={formData.first_name}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                            required
                        />
                        <input 
                            type="text" 
                            name="last_name" 
                            placeholder={mode === 'update' && formData.last_name ? formData.last_name : "Last Name"} 
                            value={formData.last_name}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                            required
                        />
                    </div>
                    <div className={styles.formRow}>
                        <input 
                            type="email" 
                            name="email" 
                            placeholder={mode === 'update' && formData.email ? formData.email : "Email"} 
                            value={formData.email}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                            required
                        />
                        <input 
                            type="tel" 
                            name="mobile" 
                            placeholder={mode === 'update' && formData.mobile ? formData.mobile : "Mobile Number"} 
                            value={formData.mobile}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                            required
                        />
                    </div>
                    <div className={styles.formRow}>
                        <input 
                            type="text" 
                            name="location" 
                            placeholder={mode === 'update' && formData.location ? formData.location : "Location/City"} 
                            value={formData.location}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                            required
                        />
                        <input 
                            type="text" 
                            name="province" 
                            placeholder={mode === 'update' && formData.province ? formData.province : "Province/State"} 
                            value={formData.province}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                            required
                        />
                    </div>
                    <div className={styles.formRow}>
                        <input 
                            type="text" 
                            name="job_field" 
                            placeholder={mode === 'update' && formData.job_field ? formData.job_field : "Job Field/Industry"} 
                            value={formData.job_field}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                            required
                        />
                    </div>
                    <div className={styles.formRow}>
                        <input 
                            type="url" 
                            name="portfolio_url" 
                            placeholder={mode === 'update' && formData.portfolio_url ? formData.portfolio_url : "Portfolio URL (optional)"} 
                            value={formData.portfolio_url || ''}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                        />
                        <input 
                            type="url" 
                            name="linkedin_url" 
                            placeholder={mode === 'update' && formData.linkedin_url ? formData.linkedin_url : "LinkedIn URL (optional)"} 
                            value={formData.linkedin_url || ''}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                        />
                    </div>
                    <div className={styles.formRow}>
                        <textarea 
                            name="user_summary" 
                            placeholder={mode === 'update' && formData.user_summary ? formData.user_summary : "Professional Summary (optional)"} 
                            value={formData.user_summary || ''}
                            onChange={handleInputChange}
                            className={styles.userInfoTextarea} 
                            rows={4}
                        />
                    </div>
                </section>

                {/* Education */}
                <section className={styles.formSection}>
                    <div className={styles.sectionHeader}>
                        <h3>Education</h3>
                        <button type="button" onClick={addEducation} className={styles.addButton}>+ Add Education</button>
                    </div>
                    {formData.education.map((edu, index) => (
                        <div key={index} className={styles.dynamicEntry}>
                            <div className={styles.formRow}>
                                <input 
                                    type="text" 
                                    placeholder="Institution Name" 
                                    value={edu.institution_name}
                                    onChange={(e) => updateEducation(index, 'institution_name', e.target.value)}
                                    className={styles.userInfoInput} 
                                    required
                                />
                                <input 
                                    type="text" 
                                    placeholder="Date (e.g., 2020-2024)" 
                                    value={edu.date}
                                    onChange={(e) => updateEducation(index, 'date', e.target.value)}
                                    className={styles.userInfoInput} 
                                    required
                                />
                            </div>
                            <div className={styles.formRow}>
                                <input 
                                    type="text" 
                                    placeholder="Location (optional)" 
                                    value={edu.location || ''}
                                    onChange={(e) => updateEducation(index, 'location', e.target.value)}
                                    className={styles.userInfoInput} 
                                />
                                <input 
                                    type="text" 
                                    placeholder="Details (optional)" 
                                    value={edu.details || ''}
                                    onChange={(e) => updateEducation(index, 'details', e.target.value)}
                                    className={styles.userInfoInput} 
                                />
                            </div>
                            <button type="button" onClick={() => removeEducation(index)} className={styles.removeButton}>Remove</button>
                        </div>
                    ))}
                </section>

                {/* Work Experience */}
                <section className={styles.formSection}>
                    <div className={styles.sectionHeader}>
                        <h3>Work Experience</h3>
                        <button type="button" onClick={addWorkExperience} className={styles.addButton}>+ Add Work Experience</button>
                    </div>
                    {formData.work_experience.map((work, index) => (
                        <div key={index} className={styles.dynamicEntry}>
                            <div className={styles.formRow}>
                                <input 
                                    type="text" 
                                    placeholder="Company Name" 
                                    value={work.company_name}
                                    onChange={(e) => updateWorkExperience(index, 'company_name', e.target.value)}
                                    className={styles.userInfoInput} 
                                    required
                                />
                                <input 
                                    type="text" 
                                    placeholder="Date (e.g., Jan 2020 - Dec 2022)" 
                                    value={work.date}
                                    onChange={(e) => updateWorkExperience(index, 'date', e.target.value)}
                                    className={styles.userInfoInput} 
                                    required
                                />
                            </div>
                            <div className={styles.formRow}>
                                <input 
                                    type="text" 
                                    placeholder="Location (optional)" 
                                    value={work.location || ''}
                                    onChange={(e) => updateWorkExperience(index, 'location', e.target.value)}
                                    className={styles.userInfoInput} 
                                />
                                <input 
                                    type="text" 
                                    placeholder="Job Details (optional)" 
                                    value={work.details || ''}
                                    onChange={(e) => updateWorkExperience(index, 'details', e.target.value)}
                                    className={styles.userInfoInput} 
                                />
                            </div>
                            <button type="button" onClick={() => removeWorkExperience(index)} className={styles.removeButton}>Remove</button>
                        </div>
                    ))}
                </section>

                {/* Certificates */}
                <section className={styles.formSection}>
                    <div className={styles.sectionHeader}>
                        <h3>Certificates</h3>
                        <button type="button" onClick={addCertificate} className={styles.addButton}>+ Add Certificate</button>
                    </div>
                    {formData.certificates.map((cert, index) => (
                        <div key={index} className={styles.dynamicEntry}>
                            <div className={styles.formRow}>
                                <input 
                                    type="text" 
                                    placeholder="Certificate Name" 
                                    value={cert.certificate_name}
                                    onChange={(e) => updateCertificate(index, 'certificate_name', e.target.value)}
                                    className={styles.userInfoInput} 
                                    required
                                />
                                <input 
                                    type="text" 
                                    placeholder="Details" 
                                    value={cert.details}
                                    onChange={(e) => updateCertificate(index, 'details', e.target.value)}
                                    className={styles.userInfoInput} 
                                    required
                                />
                            </div>
                            <button type="button" onClick={() => removeCertificate(index)} className={styles.removeButton}>Remove</button>
                        </div>
                    ))}
                </section>

                {/* Skills */}
                <section className={styles.formSection}>
                    <div className={styles.sectionHeader}>
                        <h3>Skills</h3>
                        <button type="button" onClick={addSkill} className={styles.addButton}>+ Add Skill</button>
                    </div>
                    {formData.skills.map((skill, index) => (
                        <div key={index} className={styles.dynamicEntry}>
                            <div className={styles.formRow}>
                                <input 
                                    type="text" 
                                    placeholder="Skill Name" 
                                    value={skill.skill_name}
                                    onChange={(e) => updateSkill(index, e.target.value)}
                                    className={styles.userInfoInput} 
                                    required
                                />
                                <button type="button" onClick={() => removeSkill(index)} className={styles.removeButton}>Remove</button>
                            </div>
                        </div>
                    ))}
                </section>

                {/* Projects */}
                <section className={styles.formSection}>
                    <div className={styles.sectionHeader}>
                        <h3>Projects</h3>
                        <button type="button" onClick={addProject} className={styles.addButton}>+ Add Project</button>
                    </div>
                    {formData.projects.map((project, index) => (
                        <div key={index} className={styles.dynamicEntry}>
                            <div className={styles.formRow}>
                                <input 
                                    type="text" 
                                    placeholder="Project Name" 
                                    value={project.project_name}
                                    onChange={(e) => updateProject(index, 'project_name', e.target.value)}
                                    className={styles.userInfoInput} 
                                    required
                                />
                            </div>
                            <div className={styles.formRow}>
                                <textarea 
                                    placeholder="Project Description (optional)" 
                                    value={project.description || ''}
                                    onChange={(e) => updateProject(index, 'description', e.target.value)}
                                    className={styles.userInfoTextarea} 
                                    rows={3}
                                />
                            </div>
                            <button type="button" onClick={() => removeProject(index)} className={styles.removeButton}>Remove</button>
                        </div>
                    ))}
                </section>

                <button type="submit" className={styles.userInfoSubmit}>
                    {mode === 'update' ? 'Update User Information' : 'Register User'}
                </button>
            </form>
        </div>
    );
};

export default UserInfo;
