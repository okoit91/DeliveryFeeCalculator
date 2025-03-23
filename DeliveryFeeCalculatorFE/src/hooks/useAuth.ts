import { useContext } from "react";
import { AppContext } from "@/state/AppContext";

export function useAuth() {
  const appContext = useContext(AppContext);
  const userInfo = appContext?.userInfo;

  if (!userInfo?.jwt) {
    return { userInfo: null, isAuthenticated: false, isAdmin: false, isClient: false };
  }

  const roles = (userInfo.roles || []).map(role => role.toLowerCase());

  return { 
    userInfo, 
    isAuthenticated: true, 
    isAdmin: roles.includes("admin"),
    isClient: roles.includes("client")
  };
}
