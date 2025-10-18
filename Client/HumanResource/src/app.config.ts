import { provideHttpClient, withFetch } from '@angular/common/http';
import { ApplicationConfig, provideZoneChangeDetection, PLATFORM_ID, inject } from '@angular/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
import { isPlatformBrowser } from '@angular/common';
import { provideRouter, withHashLocation, withInMemoryScrolling } from '@angular/router';
import Aura from '@primeuix/themes/aura';
import { providePrimeNG } from 'primeng/config';
import { appRoutes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(appRoutes, withHashLocation()),
    provideClientHydration(withEventReplay()),
    provideHttpClient(withFetch()),
    // 🎨 Conditional animations for SSR compatibility
    typeof window !== 'undefined' ? provideAnimationsAsync() : provideNoopAnimations(),
    providePrimeNG({
      theme: {
        preset: Aura
      },
      // 🎨 SSR-safe configuration for inputs
      csp: {
        nonce: undefined
      },
      inputStyle: 'outlined',
      ripple: false // Disable ripple for SSR consistency
    })
  ]
};

// 🐛 Debug: Verificar configuración SSR
console.log('🚀 [CONFIG] App config cargado:', {
  hydration: true,
  eventReplay: true,
  hashLocation: true
});
