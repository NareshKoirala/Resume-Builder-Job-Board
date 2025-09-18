"use client";

import { useState, useEffect, Suspense } from "react";
import DashboardHeader from "@/components/dashboard-header";
import DashboardStats from "@/components/dashboard-stats";
import QuickActions, { QuickAction } from "@/components/quick-actions";
import RecentActivity, { Activity } from "@/components/recent-activity";
import Stars from "@/components/stars";
import UserInfo from "@/components/user-info";
import { UpdateUserDto } from "@/model/data-structure";
import Loading from "@/components/loading";
import { handleUserUpdateFunc } from "./handleUserUpdate";

// Dashboard Stats Data Interface
interface DashboardStatsData {
  totalApplications: number;
  activeResumes: number;
  lastUpdated: string;
  savedJobs: number;
}

function DashboardContent() {
  const [stats, setStats] = useState<DashboardStatsData>({
    totalApplications: 0,
    activeResumes: 0,
    lastUpdated: "Never",
    savedJobs: 0,
  });

  const [userName, setUserName] = useState("User");
  const [activities, setActivities] = useState<Activity[]>([]);
  const [userExists, setUserExists] = useState<boolean | null>(null); // null = loading, true = exists, false = needs registration
  const [isLoading, setIsLoading] = useState(true);
  const [showSettings, setShowSettings] = useState(false);
  const [currentUserData, setCurrentUserData] = useState<UpdateUserDto | null>(
    null
  );
  const [publicId, setPublicId] = useState<string>("");

  useEffect(() => {
    const fetchPublicId = async () => {
      const response = await fetch("./api/cookies/get?key=publicId");
      const data = await response.json();
      setPublicId(data.data);
      setUserExists(data.data ? true : false);
      setIsLoading(false);

      const reqData = {
        path: "Users",
        publicId: data.data,
        process: "GET",
      };

      const resp = await fetch("./api/resume-api/User/Public", {
        method: "POST",
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

      const responseData = await resp.json();

      setCurrentUserData(responseData.response);

      if (responseData.response.firstName) {
        setUserName(responseData.response.firstName);
      }
    };

    fetchPublicId();
  }, []);

  const handleSettingsClick = () => {
    setShowSettings(true);
  };

  const handleSignOutClick = async () => {
    if (confirm("Are you sure you want to sign out?")) {
      await fetch("/api/cookies/set", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          age: 0,
          data: "",
          id: "publicId",
        }),
      });
    }
    window.location.href = "/";
  };

  const quickActions: QuickAction[] = [
    {
      title: "Browse Jobs",
      description: "Find jobs and build tailored resumes",
      icon: "🔍",
      href: "/jobs",
      color: "purple",
    },
    {
      title: "Your Jobs",
      description: "View and manage your job applications",
      icon: "📄",
      href: "/your-jobs",
      color: "purple",
    },
    {
      title: "Edit Profile",
      description: "Update your personal information",
      icon: "⚙️",
      onClick: handleSettingsClick,
      color: "green",
    },
  ];

  const handleUserUpdate = async (userData: UpdateUserDto) => {
    await handleUserUpdateFunc(currentUserData, userData, publicId);
    setCurrentUserData(userData);
  };

  // Loading state
  if (isLoading) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-gray-900 via-purple-950 to-black flex items-center justify-center">
        <Loading />
      </div>
    );
  }

  // Show settings/profile update
  if ((showSettings && currentUserData) || userExists === false) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-gray-900 via-purple-950 to-black p-6">
        <Stars />
        <div className="max-w-4xl mx-auto">
          <div className="flex items-center justify-between mb-8">
            {/* Header */}
            <h1 className="text-2xl md:text-3xl font-extrabold text-[var(--foreground)] mb-6 text-center">
              Update User Information
            </h1>
            <button
              onClick={() => setShowSettings(false)}
              className="self-start mb-4 px-4 py-2 rounded-xl text-sm font-semibold bg-gradient-to-r from-purple-600 to-purple-800 text-white hover:opacity-90 shadow-lg"
            >
              ← Back to Dashboard
            </button>
          </div>
          {currentUserData && (
            <UserInfo userInfo={currentUserData} onSubmit={handleUserUpdate} />
          )}
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-900 via-purple-950 to-black p-6">
      <div className="max-w-7xl mx-auto">
        <DashboardHeader
          userName={userName}
          onSignOutClick={handleSignOutClick}
        />

        <DashboardStats
          totalApplications={stats.totalApplications}
          activeResumes={stats.activeResumes}
          savedJobs={stats.savedJobs}
        />

        <QuickActions actions={quickActions} />

        <RecentActivity activities={activities} />

        {/* Footer */}
        <footer className="mt-8 text-center text-gray-400">
          <p>
            &copy; 2025 Resume Builder. Build your future, one resume at a time.
          </p>
        </footer>
      </div>
    </div>
  );
}

export default function Dashboard() {
  return (
    <Suspense
      fallback={
        <div className="min-h-screen bg-gradient-to-br from-gray-900 via-purple-950 to-black flex items-center justify-center">
          <Loading />
        </div>
      }
    >
      <DashboardContent />
    </Suspense>
  );
}
