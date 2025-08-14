import Link from 'next/link';

interface QuickAction {
  title: string;
  description: string;
  icon: string;
  href: string;
  color: string;
}

interface QuickActionsProps {
  actions: QuickAction[];
}

export default function QuickActions({ actions }: QuickActionsProps) {
  return (
    <div className="mb-8">
      <h2 className="text-2xl font-semibold text-white mb-6">Quick Actions</h2>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {actions.map((action, index) => (
          <Link 
            key={index} 
            href={action.href}
            className="purple-card p-6 hover:scale-105 transition-transform cursor-pointer group"
          >
            <div className="text-4xl mb-4">{action.icon}</div>
            <h3 className="text-xl font-semibold text-white mb-2 group-hover:text-purple-300 transition-colors">
              {action.title}
            </h3>
            <p className="text-gray-400 text-sm">{action.description}</p>
          </Link>
        ))}
      </div>
    </div>
  );
}

export type { QuickAction };
