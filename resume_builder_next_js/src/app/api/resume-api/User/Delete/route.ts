import { NextRequest, NextResponse } from "next/server";

const RESUME_API_URL = process.env.RESUME_API_URL!;

export async function POST(request: NextRequest) {
  try {
    const { field, data } = await request.json();

    // Grab cookies (publicId) from request
    const publicId = request.cookies.get("publicId")?.value;
    if (!publicId) {
      return NextResponse.json(
        { error: "publicId cookie is missing" },
        { status: 400 }
      );
    }

    // Build URL
    const url = `${RESUME_API_URL}/Users/Delete/${field}/${publicId}`;

    // console.log("Route In Delete API ->", url);
    // console.log("Data You Send ->", data);

    // Call backend API
    const res = await fetch(url, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data), // if your API expects a body with DELETE
    });

    // Parse backend response
    const result = await res.json().catch(() => ({}));

    if (!res.ok) {
      return NextResponse.json(
        { error: "Failed to delete", details: result },
        { status: res.status }
      );
    }

    return NextResponse.json({ success: true, result });
  } catch (err: any) {
    console.error("Delete API error:", err);
    return NextResponse.json(
      { error: "Something went wrong", details: err.message },
      { status: 500 }
    );
  }
}
