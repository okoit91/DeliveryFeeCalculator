"use client";

import { useParams, useRouter } from "next/navigation";
import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import { useEditForm } from "@/hooks/useEditForm";
import Link from "next/link";

// Define the expected type for a Vehicle Type
interface VehicleType {
  id: string;
  name: string;
}

export default function EditVehicleType() {
  const { id } = useParams();
  const { userInfo, isAuthenticated } = useAuth();

  // Fetch the existing vehicle type details
  const { data, loading, error } = useFetchData<VehicleType>(
    `/api/v1.0/vehicleTypes/${id}`,
    userInfo?.jwt ?? null
  );

  // Initialize the edit form hook with fetched data or default values
  const {
    formValues,
    handleChange,
    handleSubmit,
    error: formError,
    loading: updating,
  } = useEditForm<VehicleType>(
    data ?? { id: "", name: "" },
    `/api/v1.0/vehicleTypes/${id}`,
    userInfo?.jwt ?? null,
    "/pages/vehicle-types" // Redirect URL after a successful update
  );

  if (!isAuthenticated)
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  if (loading)
    return <p className="text-gray-500 text-center">Loading vehicle type data...</p>;
  if (error)
    return <p className="text-red-500 text-center">{error}</p>;

  return (
    <div className="max-w-2xl mx-auto mt-10 bg-white p-6 rounded-lg shadow-lg">
      <h1 className="text-2xl font-bold text-gray-900 text-center mb-6">
        Edit Vehicle Type
      </h1>

      {formError && <p className="text-red-500 text-center mb-4">{formError}</p>}

      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block font-semibold text-gray-700">Vehicle Type Name</label>
          <input
            type="text"
            name="name"
            value={formValues.name}
            onChange={handleChange}
            required
            className="w-full p-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-300"
          />
        </div>

        <button
          type="submit"
          disabled={updating}
          className="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition disabled:opacity-50"
        >
          {updating ? "Updating..." : "Update Vehicle Type"}
        </button>
      </form>

      <div className="text-center mt-4">
        <Link href="/pages/vehicle-types" className="text-blue-500 hover:underline">
          Back to Vehicle Types List
        </Link>
      </div>
    </div>
  );
}
