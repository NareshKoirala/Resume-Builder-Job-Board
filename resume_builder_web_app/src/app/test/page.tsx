"use client";

// Test page for development - can be removed in production
export default function Test() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-900 via-purple-950 to-black flex items-center justify-center p-6">
      <div className="text-center">
        <h1 className="text-4xl font-bold text-white mb-4">Test Page</h1>
        <p className="text-gray-300">This page is for development testing purposes.</p>
      </div>
    </div>
  );
}
