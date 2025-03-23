"use client";

import { useAuth } from "@/hooks/useAuth";
import { useCreateForm } from "@/hooks/useCreateForm";
import Link from "next/link";

interface VehicleTypeFormData {
  name: string;
}

export default function CreateVehicleType() {
  const { userInfo, isAuthenticated } = useAuth();

  const {
    formValues,
    handleChange,
    handleSubmit,
    error,
    loading,
  } = useCreateForm<VehicleTypeFormData>(
    { name: "" },
    "/api/v1.0/vehicleTypes",
    userInfo?.jwt ?? null,
    "/pages/vehicle-types"
  );

  if (!isAuthenticated) {
    return (
      <p className="text-red-500 text-center">Unauthorized: Please log in.</p>
    );
  }

  return (
    <div className="max-w-2xl mx-auto mt-10 bg-white p-6 rounded-lg shadow-lg">
      <h1 className="text-2xl font-bold text-gray-900 text-center mb-6">
        Create Vehicle Type
      </h1>

      {/* Error Message */}
      {error && <p className="text-red-500 text-center mb-4">{error}</p>}

      {/* Create Vehicle Type Form */}
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block font-semibold text-gray-700">
            Vehicle Type Name
          </label>
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
          disabled={loading}
          className="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition disabled:opacity-50"
        >
          {loading ? "Creating..." : "Create Vehicle Type"}
        </button>
      </form>

      {/* Back to Vehicle Types List */}
      <div className="text-center mt-4">
        <Link href="/pages/vehicle-types" className="text-blue-500 hover:underline">
          Back to Vehicle Types List
        </Link>
      </div>
    </div>
  );
}
