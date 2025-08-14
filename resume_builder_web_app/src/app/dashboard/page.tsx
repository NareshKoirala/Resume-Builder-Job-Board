'use client';

import { useState, useEffect } from 'react';
import DashboardHeader from '@/components/dashboard-header';
import DashboardStats from '@/components/dashboard-stats';
import QuickActions, { QuickAction } from '@/components/quick-actions';
import JobListings, { Job } from '@/components/job-listings';
import RecentActivity, { Activity } from '@/components/recent-activity';
import Stars from '@/components/stars';
import {userFetch, emailFetch} from '@/api/supabase/dbFetch';

interface DashboardStats {
  totalApplications: number;
  activeResumes: number;
  lastUpdated: string;
  profileCompletion: number;
}

export default function Dashboard() {
  const [stats, setStats] = useState<DashboardStats>({
    totalApplications: 0,
    activeResumes: 0,
    lastUpdated: 'Never',
    profileCompletion: 0
  });
  
  const [userName, setUserName] = useState('User');
  const [jobs, setJobs] = useState<Job[]>([]);
  const [activities, setActivities] = useState<Activity[]>([]);

  const quickActions: QuickAction[] = [
    {
      title: 'Browse Jobs',
      description: 'Find jobs and build tailored resumes',
      icon: '🔍',
      href: '/jobs',
      color: 'purple'
    },
    {
      title: 'Create Resume & Cover Letter',
      description: 'Build resume and cover letter for specific job',
      icon: '📄',
      href: '/create-resume',
      color: 'blue'
    },
    {
      title: 'Edit Profile',
      description: 'Update your personal information',
      icon: '👤',
      href: '/profile',
      color: 'green'
    }
  ];

  useEffect(async () => {
    // TODO: Replace with actual API calls
    const fetchDB = await emailFetch();

    // Fetch user stats
    // setStats(fetchedStats);
    
    // Fetch user name
    // setUserName(fetchedUserName);
    
    // Fetch jobs data
    // setJobs(fetchedJobs);
    
    // Fetch recent activities
    // setActivities(fetchedActivities);
  }, []);

  const handleSettingsClick = () => {
    // TODO: Navigate to settings or open settings modal
    console.log('Settings clicked');
  };

  const handleSignOutClick = () => {
    // TODO: Implement sign out logic
    console.log('Sign out clicked');
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-900 via-purple-950 to-black p-6">
        <Stars />
      <div className="max-w-7xl mx-auto">
        <DashboardHeader 
          userName={userName}
          onSettingsClick={handleSettingsClick}
          onSignOutClick={handleSignOutClick}
        />

        <DashboardStats 
          totalApplications={stats.totalApplications}
          activeResumes={stats.activeResumes}
          profileCompletion={stats.profileCompletion}
        />

        <QuickActions actions={quickActions} />

        <JobListings jobs={jobs} />

        <RecentActivity activities={activities} />

        {/* Footer */}
        <footer className="mt-8 text-center text-gray-400">
          <p>&copy; 2025 Resume Builder. Build your future, one resume at a time.</p>
        </footer>
      </div>
    </div>
  );
}
