"use client";

import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { Job } from "@/components/job-listings";

interface JobReq {
  country: string;
  page: string;
  what: string[];
  max_days_old: number;
  what_exclude: string[];
  what_or: string[];
}

function JobBoard() {
  const router = useRouter();
  const [jobs, setJobs] = useState<Job[]>([]);
  const [savedJobs, setSavedJobs] = useState<Job[]>([]);

  useEffect(() => {
    const jobReq: JobReq = {
      country: "ca",
      page: "2",
      what: ["software"],
      max_days_old: 30,
      what_exclude: ["senior"],
      what_or: ["edmonton", "toronto"],
    };

    const fetchJob = async () => {
      const response = await fetch("./api/resume-api/JobBoard", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(jobReq),
      });

      const respData = await response.json();

      setJobs(respData);
    };

    fetchJob();
  }, []);

  const handleSaveJob = (job: Job) => {
    if (!savedJobs.find((j) => j.id === job.id)) {
      setSavedJobs([...savedJobs, job]);
      alert(`Saved "${job.title}" to your saved jobs!`);
    }
  };

  const handleGenerateResume = (jobId: string) => {
    setJobs(jobs.filter((job) => job.id !== jobId));
    alert(`Resume generated for this job!`);
  };

  const handleAddJob = () => {
    alert("Add Job functionality will go here!");
  };

  return (
    <div className="flex flex-col gap-6">
      {/* Job Board Header */}
      <div className="flex flex-col items-center text-center gap-4 mb-6">
        <h1 className="text-2xl md:text-3xl font-extrabold text-[var(--foreground)]">
          Job Board
        </h1>
        <p className="text-[var(--secondary-purple)] text-sm md:text-base">
          Browse, track, and manage job listings
        </p>
        <button
          onClick={handleAddJob}
          className="px-5 py-2 rounded-xl text-sm font-semibold bg-gradient-to-r from-purple-600 to-purple-800 text-white hover:opacity-90 transition-all shadow-lg"
        >
          + Add Job
        </button>
        {/* Back to Dashboard Button */}
        <button
          onClick={() => router.push("/dashboard")}
          className="px-5 py-2 rounded-xl text-sm font-semibold bg-gradient-to-r from-purple-600 to-purple-800 text-white hover:opacity-90 transition-all shadow-lg"
        >
          ← Back to Dashboard
        </button>
      </div>

      {/* Job Listings */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {jobs.map((job) => (
          <div
            key={job.id}
            className="purple-card p-6 flex flex-col justify-between hover:scale-[1.02] transition-transform"
          >
            <div>
              <h2 className="text-xl font-bold text-[var(--foreground)] mb-1">
                {job.title}
              </h2>
              <p className="text-[var(--secondary-purple)] text-sm mb-2">
                {job.company} • {job.location}
              </p>
              <p className="text-[var(--foreground)] text-sm mb-3">
                {job.description}
              </p>
            </div>

            <div className="flex justify-between items-center mt-4">
              <span className="text-[var(--primary-purple)] font-semibold">
                {job.salaryRange}
              </span>
              {job.badge && (
                <span
                  className="px-3 py-1 rounded-full text-xs font-semibold"
                  style={{ backgroundColor: job.badge.color, color: "#fff" }}
                >
                  {job.badge.text}
                </span>
              )}
            </div>

            {/* Buttons */}
            <div className="flex gap-2 mt-4">
              <button
                onClick={() => handleSaveJob(job)}
                className="flex-1 px-3 py-2 rounded-lg text-sm font-semibold bg-[var(--input-bg)] text-[var(--foreground)] border border-[var(--border-color)] hover:bg-[var(--accent-purple)] hover:text-white transition-colors"
              >
                Save Job
              </button>
              <button
                onClick={() => handleGenerateResume(job.id)}
                className="flex-1 px-3 py-2 rounded-lg text-sm font-semibold bg-gradient-to-r from-purple-600 to-purple-800 text-white hover:opacity-90 transition-all"
              >
                Generate Resume
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default function JobPage() {
  return (
    <main className="px-4 md:px-8 py-6">
      <JobBoard />
    </main>
  );
}
