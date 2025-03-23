import axios from "axios";
import { IUserInfo } from "@/state/AppContext";
import { IResultObject } from "@/services/IResultObject";

export default class AccountService {
    private constructor() {}

    private static httpClient = axios.create({
        baseURL: "http://localhost:5203/api/v1.0/identity/Account",
        withCredentials: true,
    });

    private static async isInRole(role: string, jwt: string | null): Promise<boolean> {
        if (!jwt) return false;

        try {
            const response = await AccountService.httpClient.get<boolean>(
                `/IsInRole?role=${role}`, 
                { headers: { Authorization: `Bearer ${jwt}` } }
            );
            return response.data;
        } catch {
            return false;
        }
    }

    static async login(email: string, password: string): Promise<IResultObject<IUserInfo>> {
        try {
            console.log("Attempting login...");
            const response = await AccountService.httpClient.post<IUserInfo>("/login", { email, password });
    
            if (response.status < 300) {
                const userInfo = response.data;
                const jwt = userInfo.jwt ?? null; 
    
                const [isAdmin, isClient] = await Promise.all([
                    AccountService.isInRole("admin", jwt),
                    AccountService.isInRole("client", jwt),
                ]);
    
                userInfo.roles = [];
                if (isAdmin) userInfo.roles.push("admin");
                if (isClient) userInfo.roles.push("client");
    
                localStorage.setItem("userInfo", JSON.stringify(userInfo));
    
                console.log("Login successful:", userInfo);
                return { data: userInfo };
            }
    
            return { errors: [`${response.status} ${response.statusText}`] };
        } catch (error: any) {
            console.error("Login failed:", error.response?.data || error.message);
            return { errors: ["Login failed. Please try again."] };
        }
    }
    
    static logout() {
        localStorage.removeItem("userInfo");
    }


    static async request<T>(
        method: "GET" | "POST" | "PUT" | "DELETE",
        url: string,
        data?: any,
        token?: string | null
    ): Promise<T> {
        const isPublicEndpoint = url.toLowerCase().includes("/register") || url.toLowerCase().includes("/login");

        const jwt = isPublicEndpoint ? null : token ?? (JSON.parse(localStorage.getItem("userInfo") || "{}") as IUserInfo)?.jwt ?? null;

        if (!jwt && !isPublicEndpoint) {
            console.error("Unauthorized request blocked:", url);
            throw new Error("Unauthorized: No JWT Token Found.");
        }

        try {
            console.log(`Sending ${method} request to ${url}`);
            const response = await axios({
                method,
                url: `http://localhost:5203${url}`,
                data,
                headers: {
                    "Content-Type": "application/json",
                    ...(jwt ? { Authorization: `Bearer ${jwt}` } : {}),
                },
            });

            console.log(`API Response for ${url}:`, response.data);
            return response.data as T;
        } catch (error: any) {
            console.error("API request failed:", error.response?.data || error.message);
            throw error.response?.data || new Error("Request failed");
        }
    }
}
