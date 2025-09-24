"use client";

interface LoadingProp {
  message?: string;
}

export default function Loading({ message = "" }: LoadingProp) {
  return (
    <div className="fixed inset-0 z-[9999] flex justify-center items-center bg-black/40 backdrop-blur-sm">
      <div className="flex flex-col items-center gap-3 p-4 sm:gap-4 sm:p-6">
        <img
          src="/resource/loading.gif"
          alt="Loading..."
          className="w-20 h-20 sm:w-32 sm:h-32 rounded-xl"
        />
        {message && (
          <p className="text-white text-sm sm:text-lg font-semibold text-center">
            {message}
          </p>
        )}
      </div>
    </div>
  );
}
