"use client";

import { useEffect } from "react";
import { useRouter } from "next/navigation";
import { useAuth } from "@/hooks/useAuth";
import Link from "next/link";

export default function Home() {
  const router = useRouter();
  const { isAuthenticated } = useAuth();

  useEffect(() => {
    if (isAuthenticated) {
      router.replace("/pages/delivery-fee-calculator");
    }
  }, [isAuthenticated, router]);

  if (isAuthenticated) {
    return null;
  }

  return (
    <div className="flex flex-col items-center justify-center h-screen">
      <p className="text-3xl font-bold text-gray-400 mb-4">
        Welcome to the Delivery Fee Calculator
      </p>
      <p className="text-gray-400 mb-6">Please log in or register to continue.</p>
      
      <div className="flex space-x-4">
        <Link
          href="/pages/auth/login"
          className="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition"
        >
          Login
        </Link>
        <Link
          href="/pages/auth/register"
          className="px-4 py-2 bg-green-500 text-white rounded-md hover:bg-green-600 transition"
        >
          Register
        </Link>
      </div>
    </div>
  );
}