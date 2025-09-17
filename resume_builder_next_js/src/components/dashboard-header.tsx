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
      <div className="flex items-center justify-between rounded-2xl px-9 py-6 shadow-lg border border-[var(--border-color)] bg-[var(--card-bg)] backdrop-blur-xl">
        {/* Left Section */}
        <div>
          <h1 className="text-3xl font-extrabold text-[var(--foreground)] flex items-center gap-2">
            👋 Hello,
            <span className="bg-gradient-to-r from-purple-400 to-pink-500 bg-clip-text text-transparent">{userName}</span>
            <img
              src="/resource/cat_playing.gif"
              alt="Cat Playing"
              className="w-20 h-20"
            />
          </h1>
          <p className="text-[var(--secondary-purple)] mt-1 text-sm">
            Find jobs, track applications, and build tailored resumes & cover
            letters.
          </p>
        </div>

        {/* Right Section */}
        <div className="flex gap-3">
          <button
            onClick={onSignOutClick}
            className="px-5 py-2 rounded-xl text-sm font-semibold bg-gradient-to-r from-red-600 to-red-800 text-white hover:opacity-90 transition-all shadow-lg"
          >
            Sign Out
          </button>
        </div>
      </div>
    </header>
  );
}
