import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { LoginService } from '../../pages/auth/services/login.service';
import { from, switchMap, catchError } from 'rxjs';

export const xsrfInterceptor: HttpInterceptorFn = (req, next) => {
  const document = inject(DOCUMENT);
  const loginService = inject(LoginService);

  const headerName = 'X-XSRF-TOKEN';

  const methodsToApply = ['POST', 'PUT', 'PATCH', 'DELETE'];
  if (!methodsToApply.includes(req.method)) {
    const clonedGet = req.clone({
      withCredentials: true,
    });
    return next(clonedGet);
  }

  if (!loginService.isLoggedIn()) {
    const clonedLogin = req.clone({
      withCredentials: true,
    });
    return next(clonedLogin);
  }

  return from(loginService.getTokenCsrf()).pipe(
    switchMap((xsrfValue) => {
      if (xsrfValue && !req.headers.has(headerName)) {
        const clonedReq = req.clone({
          withCredentials: true,
          headers: req.headers.set(headerName, xsrfValue),
        });
        return next(clonedReq);
      }
      return next(req);
    }),
    catchError((error) => {
      console.error('Error retrieving CSRF token:', error);
      return next(req);
    })
  );
};
