"use client";

import { useParams } from "next/navigation";
import { useFetchData } from "@/hooks/useFetchData";
import { useAuth } from "@/hooks/useAuth";
import Link from "next/link";

// Define the expected type for a Vehicle Type
interface VehicleType {
  id: string;
  name: string;
  // Add any additional fields as needed
}

export default function VehicleTypeDetails() {
  const { id } = useParams();
  const { userInfo, isAuthenticated } = useAuth();

  // Fetch vehicle type details using the vehicle type id from the URL and JWT from userInfo
  const { data, loading, error } = useFetchData<VehicleType>(
    `/api/v1.0/vehicleTypes/${id}`,
    userInfo?.jwt ?? null
  );

  if (!isAuthenticated)
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  if (loading)
    return <p className="text-gray-500 text-center">Loading vehicle type details...</p>;
  if (error)
    return <p className="text-red-500 text-center">{error}</p>;
  if (!data)
    return <p className="text-gray-500 text-center">Vehicle type not found.</p>;

  return (
    <div className="max-w-md mx-auto p-6 bg-white shadow-md rounded-md">
      <h1 className="text-2xl font-bold mb-4 text-center">Vehicle Type Details</h1>
      <div className="mb-4">
        <p className="text-gray-900">
          <strong>Name:</strong> {data.name}
        </p>
        {/* Add more details about the vehicle type here as needed */}
      </div>
      <div className="text-center">
        <Link href="/pages/vehicle-types" className="text-blue-500 hover:underline">
          Back to Vehicle Types List
        </Link>
      </div>
    </div>
  );
}
