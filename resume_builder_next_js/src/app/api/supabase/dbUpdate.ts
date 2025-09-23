import { UUID } from "crypto";
import supabase from "./connection";


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
  console.log(`dbUpdate called for table: ${table}, userId: ${userId}, updates:`, updates);
  
  // Handle empty arrays
  if (!updates || updates.length === 0) {
    console.log(`No updates provided for ${table}, deleting existing records`);
    // Still delete existing records if updates is empty
    const { error: deleteError } = await supabase
      .from(table)
      .delete()
      .eq("user_id", userId);

    if (deleteError) {
      console.error(`Error deleting data from ${table}:`, deleteError);
      return null;
    }

    console.log(`Successfully cleared ${table} for user ${userId}`);
    return [];
  }

  console.log(`Deleting existing records from ${table} for user ${userId}`);
  const { data: deletedData, error: deleteError } = await supabase
    .from(table)
    .delete()
    .eq("user_id", userId);

  if (deleteError) {
    console.error(`Error deleting data from ${table}:`, deleteError);
    return null;
  }

  // Add user_id to each record in the updates array
  const updatesWithUserId = updates.map((record, index) => {
    // Remove any 'id' field if it exists to let the database auto-generate it
    const { id, ...recordWithoutId } = record as any;
    if (id) {
      console.log(`Removing id field from ${table} record ${index}:`, id);
    }
    const finalRecord = {
      ...recordWithoutId,
      user_id: userId
    };
    console.log(`Processed ${table} record ${index}:`, finalRecord);
    return finalRecord;
  });

  console.log(`Inserting new records into ${table}:`, updatesWithUserId);
  const { data, error } = await supabase
    .from(table)
    .insert(updatesWithUserId)
    .select(); // Add select to return the inserted data

  if (error) {
    console.error(`Error inserting data into ${table}:`, error);
    return null;
  }

  console.log(`Successfully updated ${table} for user ${userId}:`, data);
  return data;
};
