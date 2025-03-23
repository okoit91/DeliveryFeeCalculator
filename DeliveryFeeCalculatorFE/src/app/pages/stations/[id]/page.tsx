"use client";

import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import { useParams } from "next/navigation";
import Link from "next/link";

// Define the expected type for a Station
interface Station {
  id: string;
  stationName: string;
  cityId: string;
  city: {
    id: string;
    name: string;
  };
  wmoCode: number;
  isActive: boolean;
}

export default function StationDetails() {
  const { id } = useParams();
  const { userInfo, isAuthenticated } = useAuth();

  // Fetch station details
  const { data: station, loading, error } = useFetchData<Station>(
    `/api/v1.0/stations/${id}`,
    userInfo?.jwt ?? null
  );

  if (!isAuthenticated) {
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  }

  if (loading) {
    return <p className="text-gray-500 text-center">Loading station details...</p>;
  }

  if (error) {
    return <p className="text-red-500 text-center">{error}</p>;
  }

  if (!station) {
    return <p className="text-gray-500 text-center">Station not found.</p>;
  }

  return (
    <div className="max-w-lg mx-auto p-6 bg-white shadow-md rounded-md mt-10">
      <h1 className="text-2xl font-bold mb-6 text-center">Station Details</h1>

      <div className="mb-4">
        <p className="text-gray-900">
          <strong>Station Name:</strong> {station.stationName}
        </p>
        <p className="text-gray-900">
          <strong>City:</strong> {station.city?.name ?? "N/A"}
        </p>
        <p className="text-gray-900">
          <strong>WMO Code:</strong> {station.wmoCode}
        </p>
        <p className="text-gray-900">
          <strong>Status:</strong> {station.isActive ? "Active" : "Inactive"}
        </p>
      </div>

      <div className="text-center">
        <Link href="/pages/stations" className="text-blue-500 hover:underline">
          Back to Stations List
        </Link>
      </div>
    </div>
  );
}
