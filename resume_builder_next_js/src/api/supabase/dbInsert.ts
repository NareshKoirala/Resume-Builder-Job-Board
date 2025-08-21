import { UUID } from "crypto";
import supabase from "./connection";
import { UserRegisterDto } from "@/model/data-structure";

interface UserCredentials {
  email: string;
  password_hash: string;
}

// Comprehensive user registration function
export const userRegisterComplete = async (userData: UserRegisterDto): Promise<any> => {
  try {
    // Extract user data for the users table (excluding arrays and password)
    const { pass, education, work_experience, certificates, skills, projects, ...userTableData } = userData;
    
    // Insert user into users table
    const { data: userResult, error: userError } = await supabase
      .from("users")
      .insert(userTableData)
      .select()
      .single();

    if (userError) {
      console.error("Error inserting user:", userError);
      return null;
    }

    const userId = userResult.id;

    // Insert related data in parallel
    const insertPromises = [];

    // Insert education records
    if (education && education.length > 0) {
      const educationRecords = education.map(edu => ({ ...edu, user_id: userId }));
      insertPromises.push(
        supabase.from("education").insert(educationRecords)
      );
    }

    // Insert work experience records
    if (work_experience && work_experience.length > 0) {
      const workRecords = work_experience.map(work => ({ ...work, user_id: userId }));
      insertPromises.push(
        supabase.from("work_experience").insert(workRecords)
      );
    }

    // Insert certificate records
    if (certificates && certificates.length > 0) {
      const certRecords = certificates.map(cert => ({ ...cert, user_id: userId }));
      insertPromises.push(
        supabase.from("certificates").insert(certRecords)
      );
    }

    // Insert skill records
    if (skills && skills.length > 0) {
      const skillRecords = skills.map(skill => ({ ...skill, user_id: userId }));
      insertPromises.push(
        supabase.from("skills").insert(skillRecords)
      );
    }

    // Insert project records
    if (projects && projects.length > 0) {
      const projectRecords = projects.map(project => ({ ...project, user_id: userId }));
      insertPromises.push(
        supabase.from("projects").insert(projectRecords)
      );
    }

    // Wait for all inserts to complete
    if (insertPromises.length > 0) {
      const results = await Promise.allSettled(insertPromises);
      
      // Check for any failed inserts
      const failures = results.filter(result => result.status === 'rejected');
      if (failures.length > 0) {
        console.warn("Some related data inserts failed:", failures);
      }
    }

    return [userResult]; // Return in array format to match existing expectation
  } catch (error) {
    console.error("Error during complete user registration:", error);
    return null;
  }
};

// Insert a single user record (legacy function)
export const userInsert = async (userData: object): Promise<UserRegisterDto | null | any> => {
  const { data, error } = await supabase
    .from("users")
    .insert(userData)
    .select(); // Return the inserted data

  if (error) {
    console.error("Error inserting user data:", error);
    return null;
  }

  return data;
};

// Insert user credentials with hashed password
  export const dbCredentialsInsert = async (userId: UUID, credentials: UserCredentials) => {
  const user_credentials = {
    email: credentials.email,
    password_hash: credentials.password_hash,
    user_id: userId
  };

  const { data, error } = await supabase
    .from("credentials")
    .insert(user_credentials)
    .select(); // Return the inserted data

  if (error) {
    console.error("Error inserting user credentials:", error);
    return null;
  }

  return data;
};

// Insert multiple records into any table with user_id
export const dbInsert = async (userId: UUID, table: string, records: object[]) => {
  // Handle empty arrays
  if (!records || records.length === 0) {
    return [];
  }

  // Add user_id to each record
  const recordsWithUserId = records.map(record => ({
    ...record,
    user_id: userId
  }));

  const { data, error } = await supabase
    .from(table)
    .insert(recordsWithUserId)
    .select(); // Return the inserted data

  if (error) {
    console.error(`Error inserting data into ${table}:`, error);
    return null;
  }

  return data;
};

// Insert a single record into any table with user_id
export const dbInsertSingle = async (userId: UUID, table: string, record: object) => {
  const recordWithUserId = {
    ...record,
    user_id: userId
  };

  const { data, error } = await supabase
    .from(table)
    .insert(recordWithUserId)
    .select(); // Return the inserted data

  if (error) {
    console.error(`Error inserting single record into ${table}:`, error);
    return null;
  }

  return data;
};