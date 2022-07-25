export interface User {
    username: string;
    displayName: string;
    token: string;
    lastStudiedLanguage: string;
    nativeLanguage: string;
}

export interface UserFormValues {
    email: string;
    password: string;
    username?: string;
    nativeLanguage?: string;
    studyLanguage?: string
}