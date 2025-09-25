import { NextResponse, NextRequest } from "next/server";

// Disable TLS certificate validation (for development purposes only)
process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";

const RESUME_API_URL = process.env.RESUME_API_URL;

export async function POST(req: NextRequest) {
  try {
    const { type, data } = await req.json(); // parse JSON from client
    
    const publicId = req.cookies.get("publicId")?.value || "no-id";

    const finalURL = `${RESUME_API_URL}/${type}/Json/${publicId}`;

    const response = await fetch(finalURL, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data), // send the actual payload
    });
    
    const responseData = await response.json();

    return NextResponse.json(responseData.resume);
  } catch (error) {
    console.error("Error in JobBoard route:", error);
    return NextResponse.json({ error: "Failed to fetch jobs" }, { status: 500 });
  }
}
