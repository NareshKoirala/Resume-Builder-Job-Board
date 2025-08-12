"use client";
import UserInfo from "../../components/user-info";
import { UpdateUserDto } from "../../model/data-structure";
import { useState } from "react";

// Example user data for update mode
const sampleUserData: UpdateUserDto = {
  publicId: "user123",
  firstName: "John",
  lastName: "Doe",
  email: "john.doe@example.com",
  mobile: "+1234567890",
  location: "New York",
  province: "NY",
  jobField: "Software Development",
  portfolioUrl: "https://johndoe.portfolio.com",
  linkedInUrl: "https://linkedin.com/in/johndoe",
  userSummary:
    "Experienced software developer with 5+ years in web development.",
  education: [
    {
      institutionName: "University of Technology",
      date: "2015-2019",
      location: "New York, NY",
      details: "Bachelor of Computer Science",
    },
  ],
  workExperience: [
    {
      companyName: "Tech Corp",
      date: "2019-2023",
      location: "New York, NY",
      details: "Senior Software Developer",
    },
  ],
  certificates: [
    {
      certificateName: "AWS Certified Developer",
      details: "Amazon Web Services Certification",
    },
  ],
  skills: [
    { skillName: "JavaScript" },
    { skillName: "React" },
    { skillName: "Node.js" },
  ],
  projects: [
    {
      projectName: "E-commerce Platform",
      description:
        "Built a full-stack e-commerce solution using React and Node.js",
    },
  ],
};

export default function Test() {

  const handleFormSubmit = (data: any) => {
    alert(
        `Form submitted successfully! Data: ${JSON.stringify(data, null, 2)}`
    );
  };

  return (
      <UserInfo
        mode="update" // Change to "register" for register mode
        userInfo={sampleUserData} // Change to "null" for register mode
        onSubmit={handleFormSubmit}
      />
  );
}
