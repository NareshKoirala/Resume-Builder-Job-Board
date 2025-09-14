interface DashboardStatsProps {
  totalApplications: number;
  activeResumes: number;
}

export default function DashboardStats({ 
  totalApplications, 
  activeResumes
}: DashboardStatsProps) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
      <div className="purple-card p-6 text-center">
        <div className="text-3xl font-bold text-purple-300 mb-2">{totalApplications}</div>
        <div className="text-gray-300">Job Applications</div>
      </div>
      
      <div className="purple-card p-6 text-center">
        <div className="text-3xl font-bold text-purple-300 mb-2">{activeResumes}</div>
        <div className="text-gray-300">Active Resumes</div>
      </div>
    </div>
  );
}
