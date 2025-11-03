export interface SignInRequest {
  userName?: string;
  password?: string;
  rememberMe: boolean;
}

export interface User {
  id: number;
  userName?: string;
  role?: { id: number; name: string };
}

export interface SignInResponse {
  token?: string;
  user: User;
}
