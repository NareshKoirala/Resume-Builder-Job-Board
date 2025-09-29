"use client";

import { useState, useEffect } from "react";
import Tabs from "@mui/material/Tabs";
import Tab from "@mui/material/Tab";
import { styled } from "@mui/material/styles";
import { useRouter } from "next/navigation";
import Popup from "@/components/pop-up";
import { Button } from "@mui/material"; // kept only button+textfield since you use them outside dialog
import { generateJSON } from "./generateResume";

interface Job {
  id: string;
  title: string;
  company: string;
  location: string;
  description: string;
  contract_time: string;
  redirect_url: string;
}

// Styled Tabs
const PurpleTabs = styled(Tabs)({
  "& .MuiTabs-indicator": {
    backgroundColor: "#b78fff",
    height: 3,
    borderRadius: 2,
  },
});

const PurpleTab = styled(Tab)({
  textTransform: "none",
  fontWeight: 600,
  color: "#a27bc8",
  "&.Mui-selected": {
    color: "#fff",
    backgroundColor: "#3a1f5c",
    borderRadius: 12,
  },
});

export default function Your_Jobs() {
  const router = useRouter();

  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch("./api/cookies/get?key=publicId");
      const data = await response.json();

      if (!data.data) {
        window.location.href = "/";
        localStorage.clear();
        return;
      }
    };
    fetchData();
  }, []);

  const [popup, setPopup] = useState<{ status: boolean; message: string } | null>(null);
  const [currentTab, setCurrentTab] = useState<true | false | null>(true);
  const [savedJobs, setSavedJobs] = useState<Job[]>([]);
  const [activeResume, setActiveResume] = useState<Job[]>([]);
  const [applications, setApplications] = useState<Job[]>([]);

  const [openForm, setOpenForm] = useState(false);
  const [formData, setFormData] = useState<Omit<Job, "id">>({
    title: "",
    company: "",
    location: "",
    description: "",
    contract_time: "",
    redirect_url: "",
  });

  useEffect(() => {
    try {
      setSavedJobs(JSON.parse(localStorage.getItem("savedJobs") || "[]"));
      setActiveResume(JSON.parse(localStorage.getItem("activeResume") || "[]"));
      setApplications(JSON.parse(localStorage.getItem("application") || "[]"));
    } catch {
      // ignore corrupted localStorage
    }
  }, []);

  const handleChange = (_: React.SyntheticEvent, newValue: true | false | null) =>
    setCurrentTab(newValue);

  const handleGenerateResume = (job: Job) => {
    if (activeResume.length >= 10) {
      setPopup({ status: false, message: "You have MAX Resume Saved!!" });
      return;
    }
    if (activeResume.some((j) => j.id === job.id)) {
      setPopup({ status: false, message: "Job Resume already generated!" });
      return;
    }
    const updatedJobs = [...activeResume, job];
    setActiveResume(updatedJobs);
    localStorage.setItem("activeResume", JSON.stringify(updatedJobs));
    generateJSON(job);
    setPopup({ status: true, message: "Resume Generated Successfully!" });
  };

  const Applied = (job: Job) => {
    const updatedSaved = savedJobs.filter((j) => j.id !== job.id);
    const updatedActive = activeResume.filter((j) => j.id !== job.id);
    const updatedApplications = [...applications, job];
    setSavedJobs(updatedSaved);
    setActiveResume(updatedActive);
    setApplications(updatedApplications);
    localStorage.setItem("savedJobs", JSON.stringify(updatedSaved));
    localStorage.setItem("activeResume", JSON.stringify(updatedActive));
    localStorage.setItem("application", JSON.stringify(updatedApplications));
  };

  const Remove = (job: Job) => {
    switch (currentTab) {
      case true:
        const updatedSaved = savedJobs.filter((j) => j.id !== job.id);
        setSavedJobs(updatedSaved);
        localStorage.setItem("savedJobs", JSON.stringify(updatedSaved));
        break;
      case false:
        const updatedResume = activeResume.filter((j) => j.id !== job.id);
        setActiveResume(updatedResume);
        localStorage.setItem("activeResume", JSON.stringify(updatedResume));
        let existingResumes = JSON.parse(localStorage.getItem("savedResumes") || "[]");
        existingResumes = existingResumes.filter((r: any) => r.id !== job.id);
        localStorage.setItem("savedResumes", JSON.stringify(existingResumes));
        break;
      case null:
        const updatedApplied = applications.filter((j) => j.id !== job.id);
        setApplications(updatedApplied);
        localStorage.setItem("application", JSON.stringify(updatedApplied));
        break;
    }
  };

  const handleFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleFormSubmit = () => {
    const allFilled = Object.values(formData).every((v) => v.trim() !== "");
    if (!allFilled) {
      setPopup({ status: false, message: "All fields are required" });
      return;
    }

    const newJob: Job = {
      id: String(savedJobs.length + 1),
      ...formData,
    };

    const updated = [...savedJobs, newJob];
    setSavedJobs(updated);
    localStorage.setItem("savedJobs", JSON.stringify(updated));

    setFormData({
      title: "",
      company: "",
      location: "",
      description: "",
      contract_time: "",
      redirect_url: "",
    });
    setOpenForm(false);
  };

  const handleDownload = async (job: Job) => {
    let existingResumes = JSON.parse(localStorage.getItem("savedResumes") || "[]");

    if (!existingResumes.some((r: any) => r.id === job.id)) {
      setPopup({
        status: false,
        message: "No Resume for this job to Download!",
      });
      return;
    }

    const data = {
      type: "Resume",
      data: existingResumes.find((r: any) => r.id === job.id).resume,
    };

    const res = await fetch(`/api/resume-api/Job/docx`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });

    if (!res.ok) return;

    const blob = await res.blob();
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = `${job.title}.docx`;
    document.body.appendChild(a);
    a.click();
    a.remove();
    window.URL.revokeObjectURL(url);
  };

  const renderJobs = (jobs: Job[]) =>
    jobs.length === 0 ? (
      <div className="flex flex-col items-center justify-center gap-4 py-16">
        <img src="/resource/cat_clap.gif" alt="No jobs" />
        <h1 className="text-3xl font-bold text-white">No Jobs Found</h1>
      </div>
    ) : (
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        {jobs.map((job) => (
          <div
            key={job.id}
            className="relative bg-gradient-to-tl from-slate-900 to-violet-900 p-6 rounded-2xl shadow-md hover:shadow-xl transition-all flex flex-col justify-between"
          >
            <button
              onClick={() => Remove(job)}
              className="absolute top-2 right-2 bg-violet-900 hover:bg-purple-800 text-white rounded-full w-10 h-10 flex items-center justify-center shadow-md transition"
            >
              ❌
            </button>

            <div>
              <h2 className="text-xl font-bold text-white mb-2">{job.title}</h2>
              <p className="text-purple-300 text-sm mb-3">
                {job.company} • {job.location} • {job.contract_time}
              </p>
              <p className="text-purple-200 text-sm line-clamp-4">
                {job.description}
              </p>
            </div>

            <div className="flex justify-between items-center mt-4">
              {currentTab != null && (
                <button
                  onClick={() => {
                    Applied(job);
                    window.open(job.redirect_url, "_blank");
                  }}
                  className="m-2 text-center flex-1 px-3 py-2 rounded-lg text-sm font-semibold bg-gradient-to-r from-purple-600 to-purple-800 text-white hover:opacity-90 transition-all"
                >
                  Apply
                </button>
              )}

              {currentTab === false && (
                <button
                  className="flex-1 px-3 py-2 rounded-lg text-sm font-semibold bg-gradient-to-r from-purple-600 to-purple-800 text-white hover:opacity-90 transition-all"
                  onClick={() => handleDownload(job)}
                >
                  Download .docx
                </button>
              )}

              {currentTab === true && (
                <button
                  onClick={() => handleGenerateResume(job)}
                  className="flex-1 px-3 py-2 rounded-lg text-sm font-semibold bg-gradient-to-r from-purple-600 to-purple-800 text-white hover:opacity-90 transition-all"
                >
                  Generate Resume
                </button>
              )}
            </div>
          </div>
        ))}
      </div>
    );

  return (
    <div className="min-h-screen flex flex-col items-center py-12 px-6">
      <button
        onClick={() => router.push("/dashboard")}
        className="mb-8 px-6 py-2 rounded-xl text-sm font-semibold bg-gradient-to-r from-[#2e1b4b] to-[#3a1f5c] text-white hover:opacity-80 shadow-lg transition-all"
      >
        Back to Dashboard
      </button>

      <h1 className="text-4xl font-bold mb-6 text-white">Your Jobs</h1>

      <PurpleTabs value={currentTab} onChange={handleChange} centered>
        <PurpleTab label="Saved Jobs" value={true} />
        <PurpleTab label="Job Resumes" value={false} />
        <PurpleTab label="Applied" value={null} />
      </PurpleTabs>

      {currentTab === true && (
        <div className="my-6">
          <Button
            variant="contained"
            sx={{
              background: "linear-gradient(to right, #6b21a8, #4c1d95)",
              borderRadius: "12px",
              fontWeight: "600",
              "&:hover": { opacity: 0.9 },
            }}
            onClick={() => setOpenForm(true)}
          >
            Add Manual
          </Button>
        </div>
      )}

      <div className="mt-6 w-full max-w-6xl">
        {currentTab === true && renderJobs(savedJobs)}
        {currentTab === false && renderJobs(activeResume)}
        {currentTab === null && renderJobs(applications)}
      </div>

      {/* Custom Dialog (replacing MUI Dialog) */}
      {openForm && (
        <div className="fixed inset-0 flex items-center justify-center z-50 bg-black/60">
          <div className="bg-gradient-to-tr from-slate-900 via-purple-900 to-slate-950 text-white rounded-2xl p-6 w-full max-w-md shadow-2xl">
            <h2 className="text-xl font-semibold mb-4">Add Job Manually</h2>

            <div className="flex flex-col gap-4">
              {Object.keys(formData).map((key) => (
                <input
                  key={key}
                  name={key}
                  placeholder={key.replace("_", " ").toUpperCase()}
                  value={(formData as any)[key]}
                  onChange={handleFormChange}
                  className="w-full rounded-lg border border-purple-700 bg-transparent px-3 py-2 focus:outline-none focus:ring-2 focus:ring-purple-500"
                />
              ))}
            </div>

            <div className="flex justify-end gap-3 mt-6">
              <button
                onClick={() => setOpenForm(false)}
                className="px-4 py-2 rounded-lg border border-purple-600 text-purple-300 hover:bg-purple-800/40"
              >
                Cancel
              </button>
              <button
                onClick={handleFormSubmit}
                className="px-4 py-2 rounded-lg bg-gradient-to-r from-purple-600 to-purple-800 text-white font-semibold hover:opacity-90"
              >
                Save
              </button>
            </div>
          </div>
        </div>
      )}

      {popup && (
        <Popup status={popup.status} message={popup.message} onClose={() => setPopup(null)} />
      )}
    </div>
  );
}
