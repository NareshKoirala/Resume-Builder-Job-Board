import supabase from "./connection";

export const emailFetch = async (email: string) : Promise<any> => {
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
