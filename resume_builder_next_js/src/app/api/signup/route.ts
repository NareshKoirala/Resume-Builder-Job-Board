import { NextRequest, NextResponse } from 'next/server';
import bcrypt from 'bcrypt';
import { dbCredentialsInsert } from '@/api/supabase/dbInsert';
import { emailFetch } from '@/api/supabase/dbFetch';
import { randomUUID } from 'crypto';

export async function POST(request: NextRequest) {
  try {
    const { email, password } = await request.json();
    
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
    const result = await dbCredentialsInsert(randomUUID(), {
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
    