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

interface DashboardStatsData {
  totalApplications: number;
  activeResumes: number;
  lastUpdated: string;
  profileCompletion: number;
}

function DashboardContent() {
  const [stats, setStats] = useState<DashboardStatsData>({
    totalApplications: 0,
    activeResumes: 0,
    lastUpdated: "Never",
    profileCompletion: 0,
  });

  const [userName, setUserName] = useState("User");
  const [activities, setActivities] = useState<Activity[]>([]);
  const [userExists, setUserExists] = useState<boolean | null>(null); // null = loading, true = exists, false = needs registration
  const [isLoading, setIsLoading] = useState(true);
  const [showSettings, setShowSettings] = useState(false);
  const [currentUserData, setCurrentUserData] = useState<UpdateUserDto | null>(
    null
  );
  const [userEmail, setUserEmail] = useState<string>("");
  const [publicId, setPublicId] = useState<string>("");

  useEffect(() => {
    const fetchUserEmail = async () => {
      const response = await fetch("./api/cookies/get?key=userEmail");
      const data = await response.json();
      setUserEmail(data.data);
    };
    fetchUserEmail();
  }, []);

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

  const data = {
    age: 0,
    data: "",
    id: "publicId",
  };

  const handleSignOutClick = async () => {
    if (confirm("Are you sure you want to sign out?")) {
      const resp = await fetch("/api/cookies/set", {
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
    try {
      console.log("Updating user data:", userData);
      console.log("Current user publicId:", publicId);

      const updateUserData = userData as UpdateUserDto;

      setCurrentUserData(updateUserData);

      console.log("Profile updates:", updateUserData);

      const reqData = {
        path: "Users/Update",
        publicId: publicId,
        process: "PUT",
        data: updateUserData,
      };

      const resp = await fetch("./api/resume-api/User/Public", {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(reqData),
      });

      console.log("Response status (page.tsx):", resp.status);

      if (!resp.ok) {
        const errorData = await resp.json();
        throw new Error(
          errorData.error || "Failed to fetch user data from Resume API"
        );
      }

      const responseData = await resp.json();

      console.log("Update response data:", responseData);

      alert("Profile updated successfully!");

    } catch (error) {
      console.error("Error updating user data:", error);
      alert("An error occurred while updating your profile. Please try again.");
    }
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
            <div className="text-center flex-1">
              <h1 className="text-3xl font-bold text-white mb-4">
                Update Your Profile
              </h1>
              <p className="text-gray-300">
                Update your personal information and resume details
              </p>
            </div>
            <button
              onClick={() => setShowSettings(false)}
              className="bg-gray-600 hover:bg-gray-700 text-white px-4 py-2 rounded-lg transition-colors ml-4"
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
