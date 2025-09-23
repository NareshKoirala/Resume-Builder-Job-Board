import Link from "next/link"; // Make sure you have this for navigation

interface Job {
  id: string;
  title: string;
  company: string;
  location: string;
  description: string;
  salaryRange: string;
}

interface JobListingsProps {
  jobs: Job[];
}

export default function JobListings({ jobs }: JobListingsProps) {
  return (
    <div className="max-w-5xl mx-auto px-6 pt-12">
      {/* Header / Title Block */}
      <div className="flex flex-col md:flex-row items-center justify-between mb-10 bg-purple-900/20 backdrop-blur-md p-8 rounded-2xl shadow-lg border border-purple-800">
        {/* Title */}
        <h1 className="text-5xl font-extrabold text-white mb-6 md:mb-0 flex items-center gap-3">
          <span className="text-5xl">💼</span>
          Job Board
        </h1>

        {/* Action Buttons */}
        <div className="flex flex-col sm:flex-row gap-4">
          <Link
            href="/dashboard"
            className="text-purple-200 border border-purple-400 hover:bg-purple-800/30 px-5 py-3 rounded-lg font-medium transition-all shadow-sm hover:shadow-md text-center"
          >
            Go Back
          </Link>
          <button className="bg-purple-600 hover:bg-purple-700 text-white px-5 py-3 rounded-lg font-medium shadow-md transition-transform transform hover:-translate-y-0.5">
            Add Manual Job
          </button>
        </div>
      </div>

      {/* No Jobs Available */}
      {jobs.length === 0 && (
        <div className="flex justify-center px-4">
          <div className="bg-gradient-to-r from-purple-700 to-purple-900 rounded-xl p-12 text-center shadow-lg max-w-2xl w-full">
            <div className="text-6xl mb-4">💼</div>
            <h3 className="text-xl font-semibold text-white mb-2">
              No Jobs Available 😞
            </h3>
            <p className="text-gray-300">
              Check back later for new job opportunities
            </p>
          </div>
        </div>
      )}

      {/* Job List */}
      {jobs.length > 0 && (
        <div className="space-y-6">
          {jobs.map((job) => (
            <div
              key={job.id}
              className="flex flex-col md:flex-row items-start justify-between bg-gradient-to-r from-purple-800 to-purple-900 p-6 rounded-xl shadow-md hover:shadow-lg transition-shadow"
            >
              {/* Job Details */}
              <div className="flex-1">
                <div className="flex items-center gap-3 mb-2">
                  <h3 className="text-lg font-semibold text-white">
                    {job.title}
                  </h3>
                </div>
                <p className="text-purple-300 font-medium">{job.company}</p>
                <p className="text-gray-400 text-sm mb-3">{job.location}</p>
                <p className="text-gray-300 text-sm leading-relaxed mb-3">
                  {job.description}
                </p>
                <span className="text-purple-200 font-semibold">
                  {job.salaryRange}
                </span>
              </div>

              {/* Apply Button */}
              <div className="mt-4 md:mt-0 md:ml-6">
                <button className="bg-purple-600 hover:bg-purple-700 text-white px-6 py-2 rounded-lg text-sm font-medium shadow transition-colors">
                  Apply Now
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export type { Job };
