"use client";

import { useParams, useRouter } from "next/navigation";
import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import { useDelete } from "@/hooks/useDelete";
import Link from "next/link";

// Define the expected type for an Extra Fee
interface ExtraFee {
  id: string;
  conditionType: string;
  feeAmount: number;
  vehicleTypeName: string;
  minValue?: number | null;
  maxValue?: number | null;
}

export default function DeleteExtraFee() {
  const { id } = useParams();
  const { userInfo, isAuthenticated } = useAuth();
  const router = useRouter();

  // Fetch extra fee details using the provided JWT
  const { data: extraFee, loading, error } = useFetchData<ExtraFee>(
    `/api/v1.0/ExtraFees/${id}`,
    userInfo?.jwt ?? null
  );

  // Use the delete hook for handling deletion
  const { handleDelete, deleting, deleteError } = useDelete(
    `/api/v1.0/ExtraFees/${id}`,
    userInfo?.jwt ?? null,
    "/pages/extra-fees" // Redirect URL after successful deletion
  );

  if (!isAuthenticated)
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  if (loading)
    return <p className="text-gray-500 text-center">Loading extra fee data...</p>;
  if (error)
    return <p className="text-red-500 text-center">{error}</p>;

  return (
    <div className="max-w-md mx-auto mt-10 bg-white p-6 rounded-lg shadow-lg">
      <h1 className="text-2xl font-bold text-gray-900 text-center mb-6">Delete Extra Fee</h1>

      {deleteError && <p className="text-red-500 text-center mb-4">{deleteError}</p>}

      <p className="text-center text-gray-900 mb-6">
        Are you sure you want to delete the extra fee for{" "}
        <strong>{extraFee?.vehicleTypeName}</strong> with condition{" "}
        <strong>{extraFee?.conditionType}</strong> and fee amount{" "}
        <strong>${extraFee?.feeAmount.toFixed(2)}</strong>?
      </p>

      <div className="flex justify-center space-x-4">
        <button
          onClick={handleDelete}
          disabled={deleting}
          className="bg-red-500 text-white px-4 py-2 rounded-md hover:bg-red-600 transition disabled:opacity-50"
        >
          {deleting ? "Deleting..." : "Delete"}
        </button>

        <Link
          href="/pages/extra-fees"
          className="bg-gray-300 px-4 py-2 rounded-md hover:bg-gray-400 transition text-black"
        >
          Cancel
        </Link>
      </div>
    </div>
  );
}
