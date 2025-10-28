import { Component, effect, inject, signal, PLATFORM_ID } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Toast } from 'primeng/toast';
import { LoginService } from './pages/auth/services/login.service';
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Toast],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('Human resources');
  private platformId = inject(PLATFORM_ID);
  private loginService = inject(LoginService);

  constructor() {
    effect(() => {
      if (!this.loginService.isLoading()) {
        if (isPlatformBrowser(this.platformId)) {
          const overlay = document.getElementById('app-loading-overlay');
          if (overlay) {
            overlay.style.opacity = '0';
            overlay.remove();
          }
        }
      }
    });
  }
}
