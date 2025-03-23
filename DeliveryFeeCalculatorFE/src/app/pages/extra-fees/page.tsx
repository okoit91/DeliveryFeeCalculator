"use client";

import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import Link from "next/link";

// Define the expected type for an Extra Fee
interface ExtraFee {
  id: string;
  conditionType: string;
  feeAmount: number;
  vehicleType?: {
    id: string;
    name: string;
  };
  minValue?: number | null;
  maxValue?: number | null;
}

export default function ExtraFees() {
  const { userInfo, isAuthenticated } = useAuth();

  // Use the useFetchData hook to fetch extra fees using the provided JWT
  const { data: extraFees, loading, error } = useFetchData<ExtraFee[]>(
    "/api/v1.0/ExtraFees",
    userInfo?.jwt ?? null
  );

  if (!isAuthenticated) {
    return (
      <p className="text-red-500 text-center">Unauthorized: Please log in.</p>
    );
  }
  if (loading) {
    return <p className="text-gray-500 text-center">Loading extra fees...</p>;
  }
  if (error) {
    return <p className="text-red-500 text-center">{error}</p>;
  }

  return (
    <div className="max-w-2xl mx-auto mt-10 bg-white p-6 rounded-lg shadow-lg">
      <h1 className="text-2xl font-bold text-gray-900 text-center mb-6">
        Extra Fees
      </h1>

      <div className="text-center mb-4">
        <Link
          href="/pages/extra-fees/create"
          className="text-blue-500 hover:underline"
        >
          Create New Extra Fee
        </Link>
      </div>

      <ul className="space-y-2">
        {Array.isArray(extraFees) && extraFees.length > 0 ? (
          extraFees.map((fee) => (
            <li
              key={fee.id}
              className="flex justify-between items-center border-b py-2"
            >
              <span className="text-gray-800">
                <strong>{fee.vehicleType?.name ?? "Unknown Vehicle"}</strong> -{" "}
                {fee.conditionType} - ${fee.feeAmount.toFixed(2)}
                {fee.minValue !== null && ` | Min: ${fee.minValue}`}
                {fee.maxValue !== null && ` | Max: ${fee.maxValue}`}
              </span>
              <div className="space-x-4">
                <Link
                  href={`/pages/extra-fees/${fee.id}`}
                  className="text-green-500 hover:underline"
                >
                  Details
                </Link>
                <Link
                  href={`/pages/extra-fees/${fee.id}/edit`}
                  className="text-blue-500 hover:underline"
                >
                  Edit
                </Link>
                <Link
                  href={`/pages/extra-fees/${fee.id}/delete`}
                  className="text-red-500 hover:underline"
                >
                  Delete
                </Link>
              </div>
            </li>
          ))
        ) : (
          <p className="text-gray-600 text-center">No extra fees found.</p>
        )}
      </ul>
    </div>
  );
}
