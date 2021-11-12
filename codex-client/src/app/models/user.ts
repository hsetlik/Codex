export interface User {
    username: string;
    displayName: string;
    token: string;
    lastStudiedLanguage: string;
}

export interface UserFormValues {
    email: string;
    password: string;
    username?: string;
}