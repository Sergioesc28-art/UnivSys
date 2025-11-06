// --- src/app/features/estudiantes/estudiantes.routes.ts ---
import { Routes } from '@angular/router';
import { EstudianteListComponent } from './pages/estudiante-list/estudiante-list.component';
import { EstudianteCreateComponent } from './pages/estudiante-create/estudiante-create.component';
import { EstudianteDetailComponent } from './pages/estudiante-detail/estudiante-detail.component';
import { EstudianteEditComponent } from './pages/estudiante-edit/estudiante-edit.component';
import { roleGuard } from '../../core/guards/role.guard';

export const ESTUDIANTES_ROUTES: Routes = [
  { 
    path: '', 
    component: EstudianteListComponent,
  },
  { path: 'nuevo', component: EstudianteCreateComponent },
  { path: ':idEstudiante', component: EstudianteDetailComponent },
  { path: ':idEstudiante/editar', component: EstudianteEditComponent }
];