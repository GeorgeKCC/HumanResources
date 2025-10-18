import { HttpClient } from '@angular/common/http';
import { inject, Injectable, Signal, signal, WritableSignal } from '@angular/core';
import { LoginRequestModel } from '../models/login-request.model';
import { GenericResponse } from '@/shared/response/generic-response.model';
import { LoginResponseModel } from '../models/login-response.model';
import { Router } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { lastValueFrom } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class LoginService {
    private httpClient = inject(HttpClient);
    private router = inject(Router);

    isLoggin$: WritableSignal<boolean> = signal(false);
    isLoading = signal(false);
    hasError = signal(false);

    login(data: LoginRequestModel) {
        this.isLoading.set(true);
        return this.httpClient.post<GenericResponse<LoginResponseModel>>(`${environment.apiUrl}/login`, data, { withCredentials: true }).subscribe({
            next: (response) => {
                if (response.isSuccess) {
                    this.isLoading.set(false);
                    this.isLoggin$.set(true);
                    this.router.navigate(['general']);
                }
            },
            error: (error) => {
                this.isLoading.set(false);
                this.isLoggin$.set(false);
                this.hasError.set(true);
                console.error('Login failed', error.message);
            }
        });
    }

    async authStatus(): Promise<boolean> {
        try {
            const response = await lastValueFrom(this.httpClient.post<GenericResponse<LoginResponseModel>>(`${environment.apiUrl}/login/auth-status`, {}, { withCredentials: true }));
            return response.isSuccess;
        } catch (error) {
            console.error('Checking auth status', error);
            return false;
        }
    }
}
