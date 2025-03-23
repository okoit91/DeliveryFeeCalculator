"use client";

import { useMemo } from "react";
import { useParams, useRouter } from "next/navigation";
import { useAuth } from "@/hooks/useAuth";
import { useFetchData } from "@/hooks/useFetchData";
import { useEditForm } from "@/hooks/useEditForm";
import Link from "next/link";

// Define the form structure for Standard Fee – only feeAmount is editable.
interface StandardFeeFormData {
  feeAmount: string;
}

export default function EditStandardFee() {
  const { id } = useParams();
  const router = useRouter();
  const { userInfo, isAuthenticated } = useAuth();

  // Fetch standard fee details from the API.
  const { data, loading, error } = useFetchData<any>(
    `/api/v1.0/standardFees/${id}`,
    userInfo?.jwt ?? null
  );

  // Memoize initial form values – we only allow editing feeAmount.
  const initialValues = useMemo<StandardFeeFormData>(() => ({
    feeAmount: data?.feeAmount !== undefined ? data.feeAmount.toString() : "0",
  }), [data]);

  // Use the edit form hook to manage form state.
  const { formValues, handleChange, loading: formLoading, error: formError } = useEditForm(
    initialValues,
    `/api/v1.0/standardFees/${id}`,
    userInfo?.jwt ?? null,
    "/standardFees" // Redirect URL after a successful update.
  );

  // Custom handleSubmit builds a payload that includes all required fields.
  // Only feeAmount is updated; other fields are taken from the fetched data.
  const handleCustomSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const payload = {
      id: id, // from URL parameter
      cityId: data?.cityId,           // from fetched data
      city: data?.city,               // nested object from fetched data
      vehicleTypeId: data?.vehicleTypeId, // from fetched data
      vehicleType: data?.vehicleType,     // nested object from fetched data
      feeAmount: parseFloat(formValues.feeAmount),
    };

    try {
      const apiUrl = `${process.env.NEXT_PUBLIC_API_BASE_URL}/api/v1.0/standardFees/${id}`;
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
      router.push("/pages/standard-fees");
    } catch (err: any) {
      console.error("Edit form error:", err);
      // Optionally update a state variable to display the error.
    }
  };

  if (!isAuthenticated)
    return <p className="text-red-500 text-center">Unauthorized: Please log in.</p>;
  if (loading)
    return <p className="text-gray-700 text-center">Loading standard fee data...</p>;
  if (error)
    return <p className="text-red-500 text-center">{error}</p>;

  return (
    <div className="max-w-lg mx-auto p-6 bg-white shadow-md rounded-md">
      <h1 className="text-2xl font-bold mb-6 text-center">Edit Standard Fee</h1>

      {formError && <p className="text-red-500 text-center">{formError}</p>}

      <form onSubmit={handleCustomSubmit} className="space-y-4">
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

        <button
          type="submit"
          disabled={formLoading}
          className="w-full bg-blue-500 text-white py-2 rounded-md hover:bg-blue-600 transition disabled:opacity-50"
        >
          {formLoading ? "Updating..." : "Update Standard Fee"}
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
