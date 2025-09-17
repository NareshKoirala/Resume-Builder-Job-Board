interface DashboardHeaderProps {
  userName: string;
  onSignOutClick?: () => void;
}

export default function DashboardHeader({
  userName,
  onSignOutClick,
}: DashboardHeaderProps) {
  return (
    <header className="mb-10">
      <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-6 rounded-2xl px-6 md:px-9 py-6 shadow-lg border border-[var(--border-color)] bg-[var(--card-bg)] backdrop-blur-xl">
        
        {/* Left Section */}
        <div className="flex flex-col">
          <h1 className="text-2xl md:text-3xl font-extrabold text-[var(--foreground)] flex items-center gap-2 flex-wrap">
            👋 Hello,
            <span className="purple-gradient-text">{userName}</span>
            <img
              src="/resource/cat_playing.gif"
              alt="Cat Playing"
              className="w-14 h-14 md:w-20 md:h-20"
            />
          </h1>
          <p className="text-[var(--secondary-purple)] mt-2 text-sm md:text-base">
            Find jobs, track applications, and build tailored resumes & cover
            letters.
          </p>
        </div>

        {/* Right Section */}
        {onSignOutClick && (
          <div className="flex md:justify-end">
            <button
              onClick={onSignOutClick}
              className="px-5 py-2 rounded-xl text-sm font-semibold bg-gradient-to-r from-red-600 to-red-800 text-white hover:opacity-90 transition-all shadow-lg"
            >
              Sign Out
            </button>
          </div>
        )}
      </div>
    </header>
  );
}
