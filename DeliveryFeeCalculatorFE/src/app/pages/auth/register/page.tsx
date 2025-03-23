"use client";
import { useState, useContext } from "react";
import { useRouter } from "next/navigation";
import { AppContext } from "@/state/AppContext";
import AccountService from "@/services/AccountService";

export default function Register() {
  const { setUserInfo } = useContext(AppContext)!;
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const router = useRouter();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
  
    try {
      console.log("Registering user...");
  
      const registerResponse = await AccountService.request("POST", "/api/v1.0/identity/Account/Register", {
        email,
        password,
        firstname: firstName,
        lastname: lastName,
      });
  
      console.log("Register response:", registerResponse);
  
    
      const result = await AccountService.login(email, password);
  
      if (result.data) {
        setUserInfo(result.data);
        router.push("/");
      } else {
        console.error("Login after registration failed:", result.errors);
        setError(result.errors?.join(", ") || "Failed to log in after registration");
      }
    } catch (err: any) {
      console.error("Registration error:", err);
      setError(err.response?.data?.error || "Failed to register account");
    }
  };
  

  return (
    <div className="flex justify-center items-center h-screen">
      <div className="bg-white p-8 rounded-lg shadow-lg w-full max-w-md">
        <h2 className="text-2xl font-bold text-gray-900 text-center mb-6">Register</h2>

        {error && <p className="text-red-500 text-center mb-4">{error}</p>}

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block font-semibold text-gray-700">First Name</label>
            <input
              type="text"
              className="w-full p-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-300"
              value={firstName}
              onChange={(e) => setFirstName(e.target.value)}
              required
            />
          </div>

          <div>
            <label className="block font-semibold text-gray-700">Last Name</label>
            <input
              type="text"
              className="w-full p-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-300"
              value={lastName}
              onChange={(e) => setLastName(e.target.value)}
              required
            />
          </div>

          <div>
            <label className="block font-semibold text-gray-700">Email</label>
            <input
              type="email"
              className="w-full p-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-300"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>

          <div>
            <label className="block font-semibold text-gray-700">Password</label>
            <input
              type="password"
              className="w-full p-2 border border-gray-300 rounded-md focus:ring focus:ring-blue-300"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          <button
            type="submit"
            className="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition"
          >
            Register
          </button>
        </form>
      </div>
    </div>
  );
}
