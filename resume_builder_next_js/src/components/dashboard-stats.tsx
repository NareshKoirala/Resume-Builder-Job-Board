interface DashboardStatsProps {
  totalApplications: number;
  activeResumes: number;
  savedJobs: number;
}

export default function DashboardStats({ 
  totalApplications, 
  activeResumes,
  savedJobs
}: DashboardStatsProps) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
      {/* Total Applications */}
      <div className="purple-card p-6 text-center">
        <div className="text-3xl font-bold text-[var(--primary-purple)] mb-2">
          {totalApplications}
        </div>
        <div className="text-[var(--secondary-purple)]">Job Applications</div>
      </div>

      {/* Active Resumes */}
      <div className="purple-card p-6 text-center">
        <div className="text-3xl font-bold text-[var(--primary-purple)] mb-2">
          {activeResumes}
        </div>
        <div className="text-[var(--secondary-purple)]">Active Resumes (MAX: 10)</div>
      </div>

      {/* Saved Jobs */}
      <div className="purple-card p-6 text-center">
        <div className="text-3xl font-bold text-[var(--primary-purple)] mb-2">
          {savedJobs}
        </div>
        <div className="text-[var(--secondary-purple)]">Saved Jobs</div>
      </div>
    </div>
  );
}
