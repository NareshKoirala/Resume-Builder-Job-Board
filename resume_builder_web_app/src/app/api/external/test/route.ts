import { NextRequest, NextResponse } from 'next/server';

const API_BASE_URL = 'https://resume-builder-api-8m2c.onrender.com';

export async function POST(request: NextRequest) {
  try {
    // Test with minimal required data only
    const minimalData = {
      Pass: "test123",
      FirstName: "Test",
      LastName: "User",
      Email: `test${Date.now()}@example.com`, // Unique email to avoid duplicates
      Mobile: "1234567890",
      Location: "Test City",
      Province: "Test Province",
      JobField: "Software Developer",
      PortfolioUrl: "",
      LinkedinUrl: "",
      UserSummary: "",
      Education: [],
      WorkExperience: [],
      Certificates: [],
      Skills: [],
      Projects: []
    };
    
    console.log('Testing external API with minimal data:', JSON.stringify(minimalData, null, 2));

    const response = await fetch(`${API_BASE_URL}/User/register`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
      body: JSON.stringify(minimalData),
      signal: AbortSignal.timeout(15000), // 15 second timeout
    });

    console.log('API Test Response Status:', response.status);
    console.log('API Test Response Headers:', Object.fromEntries(response.headers.entries()));

    const responseText = await response.text();
    console.log('API Test Response Body:', responseText);

    if (!response.ok) {
      return NextResponse.json({
        success: false,
        message: `API test failed: ${response.status} ${response.statusText}`,
        details: responseText,
        testData: minimalData
      }, { status: response.status });
    }

    const result = JSON.parse(responseText);
    return NextResponse.json({
      success: true,
      message: 'API test successful',
      result: result,
      testData: minimalData
    });

  } catch (error) {
    console.error('API test error:', error);
    return NextResponse.json({
      success: false,
      message: 'API test failed with error',
      error: error instanceof Error ? error.message : 'Unknown error'
    }, { status: 500 });
  }
}
