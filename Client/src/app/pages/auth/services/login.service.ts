import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { LoginModel } from '../models/login.model';
import { Environment } from '../../../environment/environment.dev';
import { lastValueFrom } from 'rxjs';
import { ToastService } from '../../../shared/services/toast.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  http = inject(HttpClient);
  toastService = inject(ToastService);
  router = inject(Router);
  isLoggedIn = signal<boolean>(false);
  isLoading = signal<boolean>(false);

  async login(loginModel: LoginModel) {
    try {
      this.isLoading.set(true);
      await lastValueFrom(
        this.http.post(Environment.apiUrl + '/login', loginModel, { withCredentials: true })
      );
      this.isLoggedIn.set(true);
      this.isLoading.set(false);
      this.router.navigate(['/']);
    } catch (error) {
      this.isLoggedIn.set(false);
      this.isLoading.set(false);
      if (error instanceof HttpErrorResponse) {
        this.toastService.showToast({
          severity: 'error',
          summary: 'Error',
          message:
            error.status === 401
              ? 'Usuario o contraseña incorrectos'
              : 'Error desconocido. Intenta nuevamente más tarde',
          code: error.status,
        });
        console.error('Error status code:', error.status);
      }
    }
  }

  async status(): Promise<boolean> {
    try {
      this.isLoading.set(true);
      await lastValueFrom(
        this.http.post<boolean>(
          Environment.apiUrl + '/login/auth-status',
          {},
          { withCredentials: true }
        )
      );
      this.isLoggedIn.set(true);
      this.isLoading.set(false);
      return true;
    } catch (error) {
      this.isLoggedIn.set(false);
      this.isLoading.set(false);
      return false;
    }
  }

  checkLoginStatus() {
    return this.isLoggedIn;
  }
}
