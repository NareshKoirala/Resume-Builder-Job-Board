'use client';

import { useState, useEffect, Suspense } from "react";
import DashboardHeader from "@/components/dashboard-header";
import DashboardStats from "@/components/dashboard-stats";
import QuickActions, { QuickAction } from "@/components/quick-actions";
import UserInfo from "@/components/user-info";
import { UpdateUserDto } from "@/model/data-structure";
import Loading from "@/components/loading";
import { handleUserUpdateFunc } from "./handleUserUpdate";
import Popup from "@/components/pop-up";

interface DashboardStatsData {
  totalApplications: number;
  activeResumes: number;
  savedJobs: number;
}

function DashboardContent() {
  const [popup, setPopup] = useState<{ status: boolean | null; message: string } | null>(null);
  const [stats, setStats] = useState<DashboardStatsData>({ totalApplications: 0, activeResumes: 0, savedJobs: 0 });
  const [userName, setUserName] = useState("User");
  const [userExists, setUserExists] = useState<boolean | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [showSettings, setShowSettings] = useState(false);
  const [currentUserData, setCurrentUserData] = useState<UpdateUserDto | null>(null);
  const [publicId, setPublicId] = useState<string>("");

  useEffect(() => {
    const savedJobs = JSON.parse(localStorage.getItem("savedJobs") || "[]");
    const activeResume = JSON.parse(localStorage.getItem("activeResume") || "[]");
    const applications = JSON.parse(localStorage.getItem("application") || "[]");

    setStats({
      totalApplications: Array.isArray(applications) ? applications.length : 0,
      activeResumes: Array.isArray(activeResume) ? activeResume.length : 0,
      savedJobs: Array.isArray(savedJobs) ? savedJobs.length : 0,
    });
  }, []);

  useEffect(() => {
    const fetchPublicId = async () => {
      const response = await fetch("./api/cookies/get?key=publicId");
      const data = await response.json();
      setPublicId(data.data);
      setUserExists(Boolean(data.data));

      if (!data.data) {
        setIsLoading(false);
        window.location.href = "/";
        localStorage.clear();
        setPopup({ status: false, message: "Please log in to access the dashboard." });
        return;
      }

      const savedUser = localStorage.getItem("savedUser");
      if (savedUser) {
        const userJSON = JSON.parse(savedUser);
        setCurrentUserData(userJSON);
        if (userJSON.firstName) setUserName(userJSON.firstName);
        setIsLoading(false);
        return;
      }

      const reqData = { path: "Users", publicId: data.data, process: "GET" };
      const resp = await fetch("./api/resume-api/User/Public", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(reqData),
      });

      if (!resp.ok) {
        const errorData = await resp.json();
        window.location.href = "/";
        setPopup({ status: false, message: errorData.error || "Failed to fetch user data" });
        return;
      }

      const responseData = await resp.json();
      localStorage.setItem("savedUser", JSON.stringify(responseData.response));
      setCurrentUserData(responseData.response);
      if (responseData.response.firstName) setUserName(responseData.response.firstName);
      setIsLoading(false);
    };

    fetchPublicId();
  }, []);

  const handleSettingsClick = () => setShowSettings(true);

  const handleSignOutClick = async () => {
    if (confirm("Are you sure you want to sign out?")) {
      await fetch("/api/cookies/set", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ age: 0, data: "", id: "publicId" }),
      });
      localStorage.clear();
      window.location.href = "/";
    }
  };

  const quickActions: QuickAction[] = [
    { title: "Browse Jobs", description: "Find jobs and build tailored resumes", icon: "🔍", href: "/jobs", color: "purple" },
    { title: "Your Jobs", description: "View and manage your job applications", icon: "📄", href: "/your-jobs", color: "purple" },
    { title: "Edit Profile", description: "Update your personal information", icon: "⚙️", onClick: handleSettingsClick, color: "green" },
  ];

  const handleUserUpdate = async (userData: UpdateUserDto) => {
    await handleUserUpdateFunc(currentUserData, userData, publicId);
    setCurrentUserData(userData);
  };

  if (isLoading) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-gray-900 via-purple-950 to-black flex items-center justify-center">
        <Loading message="Loading User Information" />
      </div>
    );
  }

  // Settings/Profile update
  if ((showSettings && currentUserData) || userExists === false) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-gray-900 via-purple-950 to-black p-6">
        <div className="max-w-4xl mx-auto">
          <div className="flex flex-col md:flex-row items-center justify-between mb-8 gap-4">
            <h1 className="text-2xl md:text-3xl font-extrabold text-white text-center md:text-left">Update User Information</h1>
            <button
              onClick={() => setShowSettings(false)}
              className="px-4 py-2 rounded-xl text-sm font-semibold bg-gradient-to-r from-purple-600 to-purple-800 text-white hover:opacity-90 shadow-lg transition"
            >
              ← Back to Dashboard
            </button>
          </div>
          {currentUserData && <UserInfo userInfo={currentUserData} onSubmit={handleUserUpdate} />}
        </div>
      </div>
    );
  }

  // Main dashboard
  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-900 via-purple-950 to-black p-6">
      <div className="max-w-7xl mx-auto flex flex-col gap-8">
        <DashboardHeader userName={userName} onSignOutClick={handleSignOutClick} />
        <DashboardStats
          totalApplications={stats.totalApplications}
          activeResumes={stats.activeResumes}
          savedJobs={stats.savedJobs}
        />
        <QuickActions actions={quickActions} />

        {popup && <Popup status={popup.status} message={popup.message} onClose={() => setPopup(null)} />}

        <footer className="mt-12 text-center text-gray-400 text-sm md:text-base">
          &copy; 2025 Resume Builder. Build your future, one resume at a time.
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
