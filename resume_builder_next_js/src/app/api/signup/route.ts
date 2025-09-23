import { NextRequest, NextResponse } from 'next/server';
import bcrypt from 'bcrypt';
import { dbCredentialsInsert } from '../supabase/dbInsert';
import { emailFetch } from '../supabase/dbFetch';

export async function POST(request: NextRequest) {
  try {
    const { email, password } = await request.json();

    // Get publicId from cookies
    const reqPublicId = request.cookies.get('publicId')?.value;
    console.log("Public ID from cookie:", reqPublicId);

    // Validate input
    if (!email || !password || !reqPublicId) {
      return NextResponse.json(
        { success: false, message: 'Email, password, and Public ID are required' },
        { status: 400 }
      );
    }

    // Check if email already exists
    const existingUser = await emailFetch(email);
    if (existingUser && existingUser.length > 0) {
      return NextResponse.json(
        { success: false, message: 'Email already exists' },
        { status: 400 }
      );
    }
    
    // Hash the password
    const saltRounds = 10;
    const hashedPassword = await bcrypt.hash(password, saltRounds);
    
    // Insert into database
    const result = await dbCredentialsInsert(reqPublicId, {
      email,
      password_hash: hashedPassword
    });

    if (!result) {
      throw new Error('Failed to create account');
    }

    return NextResponse.json({ success: true, message: 'Account created successfully' });
  } catch (error) {
    return NextResponse.json(
      { success: false, message: 'Failed to create account' },
      { status: 500 }
    );
  }
}
    