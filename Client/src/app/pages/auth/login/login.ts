import { Component, inject, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AnimationOptions, LottieComponent } from 'ngx-lottie';
import { isPlatformBrowser } from '@angular/common';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  FormBuilder,
} from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from '../services/login.service';
import { LoginModel } from '../models/login.model';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, LottieComponent, CommonModule],
  providers: [],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
})
export class Login {
  router = inject(Router);
  loginService = inject(LoginService);
  formLogin: FormGroup;

  showPassword = false;
  lottieOptions?: AnimationOptions;

  constructor(@Inject(PLATFORM_ID) private platformId: object, private formBuilder: FormBuilder) {
    if (isPlatformBrowser(this.platformId)) {
      this.lottieOptions = {
        path: 'https://assets1.lottiefiles.com/packages/lf20_1pxqjqps.json',
        loop: true,
        autoplay: true,
      };
    }
    this.formLogin = this.formBuilder.group({
      username: new FormControl('', [Validators.email, Validators.required]),
      password: new FormControl('', [Validators.required]),
    });
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  async onSubmit() {
    const username = this.formLogin.get('username')?.value;
    const password = this.formLogin.get('password')?.value;

    const loginRequest: LoginModel = {
      username: username,
      password: password,
    };

    await this.loginService.login(loginRequest);
  }
}
