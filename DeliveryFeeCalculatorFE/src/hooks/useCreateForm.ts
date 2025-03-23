import { useState, ChangeEvent, FormEvent } from "react";
import AccountService from "@/services/AccountService";
import { useRouter } from "next/navigation";

export function useCreateForm<T>(
  initialValues: T,
  apiUrl: string,
  jwt: string | null,
  redirectUrl?: string
) {
  const [formValues, setFormValues] = useState<T>(initialValues);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const router = useRouter();

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormValues((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);
    try {
      await AccountService.request("POST", apiUrl, formValues, jwt);
      if (redirectUrl) router.push(redirectUrl);
    } catch (err: any) {
      console.error("Create form error:", err);
      setError("Failed to create data. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return { formValues, setFormValues, handleChange, handleSubmit, error, loading };
}
