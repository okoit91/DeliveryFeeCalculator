import { useEffect, useContext } from "react";
import { useRouter } from "next/navigation";
import { AppContext } from "@/state/AppContext";
import AccountService from "@/services/AccountService";

export default function Logout() {
  const router = useRouter();
  const { setUserInfo } = useContext(AppContext)!;

  useEffect(() => {
    const logoutUser = async () => {
      try {
        await AccountService.logout(); // Call backend logout
      } catch (error) {
        console.error("Logout error:", error);
      }
      
      setUserInfo(null); // Clear context
      localStorage.removeItem("userInfo"); // Remove stored user info
      router.replace("/pages/auth/login"); // Redirect without delay
    };

    logoutUser();
  }, [router, setUserInfo]);

  return (
    <div>
      <h1>Logging out...</h1>
      <p>You will be redirected shortly.</p>
    </div>
  );
}
