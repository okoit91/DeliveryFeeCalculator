import { useState, useEffect, useRef } from "react";
import AccountService from "@/services/AccountService";

export function useFetchData<T>(url: string, jwt: string | null) {
  const [data, setData] = useState<T | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const fetchedRef = useRef(false);

  useEffect(() => {
    fetchedRef.current = false;
  }, [url, jwt]);

  useEffect(() => {
    if (!jwt) {
      setError("Unauthorized");
      setLoading(false);
      return;
    }
    
    if (fetchedRef.current) return;
    fetchedRef.current = true;
    AccountService.request<T>("GET", url, undefined, jwt)
      .then((responseData) => setData(responseData))
      .catch((err) => {
        console.error("Error fetching data:", err);
        setError("Failed to fetch data.");
      })
      .finally(() => setLoading(false));
  }, [url, jwt]);

  return { data, error, loading };
}
