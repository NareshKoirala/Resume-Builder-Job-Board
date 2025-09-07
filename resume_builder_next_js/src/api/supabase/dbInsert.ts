import supabase from "./connection";

interface UserCredentials {
  email: string;
  password_hash: string;
}

// Insert user credentials with hashed password
  export const dbCredentialsInsert = async (userId: string, credentials: UserCredentials) => {
  const user_credentials = {
    email: credentials.email,
    password_hash: credentials.password_hash,
    public_id: userId
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
