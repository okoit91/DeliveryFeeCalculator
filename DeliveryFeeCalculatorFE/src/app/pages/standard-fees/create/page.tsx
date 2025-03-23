"use client";

import { useAuth } from "@/hooks/useAuth";
import { useCreateForm } from "@/hooks/useCreateForm";
import { useFetchData } from "@/hooks/useFetchData";
import Link from "next/link";

// Define form structure for creating a Standard Fee.
interface StandardFeeFormData {
  cityId: string;
  vehicleTypeId: string;
  feeAmount: number;
}

// Define types for City and VehicleType.
interface City {
  id: string;
  name: string;
}

interface VehicleType {
  id: string;
  name: string;
}

export default function CreateStandardFee() {
  const { userInfo, isAuthenticated } = useAuth();

  // Fetch available cities.
  const {
    data: cities,
    loading: citiesLoading,
    error: citiesError,
  } = useFetchData<City[]>("/api/v1.0/cities", userInfo?.jwt ?? null);

  // Fetch available vehicle types.
  const {
    data: vehicleTypes,
    loading: vtLoading,
    error: vtError,
  } = useFetchData<VehicleType[]>("/api/v1.0/vehicleTypes", userInfo?.jwt ?? null);

  // Initial form values.
  const initialValues: StandardFeeFormData = {
    cityId: "",
    vehicleTypeId: "",
    feeAmount: 0, // Changed from string to number
  };

  // Use the create form hook.
  const {
    formValues,
    setFormValues, // Ensure we update form values manually
    handleChange,
    handleSubmit,
    error: formError,
    loading: formLoading,
  } = useCreateForm<StandardFeeFormData>(
    initialValues,
    "/api/v1.0/standardFees", // Hardcoded API URL
    userInfo?.jwt ?? null,
    "/pages/standard-fees" // Fixed redirect URL to match CreateExtraFee
  );

  // Handle dropdown changes explicitly
  const handleCityChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setFormValues({
      ...formValues,
      cityId: e.target.value,
    });
  };

  const handleVehicleTypeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setFormValues({
      ...formValues,
      vehicleTypeId: e.target.value,
    });
  };

  // Debugging: Log form values before submitting
  const handleSubmitDebug = async (e: React.FormEvent) => {
    e.preventDefault();
    console.log("Submitting Form Data:", formValues);
    try {
      await handleSubmit(e);
    } catch (error) {
      console.error("Error submitting Standard Fee:", error);
    }
  };

  if (!isAuthenticated) {
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  }

  if (citiesLoading || vtLoading) {
    return <p className="text-gray-500 text-center">Loading options...</p>;
  }

  if (citiesError || vtError) {
    return <p className="text-red-500 text-center">Error loading options.</p>;
  }

  return (
    <div className="max-w-lg mx-auto p-6 bg-white shadow-md rounded-md mt-10">
      <h1 className="text-2xl font-bold mb-6 text-center">Create Standard Fee</h1>

      {formError && <p className="text-red-500 text-center mb-4">{formError}</p>}

      <form onSubmit={handleSubmitDebug} className="space-y-4">
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

        {/* Vehicle Type Dropdown */}
        <div>
          <label className="block text-gray-700 font-medium">Vehicle Type</label>
          <select
            name="vehicleTypeId"
            value={formValues.vehicleTypeId}
            onChange={handleVehicleTypeChange}
            required
            className="w-full border border-gray-300 px-3 py-2 rounded-md focus:ring focus:ring-blue-300"
          >
            <option value="">Select a vehicle type</option>
            {vehicleTypes?.map((vt) => (
              <option key={vt.id} value={vt.id}>
                {vt.name}
              </option>
            ))}
          </select>
        </div>

        {/* Fee Amount */}
        <div>
          <label className="block text-gray-700 font-medium">Fee Amount ($):</label>
          <input
            type="number"
            step="0.01"
            name="feeAmount"
            value={formValues.feeAmount}
            onChange={handleChange}
            required
            className="w-full border border-gray-300 px-3 py-2 rounded-md focus:ring focus:ring-blue-300"
          />
        </div>

        <button
          type="submit"
          disabled={formLoading}
          className="w-full bg-blue-500 text-white py-2 rounded-md hover:bg-blue-600 transition disabled:opacity-50"
        >
          {formLoading ? "Creating..." : "Create Standard Fee"}
        </button>
      </form>

      <div className="text-center mt-4">
        <Link href="/pages/standard-fees" className="text-blue-500 hover:underline">
          Back to Standard Fees List
        </Link>
      </div>
    </div>
  );
}
