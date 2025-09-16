import { createClient } from "@supabase/supabase-js";


const supabaseUrl = process.env.NEXT_PUBLIC_SUPABASE_URL ?? "";
const supabaseKey = process.env.NEXT_PUBLIC_SUPABASE_ANON_KEY ?? "";

// Check if environment variables are properly set
if (!supabaseUrl || !supabaseKey) {
  console.error("Supabase environment variables are not set. Please check your .env file.");
  console.error("Required variables: NEXT_PUBLIC_SUPABASE_URL, NEXT_PUBLIC_SUPABASE_KEY");
}

const supabase = createClient(supabaseUrl, supabaseKey);

export default supabase;
