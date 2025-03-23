"use client";

import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import Link from "next/link";

interface City {
  id: string;
  name: string;
}

interface Station {
  id: string;
  cityId: string;
  city?: City;
  stationName: string;
  wmoCode: number;
  isActive: boolean;
}

export default function Stations() {
  const { userInfo, isAuthenticated } = useAuth();

  const { data: stations, loading, error } = useFetchData<Station[]>(
    "/api/v1.0/stations",
    userInfo?.jwt ?? null
  );

  if (!isAuthenticated)
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;

  if (loading)
    return <p className="text-gray-500 text-center">Loading stations...</p>;

  if (error)
    return <p className="text-red-500 text-center">{error}</p>;

  return (
    <div className="max-w-3xl mx-auto mt-10 bg-white p-6 rounded-lg shadow-lg">
      <h1 className="text-2xl font-bold text-gray-900 text-center mb-6">
        Stations
      </h1>

      <div className="text-center mb-4">
        <Link href="/pages/stations/create" className="text-blue-500 hover:underline">
          Create New Station
        </Link>
      </div>

      <ul className="space-y-2">
        {stations && stations.length > 0 ? (
          stations.map((station) => (
            <li key={station.id} className="flex justify-between items-center border-b py-2">
              <div>
                <p className="text-gray-900 font-medium">{station.stationName}</p>
                <p className="text-gray-600 text-sm">
                  City: {station.city?.name ?? "Unknown"} | WMO Code: {station.wmoCode}
                </p>
                <p className={`text-sm ${station.isActive ? "text-green-500" : "text-red-500"}`}>
                  {station.isActive ? "Active" : "Inactive"}
                </p>
              </div>
              <div className="space-x-4">
                <Link href={`/pages/stations/${station.id}`} className="text-green-500 hover:underline">
                  Details
                </Link>
                <Link href={`/pages/stations/${station.id}/edit`} className="text-blue-500 hover:underline">
                  Edit
                </Link>
                <Link href={`/pages/stations/${station.id}/delete`} className="text-red-500 hover:underline">
                  Delete
                </Link>
              </div>
            </li>
          ))
        ) : (
          <p className="text-gray-600 text-center">No stations found.</p>
        )}
      </ul>
    </div>
  );
}
