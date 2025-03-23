"use client";

import { useParams, useRouter } from "next/navigation";
import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import { useDelete } from "@/hooks/useDelete";
import Link from "next/link";

// Define the Station interface
interface Station {
  id: string;
  stationName: string;
}

export default function DeleteStation() {
  const { id } = useParams();
  const router = useRouter();
  const { userInfo, isAuthenticated } = useAuth();

  // Fetch station details
  const { data: station, loading, error } = useFetchData<Station>(
    `/api/v1.0/stations/${id}`,
    userInfo?.jwt ?? null
  );

  // Use the delete hook to handle station deletion
  const { handleDelete, deleting, deleteError } = useDelete(
    `/api/v1.0/stations/${id}`,
    userInfo?.jwt ?? null,
    "/pages/stations" // Redirect after deletion
  );

  if (!isAuthenticated) {
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  }

  if (loading) {
    return <p className="text-gray-500 text-center">Loading station data...</p>;
  }

  if (error) {
    return <p className="text-red-500 text-center">{error}</p>;
  }

  return (
    <div className="max-w-md mx-auto mt-10 bg-white p-6 rounded-lg shadow-lg">
      <h1 className="text-2xl font-bold text-gray-900 text-center mb-6">
        Delete Station
      </h1>

      {deleteError && <p className="text-red-500 text-center mb-4">{deleteError}</p>}

      <p className="text-center text-gray-900 mb-6">
        Are you sure you want to delete the station{" "}
        <strong>{station?.stationName ?? "Unknown Station"}</strong>?
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
          href="/pages/stations"
          className="bg-gray-300 px-4 py-2 rounded-md hover:bg-gray-400 transition text-black"
        >
          Cancel
        </Link>
      </div>
    </div>
  );
}
