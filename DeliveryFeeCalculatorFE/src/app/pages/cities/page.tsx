"use client";

import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import Link from "next/link";


interface City {
  id: string;
  name: string;
}

export default function Cities() {
  const { userInfo, isAuthenticated } = useAuth();

  const { data: cities, loading, error } = useFetchData<City[]>(
    "/api/v1.0/cities",
    userInfo?.jwt ?? null
  );

  if (!isAuthenticated) {
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  }
  if (loading) {
    return <p className="text-gray-500 text-center">Loading cities...</p>;
  }
  if (error) {
    return <p className="text-red-500 text-center">{error}</p>;
  }

  return (
    <div className="max-w-md mx-auto mt-10 bg-white p-6 rounded-lg shadow-lg">
      <h1 className="text-2xl font-bold text-gray-900 text-center mb-6">Cities</h1>

      <div className="text-center mb-4">
        <Link href="/pages/cities/create" className="text-blue-500 hover:underline">
          Create New City
        </Link>
      </div>

      <ul className="space-y-2">
        {Array.isArray(cities) && cities.length > 0 ? (
          cities.map((city) => (
            <li key={city.id} className="flex justify-between items-center border-b py-2">
              <span className="text-gray-800">{city.name}</span>
              <div className="space-x-4">
                <Link href={`/pages/cities/${city.id}`} className="text-green-500 hover:underline">
                  Details
                </Link>
                <Link href={`/pages/cities/${city.id}/edit`} className="text-blue-500 hover:underline">
                  Edit
                </Link>
                <Link href={`/pages/cities/${city.id}/delete`} className="text-red-500 hover:underline">
                  Delete
                </Link>
              </div>
            </li>
          ))
        ) : (
          <p className="text-gray-600 text-center">No cities found.</p>
        )}
      </ul>
    </div>
  );
}
