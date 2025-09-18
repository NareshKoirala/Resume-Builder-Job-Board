import { NextResponse, NextRequest } from "next/server";
import { Job } from "@/components/job-listings";

// Disable TLS certificate validation (for development purposes only)
process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";

const RESUME_API_URL = process.env.RESUME_API_URL;

export async function POST(data: NextRequest){

    const userData = await data.json();

    // console.log("User Send Data (JobBoard Route)", userData);

    const finalURL = `${RESUME_API_URL}/JobBoard`

    try
    {
        const response = await fetch(finalURL,{
            method: "POST",
            headers:{
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        })

        const responseData = await response.json();

        // console.log("Response From api (Job Board Route)", responseData);

        return NextResponse.json(responseData);
    }
    catch (error)
    {

    }
}