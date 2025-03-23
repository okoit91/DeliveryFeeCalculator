"use client";

import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import Link from "next/link";

// Define expected type for vehicle types
interface VehicleType {
  id: string;
  name: string;
}

export default function VehicleTypes() {
  const { userInfo, isAuthenticated } = useAuth();

  const { data: vehicleTypes, loading, error } = useFetchData<VehicleType[]>(
    "/api/v1.0/vehicleTypes",
    userInfo?.jwt ?? null
  );

  if (!isAuthenticated) {
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  }
  if (loading) {
    return <p className="text-gray-500 text-center">Loading vehicle types...</p>;
  }
  if (error) {
    return <p className="text-red-500 text-center">{error}</p>;
  }

  return (
    <div className="max-w-md mx-auto mt-10 bg-white p-6 rounded-lg shadow-lg">
      <h1 className="text-2xl font-bold text-gray-900 text-center mb-6">Vehicle Types</h1>

      <div className="text-center mb-4">
        <Link href="/pages/vehicle-types/create" className="text-blue-500 hover:underline">
          Create New Vehicle Type
        </Link>
      </div>

      <ul className="space-y-2">
        {Array.isArray(vehicleTypes) && vehicleTypes.length > 0 ? (
          vehicleTypes.map((vehicleType) => (
            <li key={vehicleType.id} className="flex justify-between items-center border-b py-2">
              <span className="text-gray-900 font-medium">{vehicleType.name}</span>
              <div className="space-x-4">
                <Link href={`/pages/vehicle-types/${vehicleType.id}`} className="text-green-500 hover:underline">
                  Details
                </Link>
                <Link href={`/pages/vehicle-types/${vehicleType.id}/edit`} className="text-blue-500 hover:underline">
                  Edit
                </Link>
                <Link href={`/pages/vehicle-types/${vehicleType.id}/delete`} className="text-red-500 hover:underline">
                  Delete
                </Link>
              </div>
            </li>
          ))
        ) : (
          <p className="text-gray-600 text-center">No vehicle types found.</p>
        )}
      </ul>
    </div>
  );
}
