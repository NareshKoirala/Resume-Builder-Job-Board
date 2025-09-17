import { NextResponse, NextRequest } from "next/server";

export async function GET(request: NextRequest) {  
  // Get query params from URL
  const url = new URL(request.url);
  const key = url.searchParams.get("key") || ""; // e.g., ?key=userEmail

  const data = request.cookies.get(key);

  return NextResponse.json({ data: data?.value });
}
