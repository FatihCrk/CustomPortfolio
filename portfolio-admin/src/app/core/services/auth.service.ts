import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, BehaviorSubject } from 'rxjs';
import { LoginRequest, LoginResponse, User, SetupWizardRequest } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = 'https://localhost:7000/api/auth';
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();
  
  private isAuthenticatedSignal = signal<boolean>(false);
  public isAuthenticated = this.isAuthenticatedSignal.asReadonly();

  constructor(private http: HttpClient) {
    this.loadUserFromStorage();
  }

  private loadUserFromStorage(): void {
    const token = localStorage.getItem('access_token');
    const userStr = localStorage.getItem('current_user');
    
    if (token && userStr) {
      try {
        const user = JSON.parse(userStr) as User;
        this.currentUserSubject.next(user);
        this.isAuthenticatedSignal.set(true);
      } catch {
        this.logout();
      }
    }
  }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, request).pipe(
      tap(response => {
        localStorage.setItem('access_token', response.accessToken);
        localStorage.setItem('refresh_token', response.refreshToken);
        localStorage.setItem('current_user', JSON.stringify(response.user));
        this.currentUserSubject.next(response.user);
        this.isAuthenticatedSignal.set(true);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
    localStorage.removeItem('current_user');
    this.currentUserSubject.next(null);
    this.isAuthenticatedSignal.set(false);
  }

  checkSetupStatus(): Observable<{ isSetupCompleted: boolean }> {
    return this.http.get<{ isSetupCompleted: boolean }>(`${this.apiUrl}/setup-status`);
  }

  setup(request: SetupWizardRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/setup`, request);
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.getValue();
  }

  getToken(): string | null {
    return localStorage.getItem('access_token');
  }
}
