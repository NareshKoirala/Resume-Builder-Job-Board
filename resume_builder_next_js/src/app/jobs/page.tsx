"use client";

import { useRouter } from "next/navigation";
import { useState, useEffect } from "react";
import JobSearch from "@/components/seach_bar";
import Popup from "@/components/pop-up";

interface Job {
  id: string;
  title: string;
  company: string;
  location: string;
  description: string;
  contract_time: string;
  redirect_url: string;
}

function JobBoard() {
  const router = useRouter();
  const [popup, setPopup] = useState<{ status: boolean | null; message: string } | null>(null);

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

  const [jobs, setJobs] = useState<Job[]>([]);
  const [savedJobs, setSavedJobs] = useState<Job[]>(() => {
    if (typeof window !== "undefined") {
      const saved = localStorage.getItem("savedJobs");
      return saved ? JSON.parse(saved) : [];
    }
    return [];
  });

  const handleSaveJob = (job: Job) => {
    if (!savedJobs.find((j) => j.id === job.id)) {
      const updatedJobs = [...savedJobs, job];
      setSavedJobs(updatedJobs);
      localStorage.setItem("savedJobs", JSON.stringify(updatedJobs));
    } else {
      setPopup({ status: false, message: "Job already saved!" });
    }
  };

  const JobDisplay = () => {
    if (jobs.length === 0) {
      return (
        <div className="flex flex-col items-center justify-center gap-4 py-8">
          <h1 className="text-3xl sm:text-4xl font-bold text-[var(--foreground)] text-center">
            No Jobs Found
          </h1>
          <img
            src="/resource/cat_clap.gif"
            alt="No jobs found"
            className="w-36 sm:w-48 h-36 sm:h-48 rounded-xl shadow-lg"
          />
          <p className="text-[var(--secondary-purple)] text-center max-w-xs sm:max-w-sm px-4">
            Try adjusting your search criteria or check back later for new job postings!
          </p>
        </div>
      );
    }

    return (
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4 sm:gap-6">
        {jobs.map((job) => (
          <div
            key={job.id}
            className="purple-card p-4 sm:p-6 flex flex-col justify-between hover:scale-[1.02] transition-transform rounded-xl shadow-md"
          >
            <div>
              <h2 className="text-lg sm:text-xl font-bold text-[var(--foreground)] mb-1">
                {job.title}
              </h2>
              <p className="text-[var(--secondary-purple)] text-xs sm:text-sm mb-2">
                {job.company} • {job.location}
              </p>
              <p className="text-[var(--foreground)] text-sm sm:text-base mb-3 line-clamp-3">
                {job.description}
              </p>
            </div>

            <div className="flex justify-between items-center mt-4">
              <span className="text-[var(--primary-purple)] font-semibold text-sm sm:text-base">
                {job.contract_time}
              </span>
            </div>

            {/* Buttons */}
            <div className="flex flex-col sm:flex-row gap-2 mt-4">
              <button
                onClick={() => handleSaveJob(job)}
                className="flex-1 px-3 py-2 rounded-lg text-sm font-semibold bg-[var(--input-bg)] text-[var(--foreground)] border border-[var(--border-color)] hover:bg-[var(--accent-purple)] hover:text-white transition-colors"
              >
                Save Job
              </button>
            </div>
          </div>
        ))}
      </div>
    );
  };

  return (
    <div className="flex flex-col gap-6">
      <button
        onClick={() => router.push("/dashboard")}
        className="px-4 py-2 sm:px-5 sm:py-2.5 rounded-xl text-sm sm:text-base font-semibold bg-gradient-to-r from-purple-600 to-purple-800 text-white hover:opacity-50 transition-all shadow-lg"
      >
        Back to Dashboard
      </button>

      <div className="flex flex-col items-center text-center gap-2 mb-6">
        <h1 className="text-4xl sm:text-7xl font-extrabold text-[var(--foreground)]">
          Job Board
        </h1>
        <p className="text-[var(--secondary-purple)] text-xs sm:text-sm">
          Browse, track, and manage job listings
        </p>
      </div>

      <div className="flex flex-col items-center gap-3">
        <img
          src="/resource/cat_chilling.gif"
          alt="Loading..."
          className="w-20 sm:w-24 rounded-xl"
        />
      </div>

      <JobSearch onSearch={(result) => setJobs(Array.isArray(result) ? result : [])} />

      {JobDisplay()}

      {popup && (
        <Popup status={popup.status} message={popup.message} onClose={() => setPopup(null)} />
      )}
    </div>
  );
}

export default function JobPage() {
  return (
    <main className="px-4 sm:px-6 md:px-8 py-6">
      <JobBoard />
    </main>
  );
}
