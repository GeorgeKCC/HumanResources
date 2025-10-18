import { FormControl, FormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Component, effect, inject } from '@angular/core';
import { LoginService } from '../service/login.service';
import { MessageService } from 'primeng/api';
import { PrimeNgModule } from '@/shared/primeng/primeng.module';
import { LoginRequestModel } from '../models/login-request.model';
import { SsrSafeInputDirective } from '@/shared/directives/ssr-safe-input.directive';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [PrimeNgModule, FormsModule, ReactiveFormsModule, SsrSafeInputDirective],
    templateUrl: './login.component.html',
    providers: [MessageService]
})
export class Login {
    loginService = inject(LoginService);
    messageService = inject(MessageService);

    constructor() {
        effect(() => {
            const hasError = this.loginService.hasError();
            if (hasError) {
                this.messageService.add({ severity: 'error', summary: 'Sticky', detail: 'Login failed. Please check your credentials.', sticky: true });
            }
        });
    }

    formLogin = new FormGroup({
        username: new FormControl('', [Validators.email, Validators.required]),
        password: new FormControl('', [Validators.required])
    });

    login() {
        const loginRequest: LoginRequestModel = {
            username: this.formLogin.value.username!,
            password: this.formLogin.value.password!
        };
        this.loginService.login(loginRequest);
    }
}
