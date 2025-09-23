"use client";

import { useRouter } from "next/navigation";
import { useState } from "react";
import { Job } from "@/components/job-listings";
import JobSearch from "@/components/seach_bar";

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
  const [jobReq, setJobReq] = useState<JobReq>();

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

  return (
    <div className="flex flex-col gap-6">
      {/* Back to Dashboard Button */}
      <button
        onClick={() => router.push("/dashboard")}
        className="px-5 py-2 rounded-xl text-sm font-semibold bg-gradient-to-r from-purple-600 to-purple-800 text-white hover:opacity-50 transition-all shadow-lg"
      >
        Back to Dashboard
      </button>
      {/* Job Board Header */}

      <div className="flex flex-col items-center text-center gap-2 mb-6">
        <h1 className="text-7xl font-extrabold text-[var(--foreground)]">
          Job Board
        </h1>
        <p className="text-[var(--secondary-purple)] text-sm md:text-base">
          Browse, track, and manage job listings
        </p>
      </div>
      {/* Inner wrapper for column layout */}
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          gap: "12px", // spacing between gif and text
        }}
      >
        <img
          src="/resource/cat_chilling.gif"
          alt="Loading..."
          style={{ borderRadius: "12px" }}
          width={100}
        />
      </div>
      
      <JobSearch
        initialValues={jobReq}
        onSearch={(result) => setJobs(Array.isArray(result) ? result : [])}
      />

      {/* Job Listings */}
      <div className="grid grid-cols-1 md:grid-cols-2 md:grid-cols-3 gap-6">
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
