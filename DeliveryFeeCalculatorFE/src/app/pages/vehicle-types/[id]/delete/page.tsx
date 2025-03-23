"use client";

import { useParams, useRouter } from "next/navigation";
import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import { useDelete } from "@/hooks/useDelete";
import Link from "next/link";

interface VehicleType {
  id: string;
  name: string;
}

export default function DeleteVehicleType() {
  const { id } = useParams();
  const { userInfo, isAuthenticated } = useAuth();
  const router = useRouter();

  const { data: vehicleType, loading, error } = useFetchData<VehicleType>(
    `/api/v1.0/vehicleTypes/${id}`,
    userInfo?.jwt ?? null
  );

  const { handleDelete, deleting, deleteError } = useDelete(
    `/api/v1.0/vehicleTypes/${id}`,
    userInfo?.jwt ?? null,
    "/pages/vehicle-types"
  );

  if (!isAuthenticated)
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  if (loading)
    return <p className="text-gray-500 text-center">Loading vehicle type data...</p>;
  if (error)
    return <p className="text-red-500 text-center">{error}</p>;

  return (
    <div className="max-w-md mx-auto mt-10 bg-white p-6 rounded-lg shadow-lg">
      <h1 className="text-2xl font-bold text-gray-900 text-center mb-6">
        Delete Vehicle Type
      </h1>

      {/* Error Message */}
      {deleteError && <p className="text-red-500 text-center mb-4">{deleteError}</p>}

      <p className="text-center text-gray-700 mb-6">
        Are you sure you want to delete the vehicle type:{" "}
        <strong>{vehicleType?.name}</strong>?
      </p>

      {/* Buttons */}
      <div className="flex justify-center space-x-4">
        <button
          onClick={handleDelete}
          disabled={deleting}
          className="bg-red-500 text-white px-4 py-2 rounded-md hover:bg-red-600 transition disabled:opacity-50"
        >
          {deleting ? "Deleting..." : "Delete"}
        </button>

        <Link
          href="/pages/vehicle-types"
          className="bg-gray-300 px-4 py-2 rounded-md hover:bg-gray-400 transition text-black"
        >
          Cancel
        </Link>
      </div>
    </div>
  );
}
