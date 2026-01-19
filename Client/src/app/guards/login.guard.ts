import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { LoginService } from "../pages/auth/services/login.service";
import { SignalrService } from "../shared/services/signalr.service";

export const loginGuard: CanActivateFn = async() => {
   const loginService = inject(LoginService);
   const router = inject(Router);
   const signalrService = inject(SignalrService);
   
   if(loginService.isLoggedIn()) {
      if(!signalrService.isConnected()) {
         signalrService.createHubConnection();
      }
      return true;
   }

   signalrService.stopHubConnection();
   router.navigate(['/login']);
   return false;
}