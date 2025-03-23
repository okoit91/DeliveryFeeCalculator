"use client";

import { useAuth } from "@/hooks/useAuth";
import { useCreateForm } from "@/hooks/useCreateForm";
import Link from "next/link";


interface CityFormData {
  name: string;
}

export default function CreateCity() {
  const { userInfo, isAuthenticated } = useAuth();

  
  const { formValues, handleChange, handleSubmit, loading, error } = useCreateForm<CityFormData>(
    { name: "" },
    "/api/v1.0/cities",
    userInfo?.jwt ?? null,
    "/pages/cities"
  );


  if (!isAuthenticated) {
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  }

  return (
    <div className="max-w-2xl mx-auto mt-10 bg-white p-6 rounded-lg shadow-lg">
      <h1 className="text-2xl font-bold text-gray-900 text-center mb-6">
        Create City
      </h1>

      {/* Error Message */}
      {error && <p className="text-red-500 text-center mb-4">{error}</p>}

      {/* Create City Form */}
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block font-semibold text-gray-700">City Name</label>
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
          disabled={loading}
          className="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition disabled:opacity-50"
        >
          {loading ? "Creating..." : "Create City"}
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
