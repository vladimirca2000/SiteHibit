import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginResponse } from './auth.models';

const TOKEN_KEY = 'hibit_auth_token';
const EXPIRES_KEY = 'hibit_auth_expires_at';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);

  getToken(): string | null {
    return sessionStorage.getItem(TOKEN_KEY);
  }

  hasValidToken(): boolean {
    const token = this.getToken();
    const expiresAt = sessionStorage.getItem(EXPIRES_KEY);
    if (!token || !expiresAt) {
      return false;
    }
    return new Date(expiresAt).getTime() > Date.now();
  }

  login(): Observable<void> {
    return this.http
      .post<LoginResponse>(`${environment.apiUrl}/api/auth/login`, {
        username: environment.appUser.username,
        password: environment.appUser.password,
      })
      .pipe(
        tap((response) => this.storeToken(response)),
        map(() => void 0),
      );
  }

  ensureAuthenticated(): Promise<void> {
    if (this.hasValidToken()) {
      return Promise.resolve();
    }
    return new Promise((resolve, reject) => {
      this.login().subscribe({
        next: () => resolve(),
        error: (err) => reject(err),
      });
    });
  }

  clearToken(): void {
    sessionStorage.removeItem(TOKEN_KEY);
    sessionStorage.removeItem(EXPIRES_KEY);
  }

  private storeToken(response: LoginResponse): void {
    sessionStorage.setItem(TOKEN_KEY, response.accessToken);
    sessionStorage.setItem(EXPIRES_KEY, response.expiresAt);
  }
}
