import { NextRequest, NextResponse } from 'next/server';
import bcrypt from 'bcrypt';
import { emailFetch } from '@/api/supabase/dbFetch';

export async function POST(request: NextRequest) {
  try {
    const { email, password } = await request.json();
    
    // Validate input
    if (!email || !password) {
      return NextResponse.json(
        { success: false, message: 'Email and password are required' },
        { status: 400 }
      );
    }
    
    // Check if user exists
    const userResult = await emailFetch(email);
    if (!userResult || userResult.length === 0) {
      return NextResponse.json(
        { success: false, message: 'Invalid email or password' },
        { status: 401 }
      );
    }
    
    const user = userResult[0];
    
    // Verify password
    const isPasswordValid = await bcrypt.compare(password, user.password_hash);
    if (!isPasswordValid) {
      return NextResponse.json(
        { success: false, message: 'Invalid email or password' },
        { status: 401 }
      );
    }
    
    // Success - In a real app, you might want to create a JWT token here
    return NextResponse.json({ 
      success: true, 
      message: 'Signed in successfully',
      user: {
        id: user.id,
        email: user.email
      }
    });
    
  } catch (error) {
    console.error('Sign-in error:', error);
    return NextResponse.json(
      { success: false, message: 'Internal server error' },
      { status: 500 }
    );
  }
}
