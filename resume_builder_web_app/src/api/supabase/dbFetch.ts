import { UUID } from "crypto";
import supabase from "./connection";

export const emailFetch = async (email: string) => {
  const { data, error } = await supabase
    .from("credentials")
    .select("*")
    .ilike("email", `%${email}%`);

  if (error) {
    console.error("Error fetching data:", error);
    return null;
  }

  return data;
};

export const userFetch = async (userId: string) => {
  const { data, error } = await supabase
    .from("users")
    .select("*")
    .eq("id", userId);

  if (error) {
    console.error("Error fetching data:", error);
    return null;
  }

  return data;
};

export const dbFetch = async (userId: UUID, table: string) => {
  const { data, error } = await supabase
    .from(table)
    .select("*")
    .eq("user_id", userId);

  if (error) {
    console.error("Error fetching data:", error);
    return null;
  }

  return data;
};
