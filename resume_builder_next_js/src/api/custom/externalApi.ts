import { UserRegisterDto } from '@/model/data-structure';
import { userRegisterComplete as localUserRegisterComplete } from '@/api/supabase/dbInsert';

interface ApiResponse {
  success: boolean;
  message: string;
  publicId?: string;
}

/**
 * Register a new user with the external API via Next.js API proxy
 * @param userData - The user registration data
 * @returns Promise<ApiResponse | null>
 */
export const registerUserWithExternalApi = async (userData: UserRegisterDto): Promise<ApiResponse | null> => {
  try {
    console.log('Registering user with external API via proxy:', { email: userData.email });
    
    const response = await fetch('/api/external/register', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(userData),
    });

    console.log('External API proxy response status:', response.status);

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({ message: 'Unknown error' }));
      console.error('API proxy request failed:', {
        status: response.status,
        statusText: response.statusText,
        errorData: errorData
      });
      return null;
    }

    const result: ApiResponse = await response.json();
    console.log('External API proxy response:', result);
    return result;
  } catch (error) {
    console.error('Error calling external API via proxy:', error);
    return null;
  }
};

/**
 * Register user with external API and then update local database
 * @param userData - The user registration data
 * @returns Promise<{success: boolean, publicId?: string, localUserId?: string, error?: string}>
 */
export const registerUserComplete = async (userData: UserRegisterDto): Promise<{success: boolean, publicId?: string, localUserId?: string, error?: string}> => {
  try {
    console.log('Starting complete user registration process for:', userData.email);
    console.log('User data being sent:', JSON.stringify(userData, null, 2));
    
    // Step 1: Try to register with external API
    const apiResponse = await registerUserWithExternalApi(userData);
    
    let publicId: string;
    
    if (!apiResponse || !apiResponse.success) {
      // If external API fails, generate a temporary publicId for now
      console.warn('External API registration failed, proceeding with local registration only');
      console.warn('External API error:', apiResponse?.message || 'Unknown error');
      
      // Generate a temporary public ID
      publicId = `temp_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`;
      console.log('Generated temporary publicId:', publicId);
    } else {
      publicId = apiResponse.publicId!;
      console.log('External API registration successful, publicId:', publicId);
    }

    // Step 2: Register in local database using the existing function
    console.log('Starting local database registration...');
    const localDbResponse = await localUserRegisterComplete(userData);
    
    if (!localDbResponse || localDbResponse.length === 0) {
      console.error('Local database registration failed');
      return {
        success: false,
        error: 'Failed to register in local database'
      };
    }

    const localUserId = localDbResponse[0].id;
    console.log('Local database registration successful, localUserId:', localUserId);
    
    return {
      success: true,
      publicId: publicId,
      localUserId: localUserId
    };
  } catch (error) {
    console.error('Error in complete registration process:', error);
    return {
      success: false,
      error: error instanceof Error ? error.message : 'Unknown error occurred'
    };
  }
};
