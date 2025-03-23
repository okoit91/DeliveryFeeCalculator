"use client";

import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import { useParams } from "next/navigation";
import Link from "next/link";

// Define the expected type for a Standard Fee
interface StandardFee {
  id: string;
  cityId: string;
  vehicleTypeId: string;
  city: {
    id: string;
    name: string;
  };
  vehicleType: {
    id: string;
    name: string;
  };
  feeAmount: number;
}

export default function StandardFeeDetails() {
  const { id } = useParams();
  const { userInfo, isAuthenticated } = useAuth();

  // Fetch standard fee details
  const { data: standardFee, loading, error } = useFetchData<StandardFee>(
    `/api/v1.0/StandardFees/${id}`,
    userInfo?.jwt ?? null
  );

  if (!isAuthenticated) {
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  }

  if (loading) {
    return <p className="text-gray-500 text-center">Loading standard fee details...</p>;
  }

  if (error) {
    return <p className="text-red-500 text-center">{error}</p>;
  }

  if (!standardFee) {
    return <p className="text-gray-500 text-center">Standard Fee not found.</p>;
  }

  return (
    <div className="max-w-lg mx-auto p-6 bg-white shadow-md rounded-md mt-10">
      <h1 className="text-2xl font-bold mb-6 text-center">Standard Fee Details</h1>
      <div className="mb-4">
        <p className="text-gray-900">
          <strong>City:</strong> {standardFee.city?.name ?? "N/A"}
        </p>
        <p className="text-gray-900">
          <strong>Vehicle Type:</strong> {standardFee.vehicleType?.name ?? "N/A"}
        </p>
        <p className="text-gray-900">
          <strong>Fee Amount ($):</strong> {standardFee.feeAmount.toFixed(2)}
        </p>
      </div>
      <div className="text-center">
        <Link href="/pages/standard-fees" className="text-blue-500 hover:underline">
          Back to Standard Fees List
        </Link>
      </div>
    </div>
  );
}
