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
  "Toronto",
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
  onSearch: (jobs: any[]) => void;
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
  const [needUpdate, UpdateJob] = useState(true);
  const [showAdvanced, setShowAdvanced] = useState(false);
  // Add a separate state for the raw keyword input
  const [keywordInput, setKeywordInput] = useState(form.what.join(", "));

  useEffect(() => {
    if (!needUpdate) return;
    const fetchJob = async () => {
      const response = await fetch("/api/resume-api/JobBoard", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(form),
      });
      const respData = await response.json();
      onSearch(respData);
      UpdateJob(false);
    };
    fetchJob();
  }, [needUpdate, form, onSearch]);

  const handleArrayChange = (name: keyof JobReq, value: string) => {
    setForm((prev) => ({
      ...prev,
      [name]: value ? value.split(",").map((v) => v.trim()) : [],
    }));
  };

  const handleKeywordChange = (value: string) => {
    setKeywordInput(value);
  };

  // When submitting, update the form state
  const handleSearch = () => {
    setForm((prev) => ({
      ...prev,
      what: keywordInput
        .split(",")
        .map((v) => v.trim())
        .filter(Boolean),
    }));
    UpdateJob(true);
  };

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
      if (state === "default") newOr.push(tag);
      else if (state === "include") {
        newOr = newOr.filter((t) => t !== tag);
        newEx.push(tag);
      } else newEx = newEx.filter((t) => t !== tag);
      return { ...prev, what_or: newOr, what_exclude: newEx };
    });
  };

  const handlePageChange = (page: number) => {
    setForm((prev) => ({ ...prev, page: String(page) }));
    UpdateJob(true);
  };

  return (
    <div className="flex flex-col gap-4">
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
        className="flex flex-col sm:flex-row rounded-lg border shadow-sm overflow-hidden"
      >
        {/* Keywords */}
        <div className="flex items-center flex-1 px-3 py-2 border-b sm:border-b-0 sm:border-r">
          <Search className="w-5 h-5 text-gray-500 mr-2" />
          <input
            id="what"
            name="what"
            value={keywordInput}
            onChange={(e) => handleKeywordChange(e.target.value)}
            placeholder="Job title, keywords, or company"
            className="w-full p-2 outline-none text-sm sm:text-base"
            required
          />
        </div>

        {/* Country Dropdown */}
        <div className="flex items-center flex-1 px-3 py-2 border-b sm:border-b-0 sm:border-r">
          <MapPin className="w-5 h-5 mr-2 text-gray-500" />
          <select
            id="country"
            name="country"
            value={form.country}
            onChange={(e) =>
              setForm((prev) => ({ ...prev, country: e.target.value }))
            }
            className="w-full p-2 text-sm sm:text-base"
          >
            <option value="ca">Canada</option>
            <option value="gb">United Kingdom</option>
            <option value="us">United States</option>
          </select>
        </div>

        {/* Search Button */}
        <button
          type="submit"
          className="w-full sm:w-auto text-white px-4 py-2 sm:px-6 sm:py-2.5 font-semibold bg-purple-600 hover:bg-purple-700 transition"
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
          <SlidersHorizontal className="w-4 h-4" /> Tags
        </button>
      </div>

      {showAdvanced && (
        <div className="rounded-lg shadow-md p-3 sm:p-4 flex flex-wrap gap-3 sm:gap-4">
          {tags.map((tag) => {
            const state = getTagState(tag);
            return (
              <button
                key={tag}
                type="button"
                onClick={() => toggleTag(tag)}
                className={`px-3 sm:px-4 py-1 sm:py-2 rounded-md border text-sm sm:text-base font-medium transition-colors
                  ${
                    state === "default"
                      ? "bg-white border-gray-200 text-black hover:bg-gray-200"
                      : state === "include"
                      ? "bg-green-500 border-green-600 text-white hover:bg-green-600"
                      : "bg-red-500 border-red-600 text-white hover:bg-red-600"
                  }`}
              >
                {tag}
              </button>
            );
          })}
        </div>
      )}
      <hr className="mt-4 border-gray-300" />
    </div>
  );
}
