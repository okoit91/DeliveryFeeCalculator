"use client";

import { useRouter, usePathname } from "next/navigation";
import { useContext, useEffect, useState } from "react";
import { AppContext } from "@/state/AppContext";

export default function ProtectedRoute({ children }: { children: React.ReactNode }) {
  const { userInfo } = useContext(AppContext)!;
  const router = useRouter();
  const pathname = usePathname();
  const [hasRedirected, setHasRedirected] = useState(false);

  const unprotectedRoutes = ["/", "/pages/auth/login", "/pages/auth/register"];
  const isUnprotectedRoute = unprotectedRoutes.includes(pathname);

  useEffect(() => {
    if (userInfo === null && !isUnprotectedRoute && !hasRedirected) {
      setHasRedirected(true);
      router.replace("/pages/auth/login");
    }
  }, [userInfo, isUnprotectedRoute, router, hasRedirected]);

  if (userInfo === undefined) {
    return <p>Loading...</p>;
  }

  return <>{children}</>;
}
