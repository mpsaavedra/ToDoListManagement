import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User, SignInRequest, SignUpRequest, SignInResponse } from '../models/user.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/api/User`;

  constructor(private http: HttpClient) { }

  signIn(request: SignInRequest): Observable<SignInResponse> {
    return this.http.post<SignInResponse>(`${this.apiUrl}/SignIn`, request);
  }

  signUp(request: SignUpRequest): Observable<User> {
    return this.http.post<User>(`${this.apiUrl}/SignUp`, request);
  }

  signOut(): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/SignOut`, {});
  }
}