"use client";

import { useParams } from "next/navigation";
import { useFetchData } from "@/hooks/useFetchData";
import { useAuth } from "@/hooks/useAuth";
import Link from "next/link";

// Define the expected type for a City
interface City {
  id: string;
  name: string;
  // Add any other fields your city object might have
}

export default function CityDetails() {
  const { id } = useParams();
  const { userInfo, isAuthenticated } = useAuth();

  // Fetch city details using the city id from the URL and JWT from userInfo
  const { data, loading, error } = useFetchData<City>(
    `/api/v1.0/cities/${id}`,
    userInfo?.jwt ?? null
  );

  if (!isAuthenticated)
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  if (loading)
    return <p className="text-gray-500 text-center">Loading city details...</p>;
  if (error) return <p className="text-red-500 text-center">{error}</p>;
  if (!data) return <p className="text-gray-500 text-center">City not found.</p>;

  return (
    <div className="max-w-md mx-auto p-6 bg-white shadow-md rounded-md">
      <h1 className="text-2xl font-bold mb-4 text-center">City Details</h1>
      <div className="mb-4">
        <p className="text-gray-900">
          <strong>Name:</strong> {data.name}
        </p>
      </div>
      <div className="text-center">
        <Link href="/pages/cities" className="text-blue-500 hover:underline">
          Back to Cities List
        </Link>
      </div>
    </div>
  );
}
