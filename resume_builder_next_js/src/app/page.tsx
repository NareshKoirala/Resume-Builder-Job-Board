"use client";

import { useRouter } from "next/navigation";
import { useState } from "react";

export default function Home() {
  const [showApiFeatures, setShowApiFeatures] = useState(false);
  const router = useRouter();
  const handleSignIn = () => {
    router.push("/auth");
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center p-8">
      <div className="text-center max-w-4xl mx-auto">
        <h1 className="text-6xl font-bold mb-6 bg-gradient-to-r from-purple-200 via-purple-100 to-white bg-clip-text text-transparent drop-shadow-lg">
          Resume Builder & Job Tracker
        </h1>
        <p className="text-xl mb-8 text-purple-100 leading-relaxed">
          Track job applications and create tailored resumes & cover letters
          with our custom API. Built by Naresh Koirala, this platform generates
          personalized documents based on job requirements and provides
          job-matching insights.
        </p>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mt-12">
          <div className="p-6 rounded-xl bg-purple-900/30 border border-purple-400/30 hover:shadow-xl hover:shadow-purple-500/20 transition-all hover:border-purple-300/50">
            <div className="text-3xl mb-4">📋</div>
            <h3 className="text-lg font-semibold text-purple-100 mb-2">
              Job Tracking
            </h3>
            <p className="text-purple-200 text-sm">
              Track and manage your job applications with detailed status
              updates
            </p>
          </div>
          <div className="p-6 rounded-xl bg-purple-900/30 border border-purple-400/30 hover:shadow-xl hover:shadow-purple-500/20 transition-all hover:border-purple-300/50">
            <div className="text-3xl mb-4">🎯</div>
            <h3 className="text-lg font-semibold text-purple-100 mb-2">
              Smart Matching
            </h3>
            <p className="text-purple-200 text-sm">
              Get job-matching percentages and skill gap analysis for each role
            </p>
          </div>
          <div className="p-6 rounded-xl bg-purple-900/30 border border-purple-400/30 hover:shadow-xl hover:shadow-purple-500/20 transition-all hover:border-purple-300/50">
            <div className="text-3xl mb-4">📄</div>
            <h3 className="text-lg font-semibold text-purple-100 mb-2">
              Tailored Documents
            </h3>
            <p className="text-purple-200 text-sm">
              Generate customized resumes and cover letters based on job
              requirements
            </p>
          </div>
          <div className="p-6 rounded-xl bg-purple-900/30 border border-purple-400/30 hover:shadow-xl hover:shadow-purple-500/20 transition-all hover:border-purple-300/50">
            <div className="text-3xl mb-4">⚡</div>
            <h3 className="text-lg font-semibold text-purple-100 mb-2">
              Custom API
            </h3>
            <p className="text-purple-200 text-sm">
              Powered by ASP.NET Core API for fast, reliable document generation
            </p>
          </div>
        </div>

        <div className="mt-12">
          <button
            onClick={() => setShowApiFeatures(!showApiFeatures)}
            className="mb-6 px-6 py-3 bg-purple-800/40 hover:bg-purple-700/50 border border-purple-400/30 hover:border-purple-300/50 text-purple-100 font-medium rounded-lg transition-all flex items-center mx-auto"
          >
            <span className="mr-2">View API Features</span>
            <svg
              className={`w-4 h-4 transition-transform ${
                showApiFeatures ? "rotate-180" : ""
              }`}
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M19 9l-7 7-7-7"
              />
            </svg>
          </button>

          {showApiFeatures && (
            <div className="mb-8 p-8 rounded-2xl bg-gradient-to-br from-purple-900/40 to-violet-900/30 border border-purple-400/30 backdrop-blur-sm animate-in slide-in-from-top duration-300">
              <h2 className="text-3xl font-bold text-center text-purple-100 mb-8 bg-gradient-to-r from-purple-200 to-violet-200 bg-clip-text text-transparent">
                API Features
              </h2>
              <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                <div className="p-6 rounded-xl bg-purple-800/30 border border-purple-400/20 hover:border-purple-300/40 transition-all hover:shadow-lg hover:shadow-purple-500/10">
                  <div className="flex items-center mb-3">
                    <div className="text-2xl mr-3">📝</div>
                    <h4 className="text-lg font-semibold text-purple-100">
                      Resume & Cover Letter Creation
                    </h4>
                  </div>
                  <p className="text-purple-200 leading-relaxed">
                    Build customizable documents with personal info, experience,
                    and skills sections
                  </p>
                </div>
                <div className="p-6 rounded-xl bg-purple-800/30 border border-purple-400/20 hover:border-purple-300/40 transition-all hover:shadow-lg hover:shadow-purple-500/10">
                  <div className="flex items-center mb-3">
                    <div className="text-2xl mr-3">👤</div>
                    <h4 className="text-lg font-semibold text-purple-100">
                      User Management
                    </h4>
                  </div>
                  <p className="text-purple-200 leading-relaxed">
                    Manage user profiles and their associated resumes and
                    applications
                  </p>
                </div>
                <div className="p-6 rounded-xl bg-purple-800/30 border border-purple-400/20 hover:border-purple-300/40 transition-all hover:shadow-lg hover:shadow-purple-500/10">
                  <div className="flex items-center mb-3">
                    <div className="text-2xl mr-3">📤</div>
                    <h4 className="text-lg font-semibold text-purple-100">
                      Export Options
                    </h4>
                  </div>
                  <p className="text-purple-200 leading-relaxed">
                    Currently supports JSON model export
                  </p>
                  <div className="mt-2 text-xs text-purple-300 bg-purple-900/40 px-3 py-1 rounded-full inline-block">
                    PDF, Word, HTML coming soon
                  </div>
                </div>
                <div className="p-6 rounded-xl bg-purple-800/30 border border-purple-400/20 hover:border-purple-300/40 transition-all hover:shadow-lg hover:shadow-purple-500/10">
                  <div className="flex items-center mb-3">
                    <div className="text-2xl mr-3">🎯</div>
                    <h4 className="text-lg font-semibold text-purple-100">
                      Job Matching
                    </h4>
                  </div>
                  <p className="text-purple-200 leading-relaxed">
                    Analyze profile alignment with job requirements and skill
                    gaps
                  </p>
                </div>
              </div>
            </div>
          )}
        </div>

        <div className="mt-8">
          <button
            onClick={handleSignIn}
            className="px-8 py-4 bg-gradient-to-r from-purple-500 to-violet-600 hover:from-purple-400 hover:to-violet-500 text-white font-semibold rounded-lg shadow-lg hover:shadow-purple-500/50 hover:shadow-xl transition-all transform hover:-translate-y-1 mr-4"
          >
            Sign In
          </button>
        </div>

        <div className="mt-8 text-sm text-purple-300">
          <p>
            Powered by custom ASP.NET Core API • Developed by Naresh Koirala
          </p>
        </div>
      </div>
    </div>
  );
}
