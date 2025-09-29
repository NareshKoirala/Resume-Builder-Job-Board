import { NextRequest, NextResponse } from "next/server";

// Disable TLS certificate validation (for development purposes only)
process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";

const RESUME_API_URL = process.env.RESUME_API_URL;

export async function POST(request: NextRequest) {
    console.log("Received POST request to /api/resume-api/User/Public @ ", RESUME_API_URL);

    if (!RESUME_API_URL) {
        return NextResponse.json({ error: "RESUME_API_URL is not defined" }, { status: 500 });
    }

    const { path, publicId, process } = await request.json();

    const apiUrl = `${RESUME_API_URL}/${path}/${publicId}`;

    console.log(`Making ${process} request to:`, apiUrl);

    try {
        const response = await fetch(apiUrl, {
            method: process,
            headers: {
                "Content-Type": "application/json",
            },
        });

        if (!response.ok) {
            const errorData = await response.json();
            console.error("Error response from Resume API:", errorData);
            return NextResponse.json(
                { error: errorData.message || "Failed to create user" },
                { status: response.status }
            );
        }
        const responseData = await response.json();

        console.log("Response from Resume API:", responseData);

        return NextResponse.json({ response: responseData });

    } catch (error) {
        console.error(`Error in POST ${apiUrl}:`, error);
        return NextResponse.json(
            { error: "Internal Server Error" },
            { status: 500 }
        );
    }
}

export async function PUT(request: NextRequest) {

    if (!RESUME_API_URL) {
        return NextResponse.json({ error: "RESUME_API_URL is not defined" }, { status: 500 });
    }

    const { path, publicId, process, data } = await request.json();

    // console.log("PUT Request Data:", { path, publicId, process, data });

    const apiUrl = `${RESUME_API_URL}/${path}/${publicId}`;
    
    try {
        const response = await fetch(apiUrl, {
            method: process,
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            const errorData = await response.json();
            console.error("Error response from Resume API:", errorData);
            return NextResponse.json(
                { error: errorData.message || "Failed to update user" },
                { status: response.status }
            );
        }

        const responseData = await response.json();

        return NextResponse.json({ response: responseData });

    } catch (error) {
        console.error(`Error in PUT ${apiUrl}:`, error);
        return NextResponse.json(
            { error: "Internal Server Error" },
            { status: 500 }
        );
    }
}