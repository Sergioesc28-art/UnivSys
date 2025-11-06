import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { LoginComponent } from './features/auth/pages/login/login.component';
import { RegisterComponent } from './features/auth/pages/register/register.component'; // ¡Nueva Importación!

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'estudiantes',
    loadChildren: () => import('./features/estudiantes/estudiantes.routes')
                            .then(m => m.ESTUDIANTES_ROUTES),
    canActivate: [authGuard] 
  },
  { path: '', redirectTo: '/estudiantes', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' }
];