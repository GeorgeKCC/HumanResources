import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { LoginService } from '../../pages/auth/services/login.service';

export const xsrfInterceptor: HttpInterceptorFn = (req, next) => {
  const document = inject(DOCUMENT);
  const loginService = inject(LoginService);
  
  const cookieName = 'X-XSRF-TOKEN';
  const headerName = 'X-XSRF-TOKEN';
  
  const methodsToApply = ['POST', 'PUT', 'PATCH', 'DELETE'];
  if (!methodsToApply.includes(req.method)) {
    return next(req);
  }

  const xsrfValue = getCookieValue(document.cookie, cookieName);
  
  if (xsrfValue && !req.headers.has(headerName)) {
    const clonedReq = req.clone({
      headers: req.headers.set(headerName, xsrfValue),
    });
    return next(clonedReq);
  }

  return next(req);
};

function getCookieValue(cookieString: string, name: string): string | null {
  if (!cookieString || cookieString.trim() === '') {
    return null;
  }

  const nameEQ = name + '=';
  const ca = cookieString.split(';');
  
  for (let i = 0; i < ca.length; i++) {
    let c = ca[i].trimStart();
    
    if (c.startsWith(nameEQ)) {
      const value = c.substring(nameEQ.length, c.length);
      
      // Decodificar el valor si está URL encoded
      try {
        return decodeURIComponent(value);
      } catch (e) {
        return value;
      }
    }
  }
  
  return null;
}