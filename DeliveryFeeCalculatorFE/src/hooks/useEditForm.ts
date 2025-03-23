import { useState, useEffect, useRef, ChangeEvent, FormEvent } from "react";
import AccountService from "@/services/AccountService";
import { useRouter } from "next/navigation";

export function useEditForm<T>(
  initialValues: T,
  apiUrl: string,
  jwt: string | null,
  redirectUrl?: string
) {
  const [formValues, setFormValues] = useState<T>(initialValues);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const router = useRouter();

  const prevInitialValuesRef = useRef<string>(JSON.stringify(initialValues));

  useEffect(() => {
    const currentInitialValuesStr = JSON.stringify(initialValues);
    if (prevInitialValuesRef.current !== currentInitialValuesStr) {
      prevInitialValuesRef.current = currentInitialValuesStr;
      setFormValues(initialValues);
    }
  }, [initialValues]);

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    setFormValues((prev) => ({
      ...prev,
      [e.target.name]: e.target.value,
    }));
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);
    try {
      await AccountService.request("PUT", apiUrl, formValues, jwt);
      if (redirectUrl) router.push(redirectUrl);
    } catch (err: any) {
      console.error("Edit form error:", err);
      setError("Failed to update data. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return { formValues, setFormValues, handleChange, handleSubmit, error, loading };
}
