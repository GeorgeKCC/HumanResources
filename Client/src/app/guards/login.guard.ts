import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { LoginService } from "../pages/auth/services/login.service";

export const loginGuard: CanActivateFn = async() => {
   const loginService = inject(LoginService);
   const router = inject(Router);
   
   if(loginService.isLoggedIn()) {
      return true;
   }

   router.navigate(['/login']);
   return false;
}