"use client";
import { AppContext } from "@/state/AppContext";
import Link from "next/link";
import { useContext } from "react";
import { useRouter } from "next/navigation";
import AccountService from "@/services/AccountService";

export default function Identity() {

    const appContext = useContext(AppContext);
    if (!appContext) return null;

    const { userInfo, setUserInfo } = appContext;
    const router = useRouter();

    const handleLogout = () => {
        AccountService.logout();
        setUserInfo(null);
        router.push("/");
    };

    return userInfo ? <LoggedIn handleLogout={handleLogout} userInfo={userInfo} /> : <LoggedOut />;
}

const LoggedIn = ({ handleLogout, userInfo }: { handleLogout: () => void, userInfo: any }) => {
    return (
        <ul className="flex items-center space-x-4">
            <li>
                <span className="text-gray-700">Hello, {userInfo.firstName} {userInfo.lastName}</span>
            </li>
            {userInfo?.roles?.includes("admin") && (
                <li className="relative group">
                    <button className="text-gray-700 hover:text-gray-900 flex items-center">
                        Pages
                        <svg xmlns="http://www.w3.org/2000/svg" className="h-4 w-4 ml-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
                        </svg>
                    </button>
                    <ul className="absolute left-0 invisible opacity-0 group-hover:visible group-hover:opacity-100 flex flex-col bg-white border shadow-md rounded-md py-2 mt-1 w-40 z-10 transition-all duration-200 ease-in-out">
                        <li>
                            <Link
                                href="/pages/cities"
                                className="px-4 py-2 text-gray-800 hover:text-black hover:bg-gray-100"
                            >
                                Cities
                            </Link>
                        </li>
                        <li>
                            <Link
                                href="/pages/vehicle-types"
                                className="px-4 py-2 text-gray-800 hover:text-black hover:bg-gray-100"
                            >
                                Vehicle Types
                            </Link>
                        </li>
                        <li>
                            <Link
                                href="/pages/extra-fees"
                                className="px-4 py-2 text-gray-800 hover:text-black hover:bg-gray-100"
                            >
                                Extra Fees
                            </Link>
                        </li>
                        <li>
                            <Link
                                href="/pages/standard-fees"
                                className="px-4 py-2 text-gray-800 hover:text-black hover:bg-gray-100"
                            >
                                Standard Fees
                            </Link>
                        </li>
                        <li>
                            <Link
                                href="/pages/stations"
                                className="px-4 py-2 text-gray-800 hover:text-black hover:bg-gray-100"
                            >
                                Stations
                            </Link>


                        </li>
                    </ul>
                </li>
            )}

            <li>
                <button
                    className="px-4 py-2 bg-red-500 text-white rounded-md hover:bg-red-600 transition"
                    onClick={handleLogout}
                >
                    Logout
                </button>
            </li>
        </ul>
    );
};

const LoggedOut = () => {
    return (
        <ul className="flex items-center space-x-4">
            <li><Link href="/pages/auth/login" className="text-gray-700 hover:text-gray-900">Login</Link></li>
            <li><Link href="/pages/auth/register" className="text-gray-700 hover:text-gray-900">Register</Link></li>
        </ul>
    );
};