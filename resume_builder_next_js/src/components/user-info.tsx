'use client';

import {
    CertificateEntryDto, 
    EducationEntryDto,
    ProjectEntryDto,
    SkillsEntryDto,
    WorkEntryDto,
    UpdateUserDto
} from '../model/data-structure'

import React, { useState, useEffect } from 'react';
import styles from '../css/user-info.module.css';

interface UserInfoProps {
    userInfo?: UpdateUserDto | null;
    onSubmit?: (data: UpdateUserDto) => void;
}

const UserInfo: React.FC<UserInfoProps> = ({ userInfo = null, onSubmit }) => {
    const [formData, setFormData] = useState<UpdateUserDto>(() => {
        if (userInfo) {
            return { ...userInfo };
        }
        return {
            firstName: '',
            lastName: '',
            email: '',
            mobile: '',
            location: '',
            province: '',
            jobField: '',
            portfolioUrl: '',
            linkedInUrl: '',
            userSummary: '',
            education: [],
            workExperience: [],
            certificates: [],
            skills: [],
            projects: []
        };
    });

    // Update form data when userInfo prop changes
    useEffect(() => {
        if (userInfo) {
            setFormData({ ...userInfo });
        }
    }, [userInfo]);

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
            alert('User updated! Check console for data.');
        }
    };

    // Education handlers
    const addEducation = () => {
        setFormData(prev => ({
            ...prev,
            education: [...prev.education, { institutionName: '', date: '', location: '', details: '' }]
        }));
    };
    const updateEducation = (index: number, field: keyof EducationEntryDto, value: string) => {
        setFormData(prev => ({
            ...prev,
            education: prev.education.map((edu, i) =>
                i === index ? { ...edu, [field]: value } : edu
            )
        }));
    };
    const removeEducation = (index: number) => {
        setFormData(prev => ({
            ...prev,
            education: prev.education.filter((_, i) => i !== index)
        }));
    };

    // Work Experience handlers
    const addWorkExperience = () => {
        setFormData(prev => ({
            ...prev,
            workExperience: [...prev.workExperience, { companyName: '', date: '', location: '', details: '' }]
        }));
    };

    const updateWorkExperience = (index: number, field: keyof WorkEntryDto, value: string) => {
        setFormData(prev => ({
            ...prev,
            workExperience: prev.workExperience.map((work, i) =>
                i === index ? { ...work, [field]: value } : work
            )
        }));
    };

    const removeWorkExperience = (index: number) => {
        setFormData(prev => ({
            ...prev,
            workExperience: prev.workExperience.filter((_, i) => i !== index)
        }));
    };

    // Certificate handlers
    const addCertificate = () => {
        setFormData(prev => ({
            ...prev,
            certificates: [...prev.certificates, { certificateName: '', details: '' }]
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
            skills: [...prev.skills, { skillName: '' }]
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
            projects: [...prev.projects, { projectName: '', description: '' }]
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
                    Update User Information
                </h2>
                
                {/* Personal Information */}
                <section className={styles.formSection}>
                    <h3>Personal Information</h3>
                    <div className={styles.formRow}>
                        <input 
                            type="text" 
                            name="firstName" 
                            placeholder={formData.firstName ? formData.firstName : "First Name"} 
                            value={formData.firstName}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                            disabled={formData.firstName !== ''}
                            required
                        />
                        <input 
                            type="text" 
                            name="lastName" 
                            placeholder={formData.lastName ? formData.lastName : "Last Name"} 
                            value={formData.lastName}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                            disabled={formData.lastName !== ''}
                            required
                        />
                    </div>
                    <div className={styles.formRow}>
                        <input 
                            type="email" 
                            name="email" 
                            placeholder={formData.email ? formData.email : "Email"} 
                            value={formData.email}
                            disabled
                            className={styles.userInfoInput} 
                        />
                        <input 
                            type="tel" 
                            name="mobile" 
                            placeholder={formData.mobile ? formData.mobile : "Mobile Number"} 
                            value={formData.mobile}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                            disabled={formData.mobile !== ''}
                            required
                        />
                    </div>
                    <div className={styles.formRow}>
                        <input 
                            type="text" 
                            name="location" 
                            placeholder={formData.location ? formData.location : "Location/City"} 
                            value={formData.location}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                            required
                        />
                        <input 
                            type="text" 
                            name="province" 
                            placeholder={formData.province ? formData.province : "Province/State"} 
                            value={formData.province}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                            required
                        />
                    </div>
                    <div className={styles.formRow}>
                        <input 
                            type="text" 
                            name="jobField" 
                            placeholder={formData.jobField ? formData.jobField : "Job Field/Industry"} 
                            value={formData.jobField}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                            required
                        />
                    </div>
                    <div className={styles.formRow}>
                        <input 
                            type="url" 
                            name="portfolioUrl" 
                            placeholder={formData.portfolioUrl ? formData.portfolioUrl : "Portfolio URL (optional)"} 
                            value={formData.portfolioUrl || ''}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                        />
                        <input 
                            type="url" 
                            name="linkedinUrl" 
                            placeholder={formData.linkedInUrl ? formData.linkedInUrl : "LinkedIn URL (optional)"} 
                            value={formData.linkedInUrl || ''}
                            onChange={handleInputChange}
                            className={styles.userInfoInput} 
                        />
                    </div>
                    <div className={styles.formRow}>
                        <textarea 
                            name="userSummary" 
                            placeholder={formData.userSummary ? formData.userSummary : "Professional Summary (optional)"} 
                            value={formData.userSummary || ''}
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
                                    value={edu.institutionName}
                                    onChange={(e) => updateEducation(index, 'institutionName', e.target.value)}
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
                    {formData.workExperience.map((work, index) => (
                        <div key={index} className={styles.dynamicEntry}>
                            <div className={styles.formRow}>
                                <input 
                                    type="text" 
                                    placeholder="Company Name" 
                                    value={work.companyName}
                                    onChange={(e) => updateWorkExperience(index, 'companyName', e.target.value)}
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
                                    value={cert.certificateName}
                                    onChange={(e) => updateCertificate(index, 'certificateName', e.target.value)}
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
                                    value={skill.skillName}
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
                                    value={project.projectName}
                                    onChange={(e) => updateProject(index, 'projectName', e.target.value)}
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
                    Update User Information
                </button>
            </form>
        </div>
    );
};

export default UserInfo;
