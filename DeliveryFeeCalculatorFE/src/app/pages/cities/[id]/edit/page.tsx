"use client";

import { useEffect } from "react";
import { useParams } from "next/navigation";
import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import { useEditForm } from "@/hooks/useEditForm";
import Link from "next/link";

interface CityFormData {
  id: string;
  name: string;
}

export default function EditCity() {
  const { id } = useParams();
  const { userInfo, isAuthenticated } = useAuth();


  const { data, loading, error } = useFetchData<CityFormData>(
    `/api/v1.0/cities/${id}`,
    userInfo?.jwt ?? null
  );

  const { formValues, handleChange, handleSubmit, loading: formLoading, error: formError } = useEditForm<CityFormData>(
    data ?? { id: "", name: "" },
    `/api/v1.0/cities/${id}`,
    userInfo?.jwt ?? null,
    "/pages/cities"
  );

  if (!isAuthenticated) return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  if (loading) return <p className="text-gray-500 text-center">Loading city data...</p>;
  if (error) return <p className="text-red-500 text-center">{error}</p>;

  return (
    <div className="max-w-md mx-auto p-6 bg-white shadow-md rounded-md">
      <h1 className="text-2xl font-bold mb-6 text-center">Edit City</h1>

      {formError && <p className="text-red-500 text-center">{formError}</p>}

      <form onSubmit={handleSubmit} className="space-y-4">
        {/* City Name */}
        <div>
          <label className="block text-gray-700 font-medium">City Name</label>
          <input
            type="text"
            name="name"
            value={formValues.name}
            onChange={handleChange}
            required
            className="w-full p-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-300"
          />
        </div>

        <button
          type="submit"
          disabled={formLoading}
          className="w-full bg-blue-500 text-white py-2 rounded-md hover:bg-blue-600 transition disabled:opacity-50"
        >
          {formLoading ? "Updating..." : "Update City"}
        </button>
      </form>

      {/* Back to Cities List */}
      <div className="text-center mt-4">
        <Link href="/pages/cities" className="text-blue-500 hover:underline">
          Back to Cities List
        </Link>
      </div>
    </div>
  );
}
