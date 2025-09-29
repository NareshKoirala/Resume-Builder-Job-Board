'use client';

import { useState } from "react";
import SignIn from "@/components/sign-in";
import SignUp from "@/components/sign-up";

export default function AuthPage() {
  const [isSignUp, setIsSignUp] = useState(false);

  const toggleAuthMode = () => {
    setIsSignUp(!isSignUp);
  };

  return (
    <div className="flex justify-center items-center min-h-screen w-full px-4">
      <div className="w-full">
        {isSignUp ? (
          <SignUp onToggleMode={toggleAuthMode} />
        ) : (
          <SignIn onToggleMode={toggleAuthMode} />
        )}
      </div>
    </div>
  );
}
