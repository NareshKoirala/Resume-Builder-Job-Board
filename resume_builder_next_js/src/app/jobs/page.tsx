"use client";

import JobListings, { Job } from "@/components/job-listings";
import { useEffect, useState } from "react";

function JobBoard() {
  const [jobs, setJobs] = useState<Job[]>([]);

  useEffect(() => {
    // Mock data (replace later with API)
    const mockJobs: Job[] = [
      {
        id: "1",
        title: "Full Stack Developer",
        company: "TechNova Solutions",
        location: "Remote",
        description:
          "Work with React, Node.js, and MongoDB to build scalable web applications.",
        salaryRange: "$80,000 - $100,000",
        badge: { text: "Hot", color: "red" },
      },
      {
        id: "2",
        title: "Backend Engineer",
        company: "CloudEdge Inc.",
        location: "Toronto, Canada",
        description:
          "Develop RESTful APIs with C# and .NET Core. Optimize database queries.",
        salaryRange: "$90,000 - $110,000",
        badge: { text: "New", color: "green" },
      },
      {
        id: "3",
        title: "Frontend Developer",
        company: "PixelWorks",
        location: "New York, USA",
        description:
          "Build modern UIs with React and TypeScript. Experience with Next.js is a plus.",
        salaryRange: "$75,000 - $95,000",
      },
      {
        id: "4",
        title: "Data Analyst",
        company: "Insight Analytics",
        location: "Remote",
        description:
          "Analyze business data using SQL, Python, and Power BI. Provide reports and dashboards for stakeholders.",
        salaryRange: "$60,000 - $85,000",
      },
      {
        id: "5",
        title: "Software Engineering Intern",
        company: "NextGen Labs",
        location: "San Francisco, USA",
        description:
          "Assist in developing and testing software features. Great opportunity to learn cloud computing and CI/CD pipelines.",
        salaryRange: "$20/hr - $25/hr",
        badge: { text: "Internship", color: "blue" },
      },
    ];

    setJobs(mockJobs);
  }, []);

  return (
    <div>
      <JobListings jobs={jobs} />
    </div>
  );
}

export default function JobPage() {
  return (
    <main>
      <JobBoard />
    </main>
  );
}
