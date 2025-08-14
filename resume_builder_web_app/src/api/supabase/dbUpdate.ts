import { UUID } from "crypto";
import supabase from "./connection";

export const usersUpdate = async (userId: UUID, updates: object) => {
  const { data, error } = await supabase
    .from("users")
    .update(updates)
    .eq("id", userId); // Changed from "user_id" to "id"

  if (error) {
    console.error("Error updating user data:", error);
    return null;
  }

  return data;
};

export const credentialsUserIdUpdate = async (email: string, updates: object) => {
  const { data, error } = await supabase
    .from("credentials")
    .update(updates)
    .ilike("email", `%${email}%`);

  if (error) {
    console.error("Error updating credentials:", error);
    return null;
  }

  return data;
};

export const dbUpdate = async (userId: UUID, table: string, updates: object[]) => {
  // Handle empty arrays
  if (!updates || updates.length === 0) {
    // Still delete existing records if updates is empty
    const { error: deleteError } = await supabase
      .from(table)
      .delete()
      .eq("user_id", userId);

    if (deleteError) {
      console.error("Error deleting data:", deleteError);
      return null;
    }

    return [];
  }

  const { data: deletedData, error: deleteError } = await supabase
    .from(table)
    .delete()
    .eq("user_id", userId);

  if (deleteError) {
    console.error("Error deleting data:", deleteError);
    return null;
  }

  // Add user_id to each record in the updates array
  const updatesWithUserId = updates.map(record => ({
    ...record,
    user_id: userId
  }));

  const { data, error } = await supabase
    .from(table)
    .insert(updatesWithUserId);

  if (error) {
    console.error("Error inserting data:", error);
    return null;
  }

  return data;
};
