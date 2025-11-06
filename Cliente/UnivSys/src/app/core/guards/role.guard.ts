// --- src/app/core/guards/role.guard.ts ---
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { NotificationService } from '../services/notification.service';

/**
 * Guardia funcional que verifica si el rol del usuario está incluido en la lista de roles permitidos.
 * * Uso: canActivate: [roleGuard(['Director', 'Admin'])]
 */
export const roleGuard = (allowedRoles: string[]): CanActivateFn => {
  return (route, state) => {
    const authService = inject(AuthService);
    const router = inject(Router);
    const notification = inject(NotificationService);
    
    const userRol = authService.getRol();

    if (!userRol) {
      // Si no hay rol (posiblemente token inválido), forzamos login
      router.navigate(['/login']);
      return false;
    }

    if (allowedRoles.includes(userRol)) {
      return true; // Rol permitido
    } else {
      // Rol válido pero insuficiente (403 Forbidden)
      notification.showError('Acceso denegado. Su rol (' + userRol + ') no tiene permiso para esta acción.');
      router.navigate(['/estudiantes']); // O a otra página de inicio
      return false;
    }
  };
};