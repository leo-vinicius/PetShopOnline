import { createContext, ReactNode, useContext, useState } from 'react';

type AuthData = {
    token: string;
    userType: string;
    userId: string;
};

type AuthContextType = {
    auth: AuthData | null;
    login: (token: string, userType: string, userId: string) => void;
    logout: () => void;
};

const AuthContext = createContext<AuthContextType>({
    auth: null,
    login: () => { },
    logout: () => { },
});

export function AuthProvider({ children }: { children: ReactNode }) {
    const [auth, setAuth] = useState<AuthData | null>(() => {
        const stored = localStorage.getItem('auth');
        return stored ? JSON.parse(stored) : null;
    });

    const login = (token: string, userType: string, userId: string) => {
        const authData = { token, userType, userId };
        setAuth(authData);
        localStorage.setItem('auth', JSON.stringify(authData));
    };

    const logout = () => {
        setAuth(null);
        localStorage.removeItem('auth');
    };

    return (
        <AuthContext.Provider value={{ auth, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    return useContext(AuthContext);
}