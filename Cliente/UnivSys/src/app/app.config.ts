// --- src/app/app.config.ts ---
import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { jwtInterceptor } from './core/interceptors/jwt.interceptor';
import { errorInterceptor } from './core/interceptors/error.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    
    // Configuración de rutas
    provideRouter(routes, withComponentInputBinding()), // Habilita binding de params de ruta a @Inputs

    // Configuración de HTTP y los interceptors funcionales
    provideHttpClient(
      withInterceptors([
        jwtInterceptor, 
        errorInterceptor
      ])
    ),
    
    // Los servicios en 'root' (como AuthService y NotificationService)
    // ya están proveídos y no necesitan listarse aquí.
  ]
};