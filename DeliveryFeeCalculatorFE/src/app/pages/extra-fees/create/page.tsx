"use client";

import { useAuth } from "@/hooks/useAuth";
import { useCreateForm } from "@/hooks/useCreateForm";
import { useFetchData } from "@/hooks/useFetchData";
import Link from "next/link";

// Define the form structure for Extra Fee (without the ID field)
interface ExtraFeeFormData {
  vehicleTypeId: string;
  vehicleTypeName: string;
  conditionType: string;
  feeAmount: number;
  minValue: number | null;
  maxValue: number | null;
}

// Define type for Vehicle Type
interface VehicleType {
  id: string;
  name: string;
}

export default function CreateExtraFee() {
  const { userInfo, isAuthenticated } = useAuth();

  const {
    data: vehicleTypes,
    loading: vtLoading,
    error: vtError,
  } = useFetchData<VehicleType[]>(
    "/api/v1/VehicleTypes",
    userInfo?.jwt ?? null
  );

  // Initial form values for extra fee
  const initialValues: ExtraFeeFormData = {
    vehicleTypeId: "",
    vehicleTypeName: "",
    conditionType: "",
    feeAmount: 0,
    minValue: null,
    maxValue: null,
  };

  const {
    formValues,
    setFormValues,
    handleChange,
    handleSubmit,
    error: formError,
    loading: formLoading,
  } = useCreateForm<ExtraFeeFormData>(
    initialValues,
    "/api/v1.0/extraFees",
    userInfo?.jwt ?? null,
    "/pages/extra-fees"
  );

  const handleVehicleTypeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedId = e.target.value;
    const selectedVehicle = vehicleTypes?.find((vt) => vt.id === selectedId);
    setFormValues({
      ...formValues,
      vehicleTypeId: selectedId,
      vehicleTypeName: selectedVehicle?.name || "",
    });
  };

  if (!isAuthenticated) {
    return (
      <p className="text-red-500 text-center">Unauthorized: Please log in.</p>
    );
  }

  if (vtLoading) {
    return (
      <p className="text-gray-500 text-center">Loading vehicle types...</p>
    );
  }

  if (vtError) {
    return <p className="text-red-500 text-center">{vtError}</p>;
  }

  return (
    <div className="max-w-lg mx-auto p-6 bg-white shadow-md rounded-md mt-10">
      <h1 className="text-2xl font-bold mb-6 text-center">Create Extra Fee</h1>
      {formError && <p className="text-red-500 mb-4">{formError}</p>}

      <form onSubmit={handleSubmit} className="space-y-4">
        {/* Vehicle Type Dropdown */}
        <div>
          <label className="block text-gray-700 font-medium">Vehicle Type</label>
          <select
            name="vehicleTypeId"
            className="w-full border border-gray-300 px-3 py-2 rounded-md focus:ring focus:ring-blue-300"
            value={formValues.vehicleTypeId}
            onChange={handleVehicleTypeChange}
            required
          >
            <option value="">Select a vehicle type</option>
            {vehicleTypes?.map((vt) => (
              <option key={vt.id} value={vt.id}>
                {vt.name}
              </option>
            ))}
          </select>
        </div>

        {/* Condition Type */}
        <select
          name="conditionType"
          value={formValues.conditionType}
          onChange={handleChange}
          required
          className="w-full border border-gray-300 px-3 py-2 rounded-md focus:ring focus:ring-blue-300"
        >
          <option value="">Select a condition type</option>
          <option value="temperature">Temperature</option>
          <option value="windspeed">WindSpeed</option>
          <option value="rain">Rain</option>
          <option value="snow">Snow</option>
          <option value="sleet">Sleet</option>
          <option value="sleet">Thunder</option>
          <option value="sleet">Hail</option>
          <option value="sleet">Glaze</option>
        </select>

        {/* Fee Amount */}
        <div>
          <label className="block text-gray-700 font-medium">Fee Amount ($)</label>
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

        {/* Min Value */}
        <div>
          <label className="block text-gray-700 font-medium">Min Value</label>
          <input
            type="number"
            step="0.01"
            name="minValue"
            value={formValues.minValue === null ? "" : formValues.minValue}
            onChange={handleChange}
            placeholder="Leave empty for null"
            className="w-full border border-gray-300 px-3 py-2 rounded-md focus:ring focus:ring-blue-300"
          />
        </div>

        {/* Max Value */}
        <div>
          <label className="block text-gray-700 font-medium">Max Value</label>
          <input
            type="number"
            step="0.01"
            name="maxValue"
            value={formValues.maxValue === null ? "" : formValues.maxValue}
            onChange={handleChange}
            placeholder="Leave empty for null"
            className="w-full border border-gray-300 px-3 py-2 rounded-md focus:ring focus:ring-blue-300"
          />
        </div>

        <button
          type="submit"
          disabled={formLoading}
          className="w-full bg-blue-500 text-white py-2 rounded-md hover:bg-blue-600 transition"
        >
          {formLoading ? "Creating..." : "Create Extra Fee"}
        </button>
      </form>

      <div className="text-center mt-4">
        <Link href="/pages/extra-fees" className="text-blue-500 hover:underline">
          Back to Extra Fees List
        </Link>
      </div>
    </div>
  );
}
