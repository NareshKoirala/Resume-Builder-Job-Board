import { NextRequest, NextResponse } from 'next/server';
import { UserRegisterDto } from '@/model/data-structure';

const API_BASE_URL = 'https://resume-builder-api-8m2c.onrender.com';

/**
 * Check if a value is a placeholder or invalid
 */
function isValidValue(value: string | undefined): boolean {
  if (!value) return false;
  const trimmed = value.trim().toLowerCase();
  return trimmed.length >= 2 && 
         trimmed !== 'string' && 
         trimmed !== 'null' && 
         trimmed !== 'undefined' &&
         trimmed !== 'n/a' &&
         trimmed !== 'tbd' &&
         trimmed !== 'todo';
}

/**
 * Transform user data from our format to external API format
 */
function transformUserDataForExternalAPI(userData: UserRegisterDto) {
  // Validate and sanitize the job field
  let jobField = userData.job_field || '';
  if (!isValidValue(jobField)) {
    jobField = 'Software Developer'; // Default valid job field
  }

  // Validate user summary
  let userSummary = userData.user_summary || '';
  if (!isValidValue(userSummary)) {
    userSummary = `Experienced ${jobField} with strong technical skills and passion for innovation.`;
  }
  
  return {
    Pass: userData.pass,
    FirstName: userData.first_name,
    LastName: userData.last_name,
    Email: userData.email,
    Mobile: userData.mobile,
    Location: userData.location,
    Province: userData.province,
    JobField: jobField,
    PortfolioUrl: isValidValue(userData.portfolio_url) ? userData.portfolio_url : '',
    LinkedinUrl: isValidValue(userData.linkedin_url) ? userData.linkedin_url : '',
    UserSummary: userSummary,
    // Only include arrays if they have valid entries
    Education: userData.education
      .filter(edu => isValidValue(edu.institution_name))
      .map(edu => ({
        InstitutionName: edu.institution_name,
        Date: isValidValue(edu.date) ? edu.date : '',
        Location: isValidValue(edu.location) ? edu.location : '',
        Details: isValidValue(edu.details) ? edu.details : ''
      })),
    WorkExperience: userData.work_experience
      .filter(work => isValidValue(work.company_name))
      .map(work => ({
        CompanyName: work.company_name,
        Date: isValidValue(work.date) ? work.date : '',
        Location: isValidValue(work.location) ? work.location : '',
        Details: isValidValue(work.details) ? work.details : ''
      })),
    Certificates: userData.certificates
      .filter(cert => isValidValue(cert.certificate_name))
      .map(cert => ({
        CertificateName: cert.certificate_name,
        Details: isValidValue(cert.details) ? cert.details : ''
      })),
    Skills: userData.skills
      .filter(skill => isValidValue(skill.skill_name))
      .map(skill => ({
        SkillName: skill.skill_name
      })),
    Projects: userData.projects
      .filter(project => isValidValue(project.project_name))
      .map(project => ({
        ProjectName: project.project_name,
        Description: isValidValue(project.description) ? project.description : ''
      }))
  };
}

export async function POST(request: NextRequest) {
  try {
    const userData: UserRegisterDto = await request.json();
    
    console.log('Original user data received:', { 
      email: userData.email,
      userData: JSON.stringify(userData, null, 2) 
    });

    // Validate required fields
    const requiredFields = ['pass', 'first_name', 'last_name', 'email', 'mobile', 'location', 'province'];
    const missingFields = requiredFields.filter(field => !userData[field as keyof UserRegisterDto]);
    
    if (missingFields.length > 0) {
      return NextResponse.json(
        { 
          success: false, 
          message: `Missing required fields: ${missingFields.join(', ')}`,
          missingFields
        },
        { status: 400 }
      );
    }

    // Validate email format
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(userData.email)) {
      return NextResponse.json(
        { 
          success: false, 
          message: 'Invalid email format',
        },
        { status: 400 }
      );
    }

    // Transform data to match external API format
    const transformedData = transformUserDataForExternalAPI(userData);
    
    console.log('Transformed data for external API:', JSON.stringify(transformedData, null, 2));

    // Add timestamp to email for testing to avoid duplicates
    const testEmail = `${Date.now()}_${transformedData.Email}`;
    const testData = {
      ...transformedData,
      Email: testEmail
    };

    console.log('Using test email to avoid duplicates:', testEmail);

    // Validate that we have at least some meaningful data
    const hasValidData = (
      transformedData.Education.length > 0 ||
      transformedData.WorkExperience.length > 0 ||
      transformedData.Skills.length > 0 ||
      transformedData.Projects.length > 0 ||
      isValidValue(transformedData.UserSummary)
    );

    if (!hasValidData) {
      console.warn('No valid supplementary data found, proceeding with basic profile only');
    }

    const response = await fetch(`${API_BASE_URL}/User/register`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'User-Agent': 'Resume-Builder-Web-App/1.0',
      },
      body: JSON.stringify(testData),
      // Add timeout
      signal: AbortSignal.timeout(10000), // 10 second timeout
    });

    console.log('External API response status:', response.status);
    console.log('External API response headers:', Object.fromEntries(response.headers.entries()));

    // Read response text first
    const responseText = await response.text();
    console.log('External API response body:', responseText);

    if (!response.ok) {
      console.error('External API request failed:', {
        status: response.status,
        statusText: response.statusText,
        responseBody: responseText,
        requestData: testData // Log the actual data sent, not original
      });
      
      // Try to parse as JSON for better error details
      let errorDetails;
      try {
        errorDetails = JSON.parse(responseText);
      } catch {
        errorDetails = responseText || 'No error details provided';
      }

      // Provide more specific error messages based on status code
      let errorMessage = `External API error: ${response.status} ${response.statusText}`;
      
      switch (response.status) {
        case 400:
          errorMessage = 'Bad Request: The data provided is invalid or incomplete';
          break;
        case 401:
          errorMessage = 'Unauthorized: API authentication failed';
          break;
        case 403:
          errorMessage = 'Forbidden: Access denied to external API';
          break;
        case 404:
          errorMessage = 'Not Found: External API endpoint not found';
          break;
        case 409:
          errorMessage = 'Conflict: User may already exist with this email';
          break;
        case 422:
          errorMessage = 'Unprocessable Entity: Data validation failed on external API';
          break;
        case 500:
          errorMessage = 'Internal Server Error: External API encountered an error';
          break;
        case 502:
        case 503:
        case 504:
          errorMessage = 'Service Unavailable: External API is temporarily down';
          break;
      }
      
      return NextResponse.json(
        { 
          success: false, 
          message: errorMessage,
          details: errorDetails,
          originalData: userData, // Original data for debugging
          transformedData: testData, // Transformed data for debugging
          debugInfo: {
            hasValidEducation: transformedData.Education.length > 0,
            hasValidWorkExp: transformedData.WorkExperience.length > 0,
            hasValidSkills: transformedData.Skills.length > 0,
            hasValidProjects: transformedData.Projects.length > 0,
            hasValidSummary: isValidValue(transformedData.UserSummary)
          }
        },
        { status: response.status }
      );
    }

    // Parse successful response
    let result;
    try {
      result = JSON.parse(responseText);
    } catch (parseError) {
      console.error('Failed to parse successful response as JSON:', parseError);
      return NextResponse.json(
        { 
          success: false, 
          message: 'Invalid JSON response from external API',
          rawResponse: responseText
        },
        { status: 500 }
      );
    }
    
    console.log('External API successful response:', result);
    return NextResponse.json(result);
    
  } catch (error) {
    console.error('Error proxying request to external API:', error);
    return NextResponse.json(
      { 
        success: false, 
        message: 'Failed to connect to external API',
        error: error instanceof Error ? error.message : 'Unknown error'
      },
      { status: 500 }
    );
  }
}
