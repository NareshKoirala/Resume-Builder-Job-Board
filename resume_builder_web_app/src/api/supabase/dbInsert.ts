import { UUID } from "crypto";
import supabase from "./connection";

interface UserCredentials {
  email: string;
  password_hash: string;
}

// Insert a single user record
export const userInsert = async (userData: object) => {
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