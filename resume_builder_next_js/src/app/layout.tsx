import type { Metadata } from "next";
import "../css/globals.css";
import Stars from "@/components/stars";

export const metadata: Metadata = {
  title: "Resume Builder",
  description: "Create and manage your resume effortlessly.",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <link rel="icon" href="/Resource/zoro_main.ico" />
      <body className="antialiased">
        <Stars count={20} />
        {children}
      </body>
    </html>
  );
}
