interface Activity {
  id: string;
  icon: string;
  iconColor: string;
  title: string;
  description: string;
  timestamp: string;
}

interface RecentActivityProps {
  activities: Activity[];
}

export default function RecentActivity({ activities }: RecentActivityProps) {
  if (activities.length === 0) {
    return (
      <div className="purple-card p-6">
        <h2 className="text-2xl font-semibold text-white mb-6">Recent Activity</h2>
        <div className="text-center py-8">
          <div className="text-6xl mb-4">📊</div>
          <h3 className="text-xl font-semibold text-white mb-2">No Recent Activity</h3>
          <p className="text-gray-400">Your activity will appear here once you start using the platform</p>
        </div>
      </div>
    );
  }

  return (
    <div className="purple-card p-6">
      <h2 className="text-2xl font-semibold text-white mb-6">Recent Activity</h2>
      <div className="space-y-4">
        {activities.map((activity, index) => (
          <div 
            key={activity.id} 
            className={`flex items-center justify-between ${
              index < activities.length - 1 ? 'border-b border-purple-700 pb-4' : ''
            }`}
          >
            <div className="flex items-center gap-4">
              <div className={`w-10 h-10 ${activity.iconColor} rounded-full flex items-center justify-center`}>
                {activity.icon}
              </div>
              <div>
                <h4 className="text-white font-medium">{activity.title}</h4>
                <p className="text-gray-400 text-sm">{activity.description}</p>
              </div>
            </div>
            <button className="text-purple-300 hover:text-purple-200 transition-colors">
              View →
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}

export type { Activity };
