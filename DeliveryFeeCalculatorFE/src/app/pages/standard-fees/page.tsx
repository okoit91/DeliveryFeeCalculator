"use client";

import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
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
  city?: City;
  vehicleTypeId: string;
  vehicleType?: VehicleType;
  feeAmount: number;
}

export default function StandardFees() {
  const { userInfo, isAuthenticated } = useAuth();

  const { data: standardFees, loading, error } = useFetchData<StandardFee[]>(
    "/api/v1.0/standardFees",
    userInfo?.jwt ?? null
  );

  if (!isAuthenticated)
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  if (loading)
    return <p className="text-gray-500 text-center">Loading standard fees...</p>;
  if (error)
    return <p className="text-red-500 text-center">{error}</p>;

  return (
    <div className="max-w-2xl mx-auto mt-10 bg-white p-6 rounded-lg shadow-lg">
      <h1 className="text-2xl font-bold text-gray-900 text-center mb-6">
        Standard Fees
      </h1>

      <div className="text-center mb-4">
        <Link href="/pages/standard-fees/create" className="text-blue-500 hover:underline">
          Create New Standard Fee
        </Link>
      </div>

      <ul className="space-y-2">
        {standardFees && standardFees.length > 0 ? (
          standardFees.map((fee) => (
            <li key={fee.id} className="flex justify-between items-center border-b py-2">
              <span className="text-gray-900">
                {fee.city?.name ?? "No City"} - {fee.vehicleType?.name ?? "No Vehicle Type"} - ${fee.feeAmount.toFixed(2)}
              </span>
              <div className="space-x-4">
                <Link href={`/pages/standard-fees/${fee.id}`} className="text-green-500 hover:underline">
                  Details
                </Link>
                <Link href={`/pages/standard-fees/${fee.id}/edit`} className="text-blue-500 hover:underline">
                  Edit
                </Link>
                <Link href={`/pages/standard-fees/${fee.id}/delete`} className="text-red-500 hover:underline">
                  Delete
                </Link>
              </div>
            </li>
          ))
        ) : (
          <p className="text-gray-600 text-center">No standard fees found.</p>
        )}
      </ul>
    </div>
  );
}
