import type { Metadata } from "next";
import "../css/globals.css";


export const metadata: Metadata = {
  title: "Resume Builder",
  description: "Create and manage your resume effortlessly.",
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body className="antialiased">
        {children}
      </body>
    </html>
  );
}
