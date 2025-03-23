"use client";
import { useContext } from "react";
import Link from "next/link";
import { AppContext } from "@/state/AppContext";
import AccountService from "@/services/AccountService";
import { useRouter } from "next/navigation";
import Identity from "@/components/nav/Identity";

export default function Header() {
    const { userInfo } = useContext(AppContext)!;
    const router = useRouter();

    const handleLogout = () => {
        AccountService.logout();
        localStorage.removeItem("userInfo");
        router.push("/pages/auth/login");
    };

    return (
        <header className="bg-white shadow-md">
            <nav className="container mx-auto flex justify-between items-center py-4 px-6">
                <Link href="/" className="text-xl font-bold text-gray-900">
                    Delivery Fee Calculator
                </Link>
                <Identity />
            </nav>
        </header>
    );
}