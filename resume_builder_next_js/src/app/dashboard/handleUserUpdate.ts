import { UpdateUserDto } from "../../model/data-structure";

export async function handleUserUpdateFunc(
  previousUserData: UpdateUserDto | null,
  userData: UpdateUserDto,
  publicId: string
) {
  try {
    const updateUserData = userData as UpdateUserDto;

    alert("Profile updates: " + JSON.stringify(updateUserData.firstName));

    const userInfo = {
      firstName: updateUserData.firstName,
      lastName: updateUserData.lastName,
      email: updateUserData.email,
      mobile: updateUserData.mobile,
      location: updateUserData.location,
      province: updateUserData.province,
      jobField: updateUserData.jobField,
      portfolioUrl: updateUserData.portfolioUrl,
      linkedInUrl: updateUserData.linkedInUrl,
      userSummary: updateUserData.userSummary,
    };

    const education = updateUserData.education.map((edu) => ({
      institutionName: edu.institutionName,
      date: edu.date,
      location: edu.location,
      details: edu.details,
    }));

    const workExperience = updateUserData.workExperience.map((work) => ({
      companyName: work.companyName,
      date: work.date,
      location: work.location,
      details: work.details,
    }));

    const certificates = updateUserData.certificates.map((cert) => ({
      certificateName: cert.certificateName,
      details: cert.details,
    }));

    const skills = updateUserData.skills.map((skill) => ({
      skillName: skill.skillName,
    }));

    const projects = updateUserData.projects.map((proj) => ({
      projectName: proj.projectName,
      description: proj.description,
    }));

    var reqData = {
      path: "Users/Update",
      publicId: publicId,
      process: "PUT",
      data: userInfo,
    };

    var resp = await fetch("./api/resume-api/User/Public", {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(reqData),
    });

    if (!resp.ok) {
      const errorData = await resp.json();
      throw new Error(
        errorData.error || "Failed to fetch user data from Resume API"
      );
    }

    if (ValidateData(education, previousUserData?.education || [])) {
      education.forEach(async (edu) => {
        await HelperAPIFunc(edu, publicId, "Users/Add/Education", "POST");
      });
    }
    if (ValidateData(workExperience, previousUserData?.workExperience || [])) {
      workExperience.forEach(async (work) => {
        await HelperAPIFunc(
          work,
          publicId,
          "Users/Add/Work",
          "POST"
        );
      });
    }
    if (ValidateData(certificates, previousUserData?.certificates || [])) {
      certificates.forEach(async (cert) => {
        await HelperAPIFunc(cert, publicId, "Users/Add/Certificate", "POST");
      });
    }
    if (ValidateData(skills, previousUserData?.skills || [])) {
      skills.forEach(async (skill) => {
        await HelperAPIFunc(skill, publicId, "Users/Add/Skill", "POST");
      });
    }
    if (ValidateData(projects, previousUserData?.projects || [])) {
      projects.forEach(async (proj) => {
        await HelperAPIFunc(proj, publicId, "Users/Add/Project", "POST");
      });
    }

    alert("Profile updated successfully!");
  } catch (error) {
    console.error("Error updating user data:", error);
    alert("An error occurred while updating your profile. Please try again.");
  }
}

function ValidateData(data: Array<any>, previousData: Array<any>) {
  return (
    data.length !== 0 &&
    data !== undefined &&
    JSON.stringify(data) !== JSON.stringify(previousData)
  );
}

async function HelperAPIFunc(
  data: any,
  publicId: string,
  path: string,
  method: string
) {
  const apiUrl = "./api/resume-api/User/Public";

  const reqData = {
    path: path,
    publicId: publicId,
    process: method,
    data: data,
  };
  return fetch(apiUrl, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(reqData),
  });
}
