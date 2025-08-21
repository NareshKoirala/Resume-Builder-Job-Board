import { UUID } from "crypto";
import supabase from "./connection";

// Delete a user by their ID (will cascade delete related records if DB is set up for it)
export const userDelete = async (userId: UUID) => {
  const { data, error } = await supabase
    .from("users")
    .delete()
    .eq("id", userId)
    .select(); // Return the deleted data

  if (error) {
    console.error("Error deleting user:", error);
    return null;
  }

  return data;
};

// Delete all records from a table for a specific user
export const dbDeleteAllForUser = async (userId: UUID, table: string) => {
  const { data, error } = await supabase
    .from(table)
    .delete()
    .eq("user_id", userId)
    .select(); // Return the deleted data

  if (error) {
    console.error(`Error deleting all records from ${table} for user:`, error);
    return null;
  }

  return data;
};

// Delete a specific record by its ID
export const dbDeleteById = async (table: string, recordId: string | number) => {
  const { data, error } = await supabase
    .from(table)
    .delete()
    .eq("id", recordId)
    .select(); // Return the deleted data

  if (error) {
    console.error(`Error deleting record from ${table}:`, error);
    return null;
  }

  return data;
};

// Delete specific records by IDs for a user (extra safety check)
export const dbDeleteByIds = async (userId: UUID, table: string, recordIds: (string | number)[]) => {
  if (!recordIds || recordIds.length === 0) {
    return [];
  }

  const { data, error } = await supabase
    .from(table)
    .delete()
    .eq("user_id", userId) // Safety: ensure it belongs to this user
    .in("id", recordIds)
    .select(); // Return the deleted data

  if (error) {
    console.error(`Error deleting records from ${table}:`, error);
    return null;
  }

  return data;
};

// Delete records matching specific conditions
export const dbDeleteWhere = async (userId: UUID, table: string, conditions: Record<string, any>) => {
  let query = supabase
    .from(table)
    .delete()
    .eq("user_id", userId); // Always filter by user for safety

  // Apply additional conditions
  Object.entries(conditions).forEach(([key, value]) => {
    query = query.eq(key, value);
  });

  const { data, error } = await query.select();

  if (error) {
    console.error(`Error deleting records from ${table} with conditions:`, error);
    return null;
  }

  return data;
};