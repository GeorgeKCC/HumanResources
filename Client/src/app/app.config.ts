import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
  inject,
  provideAppInitializer,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeuix/themes/aura';
import { MessageService } from 'primeng/api';
import { provideLottieOptions } from 'ngx-lottie';
import player from 'lottie-web';
import { provideHttpClient, withFetch, withInterceptors, withXsrfConfiguration } from '@angular/common/http';
import { LoginService } from './pages/auth/services/login.service';
import { xsrfInterceptor } from './shared/interceptors/xsrf.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes),
    provideClientHydration(withEventReplay()),
    provideAnimationsAsync(),
    providePrimeNG({ theme: { preset: Aura, options: { darkModeSelector: '.app-dark' } } }),
    provideLottieOptions({
      player: () => player,
    }),
    provideHttpClient(
      withFetch()
      // withInterceptors([
      //   xsrfInterceptor 
      // ])
    ),
    MessageService,
    provideAppInitializer(() => {
        const authService = inject(LoginService);
        if (typeof window !== 'undefined') {
            return authService.status();
        }
        return Promise.resolve();
    }),
  ],
};
