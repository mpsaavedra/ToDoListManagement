export interface User {
  id: number;
  userName: string;
  email: string;
  firstName: string;
  lastName: string;
}

export interface SignInRequest {
  userName: string;
  password: string;
}

export interface SignUpRequest {
  userName: string;
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

export interface SignInResponse {
  token: string;
  user: User;
}