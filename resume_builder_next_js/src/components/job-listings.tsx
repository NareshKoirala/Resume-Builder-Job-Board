import Link from 'next/link';

interface Job {
  id: string;
  title: string;
  company: string;
  location: string;
  description: string;
  salaryRange: string;
  badge?: {
    text: string;
    color: string;
  };
}

interface JobListingsProps {
  jobs: Job[];
}

export default function JobListings({ jobs }: JobListingsProps) {
  if (jobs.length === 0) {
    return (
      <div className="mb-8">
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-2xl font-semibold text-white">Featured Jobs</h2>
          <Link href="/jobs" className="text-purple-300 hover:text-purple-200 transition-colors">
            View All Jobs →
          </Link>
        </div>
        <div className="purple-card p-12 text-center">
          <div className="text-6xl mb-4">💼</div>
          <h3 className="text-xl font-semibold text-white mb-2">No Jobs Available</h3>
          <p className="text-gray-400">Check back later for new job opportunities</p>
        </div>
      </div>
    );
  }

  return (
    <div className="mb-8">
      <div className="flex items-center justify-between mb-6">
        <h2 className="text-2xl font-semibold text-white">Featured Jobs</h2>
        <Link href="/jobs" className="text-purple-300 hover:text-purple-200 transition-colors">
          View All Jobs →
        </Link>
      </div>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {jobs.map((job) => (
          <div key={job.id} className="purple-card p-6 hover:scale-105 transition-transform cursor-pointer">
            <div className="flex items-start justify-between mb-4">
              <div>
                <h3 className="text-xl font-semibold text-white mb-2">{job.title}</h3>
                <p className="text-purple-300 font-medium">{job.company}</p>
                <p className="text-gray-400 text-sm">{job.location}</p>
              </div>
              {job.badge && (
                <span className={`${job.badge.color} text-white text-xs px-2 py-1 rounded-full`}>
                  {job.badge.text}
                </span>
              )}
            </div>
            <p className="text-gray-300 text-sm mb-4">{job.description}</p>
            <div className="flex items-center justify-between">
              <span className="text-purple-200 font-bold">{job.salaryRange}</span>
              <button className="bg-purple-600 hover:bg-purple-700 text-white px-4 py-2 rounded text-sm transition-colors">
                Apply Now
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export type { Job };
