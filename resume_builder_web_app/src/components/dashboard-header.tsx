interface DashboardHeaderProps {
  userName: string;
  onSettingsClick?: () => void;
  onSignOutClick?: () => void;
}

export default function DashboardHeader({ 
  userName, 
  onSettingsClick, 
  onSignOutClick 
}: DashboardHeaderProps) {
  return (
    <header className="mb-8">
      <div className="flex items-center justify-between mb-4">
        <div>
          <h1 className="text-4xl font-bold text-white mb-2">
            Welcome back, <span className="purple-gradient-text">{userName}</span>
          </h1>
          <p className="text-gray-300">Find jobs and build tailored resumes & cover letters</p>
        </div>
        <div className="flex gap-4">
          <button 
            onClick={onSettingsClick}
            className="bg-purple-600 hover:bg-purple-700 text-white px-6 py-2 rounded-lg transition-colors"
          >
            Settings
          </button>
          <button 
            onClick={onSignOutClick}
            className="bg-red-600 hover:bg-red-700 text-white px-6 py-2 rounded-lg transition-colors"
          >
            Sign Out
          </button>
        </div>
      </div>
    </header>
  );
}
