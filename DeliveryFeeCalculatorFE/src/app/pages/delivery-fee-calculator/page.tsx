"use client";

import { useContext, useEffect, useState } from "react";
import { AppContext } from "@/state/AppContext";
import AccountService from "@/services/AccountService";

interface DeliveryFeeResponse {
  totalFee?: number;
  error?: string;
}

export default function DeliveryFeeCalculator() {

  const { userInfo } = useContext(AppContext)!;
  const [cities, setCities] = useState<{ id: string; name: string }[]>([]);
  const [vehicleTypes, setVehicleTypes] = useState<{ id: string; name: string }[]>([]);
  const [selectedCity, setSelectedCity] = useState("");
  const [selectedVehicle, setSelectedVehicle] = useState("");
  const [totalFee, setTotalFee] = useState<number | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadCities();
    loadVehicleTypes();
  }, []);

  async function loadCities() {
    try {
      const data = await AccountService.request<{ id: string; name: string }[]>(
        "GET",
        "/api/v1/Cities",
        undefined,
        userInfo!.jwt
      );
      setCities(data);
    } catch (err) {
      console.error("Error loading cities:", err);
    }
  }

  async function loadVehicleTypes() {
    try {
      const data = await AccountService.request<{ id: string; name: string }[]>(
        "GET",
        "/api/v1/VehicleTypes",
        undefined,
        userInfo!.jwt
      );
      setVehicleTypes(data);
    } catch (err) {
      console.error("Error loading vehicle types:", err);
    }
  }

  async function calculateFee() {
    if (!selectedCity || !selectedVehicle) {
      setError("Please select both a city and a vehicle type.");
      return;
    }

    try {
      const response = await AccountService.request<DeliveryFeeResponse>(
        "POST",
        "/api/v1/DeliveryFees/calculate",
        { cityId: selectedCity, vehicleTypeId: selectedVehicle },
        userInfo!.jwt
      );

      if (response.error) {
        setError(response.error);
        setTotalFee(null);
      } else {
        setTotalFee(response.totalFee ?? 0);
        setError(null);
      }
    } catch (err) {
      console.error("API Error:", err);
      setError("Failed to fetch the delivery fee.");
    }
  }

  return (
    <div className="max-w-md mx-auto mt-10 bg-white p-6 rounded-lg shadow-lg">
      <h2 className="text-2xl font-bold text-gray-900 text-center mb-6">
        Delivery Fee Calculator
      </h2>

      {/* City Selection */}
      <div className="mb-4">
        <label className="block font-semibold text-gray-700">Select City:</label>
        <select
          className="w-full p-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-300"
          value={selectedCity}
          onChange={(e) => setSelectedCity(e.target.value)}
        >
          <option value="">Select a city</option>
          {cities.map((city) => (
            <option key={city.id} value={city.id}>
              {city.name}
            </option>
          ))}
        </select>
      </div>

      {/* Vehicle Type Selection */}
      <div className="mb-4">
        <label className="block font-semibold text-gray-700">Select Vehicle Type:</label>
        <select
          className="w-full p-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-300"
          value={selectedVehicle}
          onChange={(e) => setSelectedVehicle(e.target.value)}
        >
          <option value="">Select a vehicle</option>
          {vehicleTypes.map((vehicle) => (
            <option key={vehicle.id} value={vehicle.id}>
              {vehicle.name}
            </option>
          ))}
        </select>
      </div>

      {/* Calculate Button */}
      <button
        onClick={calculateFee}
        className="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition"
      >
        Calculate Fee
      </button>

      {/* Results */}
      <div className="mt-4 text-lg text-center">
        {error && <p className="text-red-500">{error}</p>}
        {totalFee !== null && (
          <p className="text-green-600">
            <strong>Total Fee:</strong> €{totalFee.toFixed(2)}
          </p>
        )}
      </div>
    </div>
  );
}
