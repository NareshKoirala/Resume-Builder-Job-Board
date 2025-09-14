"use client";

import { useState, useEffect, Suspense } from "react";
import { useSearchParams } from "next/navigation";
import DashboardHeader from "@/components/dashboard-header";
import DashboardStats from "@/components/dashboard-stats";
import QuickActions, { QuickAction } from "@/components/quick-actions";
import RecentActivity, { Activity } from "@/components/recent-activity";
import Stars from "@/components/stars";
import UserInfo from "@/components/user-info";
import { emailFetch } from "@/api/supabase/dbFetch";
import { UserRegisterDto, UpdateUserDto } from "@/model/data-structure";
import { usersUpdate, dbUpdate } from "@/api/supabase/dbUpdate";

interface DashboardStatsData {
  totalApplications: number;
  activeResumes: number;
  lastUpdated: string;
  profileCompletion: number;
}

function DashboardContent() {

  const searchParams = useSearchParams();
  const userEmail = searchParams.get("email") || "";

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

  useEffect(() => {
    const fetchData = async () => {
      if (userEmail) {
        try {
          setIsLoading(true);

          const fetchUserId = await emailFetch(userEmail);
        } catch (error) {
          console.error("Error fetching user data:", error);
          setUserExists(false); // On error, assume user needs registration
        } finally {
          setIsLoading(false);
        }
      } else {
        setIsLoading(false);
      }
    };

    fetchData();
  }, [userEmail]);

  const handleSettingsClick = () => {
    setShowSettings(true);
  };

  const handleSignOutClick = () => {
    if (confirm("Are you sure you want to sign out?")) {
      window.location.href = "/";
    }
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

  const handleUserRegistration = async (
    userData: UserRegisterDto | UpdateUserDto
  ) => {
    try {
      // Use the integrated registration function that handles both external API and local database

      // Update the credentials table with the new local user_id

      // After successful registration, update the state to show dashboard
      setUserExists(true);

      // Set the user name from the registration data
      if (
        "first_name" in userData &&
        typeof userData.first_name === "string" &&
        userData.first_name
      ) {
        setUserName(userData.first_name);
      } else {
        setUserName(userEmail.split("@")[0]);
      }
    } catch (error) {
      console.error("Error registering user:", error);
      const errorMessage =
        error instanceof Error ? error.message : "Unknown error occurred";
      alert(`Failed to register user: ${errorMessage}`);
    }
  };

  const handleUserUpdate = async (
    userData: UserRegisterDto | UpdateUserDto
  ) => {
    try {
      console.log("Updating user data:", userData);

      const updateUserData = userData as UpdateUserDto;

      // Validate that we have a valid user ID
      if (!updateUserData.publicId) {
        throw new Error("User ID is required for updates");
      }

      // Update the main user profile data
      const userProfileUpdates = {
        first_name: updateUserData.first_name,
        last_name: updateUserData.last_name,
        email: updateUserData.email,
        mobile: updateUserData.mobile,
        location: updateUserData.location,
        province: updateUserData.province,
        job_field: updateUserData.job_field,
        portfolio_url: updateUserData.portfolio_url || null,
        linkedin_url: updateUserData.linkedin_url || null,
        user_summary: updateUserData.user_summary || null,
      };

      console.log("Updating user profile with ID:", updateUserData.publicId);
      console.log("Profile updates:", userProfileUpdates);

      // Try to update user profile
      try {
        const userUpdateResult = await usersUpdate(
          updateUserData.publicId as any,
          userProfileUpdates
        );
        console.log("User update result:", userUpdateResult);

        if (userUpdateResult === null) {
          throw new Error(
            "usersUpdate returned null - check database connection and user ID"
          );
        }
      } catch (updateError) {
        console.error("Error in usersUpdate:", updateError);
        const errorMessage =
          updateError instanceof Error ? updateError.message : "Unknown error";
        throw new Error(`Failed to update user profile: ${errorMessage}`);
      }

      // Update related data in separate tables
      const updatePromises: Promise<any>[] = [];
      const updateLabels: string[] = [];

      if (updateUserData.education && updateUserData.education.length >= 0) {
        console.log("Updating education data:", updateUserData.education);
        updatePromises.push(
          dbUpdate(
            updateUserData.publicId as any,
            "education",
            updateUserData.education
          )
        );
        updateLabels.push("education");
      }

      if (
        updateUserData.work_experience &&
        updateUserData.work_experience.length >= 0
      ) {
        console.log(
          "Updating work experience data:",
          updateUserData.work_experience
        );
        updatePromises.push(
          dbUpdate(
            updateUserData.publicId as any,
            "work_experience",
            updateUserData.work_experience
          )
        );
        updateLabels.push("work_experience");
      }

      if (
        updateUserData.certificates &&
        updateUserData.certificates.length >= 0
      ) {
        console.log("Updating certificates data:", updateUserData.certificates);
        updatePromises.push(
          dbUpdate(
            updateUserData.publicId as any,
            "certificates",
            updateUserData.certificates
          )
        );
        updateLabels.push("certificates");
      }

      if (updateUserData.skills && updateUserData.skills.length >= 0) {
        console.log("Updating skills data:", updateUserData.skills);
        updatePromises.push(
          dbUpdate(
            updateUserData.publicId as any,
            "skills",
            updateUserData.skills
          )
        );
        updateLabels.push("skills");
      }

      if (updateUserData.projects && updateUserData.projects.length >= 0) {
        console.log("Updating projects data:", updateUserData.projects);
        updatePromises.push(
          dbUpdate(
            updateUserData.publicId as any,
            "projects",
            updateUserData.projects
          )
        );
        updateLabels.push("projects");
      }

      // Wait for all related data updates to complete
      if (updatePromises.length > 0) {
        const updateResults = await Promise.all(updatePromises);
        console.log("All update results:", updateResults);

        // Check if any updates failed and provide specific error information
        const failedUpdates: string[] = [];
        updateResults.forEach((result, index) => {
          if (result === null) {
            failedUpdates.push(updateLabels[index]);
          }
        });

        if (failedUpdates.length > 0) {
          throw new Error(
            `Failed to update: ${failedUpdates.join(
              ", "
            )}. Check console for detailed error messages.`
          );
        }

        console.log("All related data updates completed successfully");
      }

      // Update local state only after successful database updates
      setCurrentUserData(updateUserData);
      if (updateUserData.first_name) {
        setUserName(updateUserData.first_name);
      }

      setShowSettings(false);
      alert("Profile updated successfully!");
    } catch (error) {
      console.error("Error updating user:", error);
      const errorMessage =
        error instanceof Error ? error.message : "Please try again.";
      alert(`Failed to update profile: ${errorMessage}`);
    }
  };

  // Loading state
  if (isLoading) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-gray-900 via-purple-950 to-black flex items-center justify-center">
        <Stars />
        <div className="text-white text-xl">Loading...</div>
      </div>
    );
  }

  // User needs to register
  if (userExists === false) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-gray-900 via-purple-950 to-black p-6">
        <Stars />
        <div className="max-w-4xl mx-auto">
          <div className="text-center mb-8">
            <h1 className="text-3xl font-bold text-white mb-4">
              Complete Your Profile
            </h1>
            <p className="text-gray-300">
              Please provide your information to get started with Resume Builder
            </p>
          </div>
          <UserInfo mode="register" onSubmit={handleUserRegistration} />
        </div>
      </div>
    );
  }

  // Show settings/profile update
  if (showSettings && currentUserData) {
    console.log("Rendering settings with data:", currentUserData);
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
            <UserInfo
              mode="update"
              userInfo={currentUserData}
              onSubmit={handleUserUpdate}
            />
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
          <div className="text-white text-xl">Loading...</div>
        </div>
      }
    >
      <DashboardContent />
    </Suspense>
  );
}
