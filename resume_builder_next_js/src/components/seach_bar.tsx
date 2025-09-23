"use client";

import { useState, useEffect } from "react";
import { Search, MapPin, SlidersHorizontal } from "lucide-react";
import Pagination from "./Pagination";

const tags: string[] = [
  "Software Developer",
  "Software Engineer",
  "Web Developer",
  "Web Engineer",
  "Mobile Developer",
  "Full Stack Developer",
  "Frontend Developer",
  "Backend Developer",
  "Junior",
  "Senior",
  "Mid-Level",
  "Application Developer",
  "Python",
  "JavaScript",
  "TypeScript",
  "React",
  "Node.js",
  "DevOps",
  "Edmonton",
  "Toronto"
];

interface JobReq {
  country: string;
  page: string;
  what: string[];
  max_days_old: number;
  what_exclude: string[];
  what_or: string[];
}

interface JobSearchProps {
  initialValues?: JobReq;
  onSearch: (jobs: any[]) => void; // callback to pass jobs to parent
}

export default function JobSearch({ initialValues, onSearch }: JobSearchProps) {
  const [form, setForm] = useState<JobReq>(
    initialValues || {
      country: "ca",
      page: "1",
      what: [],
      max_days_old: 30,
      what_exclude: [],
      what_or: [],
    }
  );

  const [needUpdate, UpdateJob] = useState<Boolean>(true);

  if(needUpdate)
  {
    const fetchJob = async () => {
      const response = await fetch("./api/resume-api/JobBoard", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(form),
      });

      const respData = await response.json();

      onSearch(respData);
    };

    fetchJob();
    UpdateJob(false);
  }

  const [showAdvanced, setShowAdvanced] = useState(false);

  const handleArrayChange = (name: keyof JobReq, value: string) => {
    setForm((prev) => ({
      ...prev,
      [name]: value ? value.split(",").map((v) => v.trim()) : [],
    }));
  };

  const handleSearch = async () => {
    UpdateJob(true);
  };

  // --- Tag State Helpers ---
  const getTagState = (tag: string) => {
    if (form.what_or.includes(tag)) return "include";
    if (form.what_exclude.includes(tag)) return "exclude";
    return "default";
  };

  const toggleTag = (tag: string) => {
    const state = getTagState(tag);

    setForm((prev) => {
      let newOr = [...prev.what_or];
      let newEx = [...prev.what_exclude];

      if (state === "default") {
        // add to include
        newOr.push(tag);
      } else if (state === "include") {
        // move to exclude
        newOr = newOr.filter((t) => t !== tag);
        newEx.push(tag);
      } else if (state === "exclude") {
        // remove from exclude (back to default)
        newEx = newEx.filter((t) => t !== tag);
      }

      return { ...prev, what_or: newOr, what_exclude: newEx };
    });
  };



  const handlePageChange = async (page: number) => {

    // Update form state immutably
    setForm((prev) => {
      const updated = { ...prev, page: String(page) };
      console.log("Updated form inside setter:", updated);
      UpdateJob(true);
      return updated;
    });
  };

  return (
    <div className="flex flex-col">
      <Pagination
        currentPage={Number(form.page)}
        totalPages={10}
        onPageChange={handlePageChange}
      />

      <form
        onSubmit={(e) => {
          e.preventDefault();
          handleSearch();
        }}
        className="flex rounded-lg border shadow-sm"
      >
        {/* Keywords */}
        <div className="flex items-center flex-1 px-4 border-r">
          <Search className="w-5 h-5 text-gray-500 mr-2" />
          <input
            id="what"
            name="what"
            value={form.what.join(", ")}
            onChange={(e) => handleArrayChange("what", e.target.value)}
            placeholder="Job title, keywords, or company"
            className="w-full p-3 outline-none"
            required
          />
        </div>
        {/* Country Dropdown */}
        <div className="flex items-center flex-1 px-4 border-r">
          <MapPin className="w-5 h-5" />
          <select
            id="country"
            name="country"
            value={form.country}
            onChange={(e) =>
              setForm((prev) => ({
                ...prev,
                country: e.target.value,
              }))
            }
            className="w-full p-3"
          >
            <option value="ca">Canada</option>
            <option value="gb">United Kingdom</option>
            <option value="us">United States</option>
          </select>
        </div>

        {/* Search Button */}
        <button
          type="submit"
          className="text-white px-6 font-semibold hover:bg-purple-700 transition"
        >
          Find jobs
        </button>
      </form>

      {/* Advanced Filters Toggle */}
      <div className="flex justify-end mt-2">
        <button
          type="button"
          onClick={() => setShowAdvanced(!showAdvanced)}
          className="flex items-center gap-2 text-sm text-gray-600 hover:text-gray-800"
        >
          <SlidersHorizontal className="w-4 h-4" />
          Tags
        </button>
      </div>

      {showAdvanced && (
        <div className="rounded-lg shadow-md space-y-4">
          <div className="flex flex-wrap gap-6">
            {tags
              .filter((x) => x.trim() !== "")
              .map((tag) => {
                const state = getTagState(tag);

                return (
                  <button
                    key={tag}
                    type="button"
                    onClick={() => toggleTag(tag)}
                    className={`px-4 py-3 rounded-md border text-sm font-medium transition-colors
                      ${
                        state === "default"
                          ? "bg-white border-gray-100 text-black hover:bg-gray-300"
                          : state === "include"
                          ? "bg-green-500 border-green-600 text-white hover:bg-green-600"
                          : "bg-red-500 border-red-600 text-white hover:bg-red-600"
                      }`}
                  >
                    {tag}{" "}
                  </button>
                );
              })}
          </div>
        </div>
      )}
      <br />
      <hr />
    </div>
  );
}
