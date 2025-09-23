"use client";

import React, { useState, useEffect } from "react";
import { UpdateUserDto } from "@/model/data-structure";

interface UserInfoProps {
  userInfo?: UpdateUserDto | null;
  onSubmit?: (data: UpdateUserDto) => void;
}

const UserInfo: React.FC<UserInfoProps> = ({ userInfo = null, onSubmit }) => {
  const [formData, setFormData] = useState<UpdateUserDto>({
    firstName: "",
    lastName: "",
    email: "",
    mobile: "",
    location: "",
    province: "",
    jobField: "",
    portfolioUrl: "",
    linkedInUrl: "",
    userSummary: "",
    education: [],
    workExperience: [],
    certificates: [],
    skills: [],
    projects: [],
  });

  useEffect(() => {
    if (userInfo) setFormData({ ...userInfo });
  }, [userInfo]);

  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (onSubmit) onSubmit(formData);
    // window.location.reload();
  };

  const addEntry = <T extends object>(field: keyof UpdateUserDto, entry: T) =>
    setFormData((prev) => ({
      ...prev,
      [field]: [...(prev[field] as T[]), entry],
    }));

  const updateEntry = <T extends object>(
    field: keyof UpdateUserDto,
    index: number,
    key: keyof T,
    value: string
  ) =>
    setFormData((prev) => ({
      ...prev,
      [field]: (prev[field] as T[]).map((item, i) =>
        i === index ? { ...item, [key]: value } : item
      ),
    }));

  const dictsData = {
  education: "Education",
  workExperience: "Work",
  certificates: "Certificate",
  skills: "Skill",
  projects: "Project",
};

const removeEntry = async <T extends object>(
  field: keyof UpdateUserDto,
  index: number
) => {
  const data = (formData[field] as T[])[index];
  console.log("Data you want to remove ->", data);

  const strField = dictsData[field as keyof typeof dictsData];
  console.log("Mapped field ->", strField);

    // ✅ Ensure the entry has an id before calling backend
  if (!data?.id) {
    // Still remove locally if you want:
    setFormData((prev) => ({
      ...prev,
      [field]: (prev[field] as T[]).filter((_, i) => i !== index),
    }));
    return;
  }

  try {
    // Call your Next.js API route (the one you made earlier)
    const res = await fetch("./api/resume-api/User/Delete", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        field: strField, // "Education", "Work", etc.
        data,            // the actual entry being removed
      }),
    });

    const result = await res.json();
    if (!res.ok) {
      console.error("Failed to delete:", result);
      return;
    }

    console.log("Successfully deleted:", result);

    // ✅ Update frontend state only if backend delete succeeds
    setFormData((prev) => ({
      ...prev,
      [field]: (prev[field] as T[]).filter((_, i) => i !== index),
    }));
  } catch (err) {
    console.error("Error deleting entry:", err);
  }
};


  return (
    <div className="min-h-screen bg-[var(--background)] flex flex-col items-center py-6 px-4 md:px-0">
      {/* Form with frosted glass effect */}
      <form
        onSubmit={handleSubmit}
        className="w-full max-w-5xl bg-[rgba(255,255,255,0.05)] backdrop-blur-md border border-[var(--border-color)] rounded-2xl shadow-lg p-6 md:p-10 flex flex-col gap-6"
      >
        {/* Personal Info */}
        <section className="flex flex-col gap-4">
          <h3 className="text-[var(--primary-purple)] font-semibold text-xl md:text-2xl">
            Personal Info
          </h3>
          <div className="flex flex-col md:flex-row gap-4">
            <input
              type="text"
              name="firstName"
              placeholder="First Name"
              value={formData.firstName}
              onChange={handleInputChange}
              className="purple-input flex-1 px-4 py-2"
              disabled={!!formData.firstName}
              required
            />
            <input
              type="text"
              name="lastName"
              placeholder="Last Name"
              value={formData.lastName}
              onChange={handleInputChange}
              className="purple-input flex-1 px-4 py-2"
              disabled={!!formData.lastName}
              required
            />
          </div>

          <div className="flex flex-col md:flex-row gap-4">
            <input
              type="email"
              name="email"
              placeholder="Email"
              value={formData.email}
              className="purple-input flex-1 px-4 py-2"
              disabled
              required
            />
            <input
              type="tel"
              name="mobile"
              placeholder="Mobile Number"
              value={formData.mobile}
              onChange={handleInputChange}
              className="purple-input flex-1 px-4 py-2"
              required
            />
          </div>

          <div className="flex flex-col md:flex-row gap-4">
            <input
              type="text"
              name="location"
              placeholder="Location/City"
              value={formData.location}
              onChange={handleInputChange}
              className="purple-input flex-1 px-4 py-2"
              required
            />
            <input
              type="text"
              name="province"
              placeholder="Province/State"
              value={formData.province}
              onChange={handleInputChange}
              className="purple-input flex-1 px-4 py-2"
              required
            />
          </div>

          <input
            type="text"
            name="jobField"
            placeholder="Job Field / Industry"
            value={formData.jobField}
            onChange={handleInputChange}
            className="purple-input px-4 py-2"
            required
          />

          <div className="flex flex-col md:flex-row gap-4">
            <input
              type="url"
              name="portfolioUrl"
              placeholder="Portfolio URL (optional)"
              value={formData.portfolioUrl || ""}
              onChange={handleInputChange}
              className="purple-input flex-1 px-4 py-2"
            />
            <input
              type="url"
              name="linkedInUrl"
              placeholder="LinkedIn URL (optional)"
              value={formData.linkedInUrl || ""}
              onChange={handleInputChange}
              className="purple-input flex-1 px-4 py-2"
            />
          </div>

          <textarea
            name="userSummary"
            placeholder="Professional Summary (optional)"
            value={formData.userSummary || ""}
            onChange={handleInputChange}
            className="purple-input resize-none h-24 px-4 py-2"
          />
        </section>

        {/* Dynamic sections */}
        {[
          {
            field: "education",
            title: "Education",
            template: {
              institutionName: "",
              date: "",
              location: "",
              details: "",
            },
            keys: ["institutionName", "date", "location", "details"],
          },
          {
            field: "workExperience",
            title: "Work Experience",
            template: { companyName: "", date: "", location: "", details: "" },
            keys: ["companyName", "date", "location", "details"],
          },
          {
            field: "certificates",
            title: "Certificates",
            template: { certificateName: "", details: "" },
            keys: ["certificateName", "details"],
          },
          {
            field: "skills",
            title: "Skills",
            template: { skillName: "" },
            keys: ["skillName"],
          },
          {
            field: "projects",
            title: "Projects",
            template: { projectName: "", description: "" },
            keys: ["projectName", "description"],
          },
        ].map((section) => (
          <section
            key={section.field as string}
            className="flex flex-col gap-4"
          >
            <div className="flex justify-between items-center">
              <h3 className="text-[var(--primary-purple)] font-semibold text-xl md:text-2xl">
                {section.title}
              </h3>
              <button
                type="button"
                onClick={() =>
                  addEntry(
                    section.field as keyof UpdateUserDto,
                    section.template
                  )
                }
                className="px-3 py-1 rounded-lg bg-[var(--accent-purple)] text-white text-sm"
              >
                + Add {section.title}
              </button>
            </div>
            {(formData[section.field as keyof UpdateUserDto] as any[]).map(
              (entry, index) => (
                <div
                  key={index}
                  className="flex flex-col gap-2 p-2 border border-[var(--border-color)] rounded-lg"
                >
                  {/* Non-description fields in row */}
                  <div className="flex flex-col md:flex-row gap-4">
                    {section.keys
                      .filter(
                        (key) => key !== "details" && key !== "description"
                      )
                      .map((key) => (
                        <input
                          key={key}
                          type="text"
                          placeholder={key}
                          value={entry[key] || ""}
                          onChange={(e) =>
                            updateEntry(
                              section.field as keyof UpdateUserDto,
                              index,
                              key as any,
                              e.target.value
                            )
                          }
                          className="purple-input flex-1 px-4 py-2"
                        />
                      ))}
                  </div>

                  {/* Details / Description textarea full width */}
                  {["details", "description"].map((key) =>
                    entry[key] !== undefined ? (
                      <textarea
                        key={key}
                        placeholder={key}
                        value={entry[key] || ""}
                        onChange={(e) =>
                          updateEntry(
                            section.field as keyof UpdateUserDto,
                            index,
                            key as any,
                            e.target.value
                          )
                        }
                        className="purple-input w-full px-4 py-2 resize-none min-h-[80px]"
                      />
                    ) : null
                  )}

                  <button
                    type="button"
                    onClick={() =>
                      removeEntry(section.field as keyof UpdateUserDto, index)
                    }
                    className="px-3 py-1 rounded-lg bg-red-600 text-white text-sm self-start"
                  >
                    Remove
                  </button>
                </div>
              )
            )}
          </section>
        ))}

        <button
          type="submit"
          className="purple-button-primary mt-4 w-full py-3"
        >
          Update User Information
        </button>
      </form>
    </div>
  );
};

export default UserInfo;
