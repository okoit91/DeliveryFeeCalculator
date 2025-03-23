"use client";
import { useState, useEffect } from "react";
import { AppContext, IUserInfo } from "@/state/AppContext";

export default function AppState({ children }: { children: React.ReactNode }) {
  const [userInfo, setUserInfo] = useState<IUserInfo | null | undefined>(undefined);
  const [hasLoadedFromStorage, setHasLoadedFromStorage] = useState(false);

  useEffect(() => {
    try {
      const storedUserInfo = localStorage.getItem("userInfo");
      if (storedUserInfo) {
        setUserInfo(JSON.parse(storedUserInfo));
      } else {
        setUserInfo(null);
      }
    } catch (error) {
      console.error("Error parsing user info from localStorage:", error);
      localStorage.removeItem("userInfo");
      setUserInfo(null);
    } finally {
      setHasLoadedFromStorage(true);
    }
  }, []);

  useEffect(() => {
    if (userInfo) {
      localStorage.setItem("userInfo", JSON.stringify(userInfo));
    } else {
      localStorage.removeItem("userInfo");
      localStorage.removeItem("jwt");
    }
  }, [userInfo]);

  return (
    <AppContext.Provider value={{ userInfo, setUserInfo, hasLoadedFromStorage }}>
      {children}
    </AppContext.Provider>
  );
}
