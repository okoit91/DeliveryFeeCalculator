"use client";

import { useMemo } from "react";
import { useParams, useRouter } from "next/navigation";
import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import { useEditForm } from "@/hooks/useEditForm";
import Link from "next/link";

// Define City and Station interfaces
interface City {
  id: string;
  name: string;
}

interface Station {
  id: string;
  stationName: string;
  cityId: string;
  city: City;
  wmoCode: number;
  isActive: boolean;
}

export default function EditStation() {
  const { id } = useParams();
  const router = useRouter();
  const { userInfo, isAuthenticated } = useAuth();

  // Fetch station details
  const { data, loading, error } = useFetchData<Station>(
    `/api/v1.0/stations/${id}`,
    userInfo?.jwt ?? null
  );

  // Fetch list of available cities
  const { data: cities } = useFetchData<City[]>(
    `/api/v1.0/cities`,
    userInfo?.jwt ?? null
  );

  // Memoize initial form values
  const initialValues = useMemo(() => ({
    id: data?.id || "",
    stationName: data?.stationName || "",
    cityId: data?.cityId || "",
    city: data?.city || { id: "", name: "" },
    wmoCode: data?.wmoCode !== undefined ? data.wmoCode.toString() : "0",
    isActive: data?.isActive || false,
  }), [data]);

  // Use edit form hook
  const {
    formValues,
    handleChange,
    loading: formLoading,
    error: formError,
    setFormValues,
  } = useEditForm(
    initialValues,
    `/api/v1.0/stations/${id}`,
    userInfo?.jwt ?? null,
    "/stations"
  );

  // Handle form submission with correct payload
  const handleCustomSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const selectedCity = cities?.find(city => city.id === formValues.cityId);
    if (!selectedCity) {
      console.error("Invalid city selection");
      return;
    }

    const payload = {
      id: formValues.id,
      stationName: formValues.stationName,
      cityId: selectedCity.id,
      city: selectedCity, // Ensure city object is included
      wmoCode: Number(formValues.wmoCode), // Ensure it's a number
      isActive: formValues.isActive,
    };

    console.log("Submitting payload:", payload);

    try {
      const apiUrl = `${process.env.NEXT_PUBLIC_API_BASE_URL}/api/v1.0/stations/${id}`;
      const response = await fetch(apiUrl, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${userInfo?.jwt}`,
        },
        body: JSON.stringify(payload),
      });

      if (!response.ok) {
        const result = await response.json();
        throw new Error(result?.errors ? JSON.stringify(result.errors) : "Update failed");
      }

      router.push("/pages/stations");
    } catch (err: any) {
      console.error("Edit form error:", err);
    }
  };

  if (!isAuthenticated) {
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  }

  if (loading) {
    return <p className="text-gray-700 text-center">Loading station data...</p>;
  }

  if (error) {
    return <p className="text-red-500 text-center">{error}</p>;
  }

  return (
    <div className="max-w-lg mx-auto p-6 bg-white shadow-md rounded-md">
      <h1 className="text-2xl font-bold mb-6 text-center">Edit Station</h1>

      {formError && <p className="text-red-500 text-center">{formError}</p>}

      <form onSubmit={handleCustomSubmit} className="space-y-4">
        {/* Station Name Input */}
        <div>
          <label className="block text-gray-700 font-medium">Station Name</label>
          <input
            type="text"
            name="stationName"
            value={formValues.stationName}
            onChange={handleChange}
            required
            className="w-full border border-gray-300 px-3 py-2 rounded-md focus:ring focus:ring-blue-300"
          />
        </div>

        {/* City Dropdown */}
        <div>
          <label className="block text-gray-700 font-medium">City</label>
          <select
            name="cityId"
            value={formValues.cityId}
            onChange={(e) => setFormValues({ ...formValues, cityId: e.target.value })}
            required
            className="w-full border border-gray-300 px-3 py-2 rounded-md focus:ring focus:ring-blue-300"
          >
            <option value="">Select a city</option>
            {cities?.map((city) => (
              <option key={city.id} value={city.id}>
                {city.name}
              </option>
            ))}
          </select>
        </div>

        {/* WMO Code Input */}
        <div>
          <label className="block text-gray-700 font-medium">WMO Code</label>
          <input
            type="number"
            step="1"
            name="wmoCode"
            value={formValues.wmoCode}
            onChange={(e) => setFormValues({ ...formValues, wmoCode: e.target.value })}
            required
            className="w-full border border-gray-300 px-3 py-2 rounded-md focus:ring focus:ring-blue-300"
          />
        </div>

        {/* Is Active Checkbox */}
        <div className="flex items-center">
          <input
            type="checkbox"
            checked={formValues.isActive}
            onChange={() => setFormValues({ ...formValues, isActive: !formValues.isActive })}
            className="mr-2"
          />
          <label className="text-gray-700">Is Active</label>
        </div>

        {/* Submit Button */}
        <button
          type="submit"
          disabled={formLoading}
          className="w-full bg-blue-500 text-white py-2 rounded-md hover:bg-blue-600 transition disabled:opacity-50"
        >
          {formLoading ? "Updating..." : "Update Station"}
        </button>
      </form>

      <div className="text-center mt-4">
        <Link href="/pages/stations" className="text-blue-500 hover:underline">
          Back to Stations List
        </Link>
      </div>
    </div>
  );
}
