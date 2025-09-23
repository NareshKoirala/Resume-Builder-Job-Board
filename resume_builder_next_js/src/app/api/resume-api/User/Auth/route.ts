import { NextRequest, NextResponse } from "next/server";

// Disable TLS certificate validation (for development purposes only)
process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";

const RESUME_API_URL = process.env.RESUME_API_URL;

export async function POST(request: NextRequest) {
  try {
    const { path, passKey, Email } = await request.json();

    if (!RESUME_API_URL) {
      return NextResponse.json(
        { error: "Resume API URL not configured" },
        { status: 500 }
      );
    }

    const apiUrl = `${RESUME_API_URL}/${path}/${encodeURIComponent(
      passKey
    )}/${encodeURIComponent(Email)}`;

    const apiResponse = await fetch(apiUrl, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (!apiResponse.ok) {
      const errorData = await apiResponse.json();
      console.error("Error response from Resume API:", errorData);
      return NextResponse.json(
        { error: errorData.message || "Failed to create user" },
        { status: apiResponse.status }
      );
    }
    const response = await apiResponse.json();

    // Create a new NextResponse to attach cookies
    const res = NextResponse.json({ response: response });

    // Set cookie with publicId from Resume API
    res.cookies.set("publicId", response.response.publicId, {
      httpOnly: true,
      path: "/",
      maxAge: 60 * 60 * 24 * 7, // 1 week
    });

    return res;
  } catch (error) {
    console.error("Error in POST /api/resume-api/User/Auth:", error);
    return NextResponse.json(
      { error: "Internal Server Error" },
      { status: 500 }
    );
  }
}
