import { NextResponse, NextRequest } from "next/server";

// process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";

const RESUME_API_URL = process.env.RESUME_API_URL;

export async function POST(req: NextRequest) {
  try {
    const { type, data } = await req.json();
    const publicId = req.cookies.get("publicId")?.value || "no-id";

    const finalURL = `${RESUME_API_URL}/${type}/docx/${publicId}`;

    const response = await fetch(finalURL, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      return NextResponse.json({ error: "Failed to fetch resume" }, { status: response.status });
    }

    const blob = await response.arrayBuffer(); // get raw bytes
    return new NextResponse(blob, {
      headers: {
        "Content-Type": "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "Content-Disposition": 'attachment; filename="resume.docx"',
      },
    });
  } catch (error) {
    console.error("Error in JobBoard route:", error);
    return NextResponse.json({ error: "Failed to fetch resume" }, { status: 500 });
  }
}
