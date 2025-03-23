import { useState } from "react";
import { useRouter } from "next/navigation";
import AccountService from "@/services/AccountService";

/**
 * @param url The API endpoint to delete the resource.
 * @param token JWT authentication token (nullable).
 * @param redirectUrl URL to redirect after successful deletion.
 */

export function useDelete(url: string, token: string | null, redirectUrl?: string) {
  const [deleting, setDeleting] = useState(false);
  const [deleteError, setDeleteError] = useState<string | null>(null);
  const router = useRouter();

  const handleDelete = async () => {
    setDeleteError(null);
    setDeleting(true);

    if (!token) {
      setDeleteError("Unauthorized: Please log in.");
      setDeleting(false);
      return;
    }

    try {
      await AccountService.request("DELETE", url, undefined, token);
      if (redirectUrl) router.push(redirectUrl);
    } catch (err: any) {
      console.error("Delete request failed:", err);
      setDeleteError("Failed to delete. Please try again.");
    } finally {
      setDeleting(false);
    }
  };

  return { handleDelete, deleting, deleteError };
}
