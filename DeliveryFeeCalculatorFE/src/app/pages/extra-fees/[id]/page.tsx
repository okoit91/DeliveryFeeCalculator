"use client";

import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import { useParams } from "next/navigation";
import Link from "next/link";

// Define the expected type for an Extra Fee
interface ExtraFee {
  id: string;
  conditionType: string;
  feeAmount: number;
  vehicleTypeId: string;
  vehicleType: {
    id: string;
    name: string;
  };
  minValue?: number | null;
  maxValue?: number | null;
}

export default function ExtraFeeDetails() {
  const { id } = useParams();
  const { userInfo, isAuthenticated } = useAuth();

  // Fetch extra fee details
  const { data: extraFee, loading, error } = useFetchData<ExtraFee>(
    `/api/v1.0/ExtraFees/${id}`,
    userInfo?.jwt ?? null
  );

  if (!isAuthenticated) {
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  }

  if (loading) {
    return <p className="text-gray-500 text-center">Loading extra fee details...</p>;
  }

  if (error) {
    return <p className="text-red-500 text-center">{error}</p>;
  }

  if (!extraFee) {
    return <p className="text-gray-500 text-center">Extra Fee not found.</p>;
  }

  return (
    <div className="max-w-lg mx-auto p-6 bg-white shadow-md rounded-md mt-10">
      <h1 className="text-2xl font-bold mb-6 text-center">Extra Fee Details</h1>
      <div className="mb-4">
        <p className="text-gray-900">
          <strong>Vehicle Type:</strong> {extraFee.vehicleType?.name ?? "N/A"}
        </p>
        <p className="text-gray-900">
          <strong>Condition Type:</strong> {extraFee.conditionType}
        </p>
        <p className="text-gray-900">
          <strong>Fee Amount ($):</strong> {extraFee.feeAmount.toFixed(2)}
        </p>
        {extraFee.minValue !== null && (
          <p className="text-gray-900">
            <strong>Min Value:</strong> {extraFee.minValue}
          </p>
        )}
        {extraFee.maxValue !== null && (
          <p className="text-gray-900">
            <strong>Max Value:</strong> {extraFee.maxValue}
          </p>
        )}
      </div>
      <div className="text-center">
        <Link href="/pages/extra-fees" className="text-blue-500 hover:underline">
          Back to Extra Fees List
        </Link>
      </div>
    </div>
  );
}
