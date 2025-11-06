import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { NotificationService } from '../services/notification.service';
import { ValidationError } from '../../Shared/models/validation-error.model';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const notification = inject(NotificationService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      
      switch (error.status) {
        case 400:
          const validationErrors = error.error as ValidationError;
          if (validationErrors && validationErrors.errors) {
            const errorMessages = Object.values(validationErrors.errors).flat();
            notification.showError(`Error de validación: ${errorMessages.join(', ')}`);
          } else {
            notification.showError(error.error?.message || 'Petición incorrecta.');
          }
          break;
        case 401:
          notification.showError('Sesión expirada. Por favor, inicie sesión.');
          authService.logout();
          router.navigate(['/login']);
          break;
        case 403:
          notification.showError('No tiene permisos para realizar esta acción.');
          break;
        case 404:
          notification.showError('El recurso solicitado no se encontró.');
          break;
        case 409:
          notification.showError(error.error?.message || 'Conflicto: El recurso ya existe (ej. ID duplicado).');
          break;
        case 500:
          notification.showError('Error interno del servidor. Contacte al administrador.');
          break;
        default:
          notification.showError(`Error ${error.status}: ${error.message}`);
      }
      return throwError(() => error);
    })
  );
};