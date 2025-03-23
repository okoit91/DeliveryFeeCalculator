"use client";

import { useMemo } from "react";
import { useParams, useRouter } from "next/navigation";
import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import { useEditForm } from "@/hooks/useEditForm";
import Link from "next/link";
import { buildExtraFeePayload } from "@/helpers/extraFeePayload"; // adjust the import path as needed
import { InputGroup } from "@/components/InputGroup"; // optional reusable component

// Define the form structure for Extra Fee (without vehicleTypeName)
interface ExtraFeeFormData {
  conditionType: string;
  feeAmount: string;
  minValue: string;
  maxValue: string;
  vehicleTypeId: string;
}

export default function EditExtraFee() {
  const { id } = useParams();
  const router = useRouter();
  const { userInfo, isAuthenticated } = useAuth();

  // Fetch extra fee details from the API
  const { data, loading, error } = useFetchData<any>(
    `/api/v1.0/ExtraFees/${id}`,
    userInfo?.jwt ?? null
  );

  // Memoize initial form values based on fetched data.
  const initialValues = useMemo<ExtraFeeFormData>(() => ({
    conditionType: data?.conditionType || "",
    feeAmount: data?.feeAmount !== undefined ? data.feeAmount.toString() : "0",
    minValue: data && data.minValue !== null ? data.minValue.toString() : "",
    maxValue: data && data.maxValue !== null ? data.maxValue.toString() : "",
    vehicleTypeId: data?.vehicleTypeId || "",
  }), [data]);

  // Use the edit form hook to manage form state.
  const { formValues, handleChange, loading: formLoading, error: formError } = useEditForm(
    initialValues,
    `/api/v1.0/ExtraFees/${id}`,
    userInfo?.jwt ?? null,
    "/pages/extra-fees" // Redirect URL after successful update
  );

  // Custom handleSubmit that builds the payload using our helper function.
  const handleCustomSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const payload = buildExtraFeePayload(id as string, formValues);

    try {
      const apiUrl = `${process.env.NEXT_PUBLIC_API_BASE_URL}/api/v1.0/ExtraFees/${id}`;
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
      router.push("/pages/extra-fees");
    } catch (err: any) {
      console.error("Edit form error:", err);
      // Optionally, update a state variable to display the error
    }
  };

  if (!isAuthenticated)
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  if (loading)
    return <p className="text-gray-700 text-center">Loading extra fee data...</p>;
  if (error)
    return <p className="text-red-500 text-center">{error}</p>;

  // Derive the vehicle type name from fetched data (read-only)
  const vehicleTypeName = data?.vehicleType?.name || "Unknown";

  return (
    <div className="max-w-lg mx-auto p-6 bg-white shadow-md rounded-md">
      <h1 className="text-2xl font-bold mb-6 text-center">Edit Extra Fee</h1>

      {formError && <p className="text-red-500 text-center">{formError}</p>}

      <form onSubmit={handleCustomSubmit} className="space-y-4">
        {/* Condition Type */}
        <InputGroup
          label="Condition Type"
          name="conditionType"
          value={formValues.conditionType}
          onChange={handleChange}
        />

        {/* Fee Amount */}
        <InputGroup
          label="Fee Amount ($)"
          type="number"
          name="feeAmount"
          value={formValues.feeAmount}
          onChange={handleChange}
        />

        {/* Vehicle Type (read-only) */}
        <InputGroup
          label="Vehicle Type"
          value={vehicleTypeName}
          disabled
        />

        {/* Min Value */}
        <InputGroup
          label="Min Value"
          type="number"
          name="minValue"
          value={formValues.minValue}
          onChange={handleChange}
          placeholder="Leave empty for null"
        />

        {/* Max Value */}
        <InputGroup
          label="Max Value"
          type="number"
          name="maxValue"
          value={formValues.maxValue}
          onChange={handleChange}
          placeholder="Leave empty for null"
        />

        <button
          type="submit"
          disabled={formLoading}
          className="w-full bg-blue-500 text-white py-2 rounded-md hover:bg-blue-600 transition disabled:opacity-50"
        >
          {formLoading ? "Updating..." : "Update Extra Fee"}
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
