import { NextResponse, NextRequest } from "next/server";

// Disable TLS certificate validation (for development purposes only)
process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";

const RESUME_API_URL = process.env.RESUME_API_URL;

export async function POST(req: NextRequest) {
  try {
    const userData = await req.json(); // parse JSON from client
    // console.log("User Send Data (JobBoard Route)", userData);

    const finalURL = `${RESUME_API_URL}/JobBoard`;

    const response = await fetch(finalURL, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(userData), // send the actual payload
    });
    
    console.log(response);

    const responseData = await response.json();
    // console.log("Response From api (Job Board Route)", responseData);

    return NextResponse.json(responseData);
  } catch (error) {
    console.error("Error in JobBoard route:", error);
    return NextResponse.json({ error: "Failed to fetch jobs" }, { status: 500 });
  }
}
