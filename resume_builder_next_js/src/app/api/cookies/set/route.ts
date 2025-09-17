import { NextResponse, NextRequest } from "next/server";

export async function POST(request: NextRequest) {
  try {
    // Read request body safely
    const bodyText = await request.text();
    if (!bodyText) {
      return NextResponse.json({ error: "Empty body" }, { status: 400 });
    }

    let age: number, data: string, id: string;
    try {
      ({ age, data, id } = JSON.parse(bodyText));
    } catch {
      return NextResponse.json({ error: "Invalid JSON" }, { status: 400 });
    }

    if (!id) {
      return NextResponse.json({ error: "Missing cookie id" }, { status: 400 });
    }

    const response = NextResponse.json({ success: true });

    // If age=0 → clear cookie
    if (age === 0) {
      response.cookies.set(id, "", {
        httpOnly: true,
        maxAge: 0,
        path: "/",
      });
    } else {
      // Otherwise set/update cookie
      response.cookies.set(id, data ?? "", {
        httpOnly: true,
        maxAge: age,
        path: "/",
      });
    }

    return response;
  } catch (err) {
    console.error("Error in /api/cookies/set:", err);
    return NextResponse.json({ error: "Server error" }, { status: 500 });
  }
}
