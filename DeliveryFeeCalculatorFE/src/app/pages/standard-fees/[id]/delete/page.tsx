"use client";

import { useParams, useRouter } from "next/navigation";
import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import { useDelete } from "@/hooks/useDelete";
import Link from "next/link";

interface City {
  id: string;
  name: string;
}

interface VehicleType {
  id: string;
  name: string;
}

interface StandardFee {
  id: string;
  cityId: string;
  city: City | null;
  vehicleTypeId: string;
  vehicleType: VehicleType | null;
  feeAmount: number;
}

export default function DeleteStandardFee() {
  const { id } = useParams();
  const router = useRouter();
  const { userInfo, isAuthenticated } = useAuth();

  // Fetch the standard fee details using the provided JWT.
  const { data: fee, loading, error } = useFetchData<StandardFee>(
    `/api/v1.0/standardFees/${id}`,
    userInfo?.jwt ?? null
  );

  // Use the delete hook to handle deletion.
  const { handleDelete, deleting, deleteError } = useDelete(
    `/api/v1.0/standardFees/${id}`,
    userInfo?.jwt ?? null,
    "/pages/standard-fees" // Redirect URL after successful deletion.
  );

  if (!isAuthenticated)
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  if (loading)
    return <p className="text-gray-500 text-center">Loading standard fee data...</p>;
  if (error)
    return <p className="text-red-500 text-center">{error}</p>;

  return (
    <div className="max-w-md mx-auto mt-10 bg-white p-6 rounded-lg shadow-lg">
      <h1 className="text-2xl font-bold text-gray-900 text-center mb-6">
        Delete Standard Fee
      </h1>

      {deleteError && <p className="text-red-500 text-center mb-4">{deleteError}</p>}

      <p className="text-center text-gray-900 mb-6">
        Are you sure you want to delete the standard fee for{" "}
        <strong>{fee?.city?.name ?? "Unknown City"}</strong> -{" "}
        <strong>{fee?.vehicleType?.name ?? "Unknown Vehicle Type"}</strong> (Amount: $
        {fee?.feeAmount !== undefined ? fee.feeAmount.toFixed(2) : "N/A"})?
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
          href="/pages/standard-fees"
          className="bg-gray-300 px-4 py-2 rounded-md hover:bg-gray-400 transition text-black"
        >
          Cancel
        </Link>
      </div>
    </div>
  );
}
