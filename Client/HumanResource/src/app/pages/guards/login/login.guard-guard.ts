import { LoginService } from '@/pages/auth/service/login.service';
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const loginGuardGuard: CanActivateFn = async (route, state) => {
  console.log('🔐 [GUARD DEBUG] Iniciando guard de login');
  console.log('🔐 [GUARD DEBUG] Route:', route);
  console.log('🔐 [GUARD DEBUG] State:', state);
  
  const loginService = inject(LoginService);
  const router = inject(Router);

  debugger; // ← Breakpoint garantizado
  console.log('🔐 [GUARD DEBUG] Verificando autenticación...');
  
  const isLoggin = await loginService.authStatus();
  console.log('🔐 [GUARD DEBUG] Estado auth:', isLoggin);
  
  if (!isLoggin) {
    console.log('🔐 [GUARD DEBUG] No autenticado, redirigiendo a /');
    router.navigate(['/']);
    return false;
  }
  
  console.log('🔐 [GUARD DEBUG] Autenticado, permitiendo acceso');
  return true;
};