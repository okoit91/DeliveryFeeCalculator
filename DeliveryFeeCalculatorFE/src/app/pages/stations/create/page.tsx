"use client";

import { useAuth } from "@/hooks/useAuth";
import { useCreateForm } from "@/hooks/useCreateForm";
import { useFetchData } from "@/hooks/useFetchData";
import Link from "next/link";

// Define form structure for creating a Station.
interface StationFormData {
  stationName: string;
  cityId: string;
  wmoCode: number;
  isActive: boolean;
}

// Define types for City.
interface City {
  id: string;
  name: string;
}

export default function CreateStation() {
  const { userInfo, isAuthenticated } = useAuth();

  // Fetch available cities.
  const {
    data: cities,
    loading: citiesLoading,
    error: citiesError,
  } = useFetchData<City[]>("/api/v1.0/cities", userInfo?.jwt ?? null);

  // Initial form values.
  const initialValues: StationFormData = {
    stationName: "",
    cityId: "",
    wmoCode: 0,
    isActive: false,
  };

  // Use the create form hook.
  const {
    formValues,
    setFormValues,
    handleChange,
    handleSubmit,
    error: formError,
    loading: formLoading,
  } = useCreateForm<StationFormData>(
    initialValues,
    "/api/v1.0/stations", // API endpoint for station creation
    userInfo?.jwt ?? null,
    "/pages/stations" // Redirect after successful creation
  );

  // Handle dropdown changes explicitly
  const handleCityChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setFormValues({
      ...formValues,
      cityId: e.target.value,
    });
  };

  // Handle checkbox toggle
  const handleCheckboxChange = () => {
    setFormValues({
      ...formValues,
      isActive: !formValues.isActive,
    });
  };

  // Debugging: Log form values before submitting
  const handleSubmitDebug = async (e: React.FormEvent) => {
    e.preventDefault();
    console.log("Submitting Form Data:", formValues);
    try {
      await handleSubmit(e);
    } catch (error) {
      console.error("Error submitting Station:", error);
    }
  };

  if (!isAuthenticated) {
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  }

  if (citiesLoading) {
    return <p className="text-gray-500 text-center">Loading cities...</p>;
  }

  if (citiesError) {
    return <p className="text-red-500 text-center">Error loading cities.</p>;
  }

  return (
    <div className="max-w-lg mx-auto p-6 bg-white shadow-md rounded-md mt-10">
      <h1 className="text-2xl font-bold mb-6 text-center">Create Station</h1>

      {formError && <p className="text-red-500 text-center mb-4">{formError}</p>}

      <form onSubmit={handleSubmitDebug} className="space-y-4">
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
            onChange={handleCityChange}
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
            onChange={handleChange}
            required
            className="w-full border border-gray-300 px-3 py-2 rounded-md focus:ring focus:ring-blue-300"
          />
        </div>

        {/* Is Active Checkbox */}
        <div className="flex items-center">
          <input
            type="checkbox"
            name="isActive"
            checked={formValues.isActive}
            onChange={handleCheckboxChange}
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
          {formLoading ? "Creating..." : "Create Station"}
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
