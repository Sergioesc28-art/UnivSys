// --- src/app/features/estudiantes/pages/estudiante-create/estudiante-create.component.ts ---
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { EstudianteService } from '../../../estudiantes/services/estudiantes.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { EstudianteRegistroDTO } from '../../models/estudiante.model';
import { EstudianteFormComponent } from '../../components/estudiante-form/estudiante-form.component';

@Component({
  selector: 'app-estudiante-create',
  standalone: true,
  imports: [
    EstudianteFormComponent // Importamos el componente de formulario
  ],
  templateUrl: './estudiante-create.component.html'
})
export class EstudianteCreateComponent {

  constructor(
    private estudianteService: EstudianteService,
    private router: Router,
    private notification: NotificationService
  ) {}

  onSave(dto: EstudianteRegistroDTO): void {
    this.estudianteService.createEstudiante(dto).subscribe({
      next: (estudianteCreado) => {
        this.notification.showSuccess(`Estudiante ${estudianteCreado.nombres} registrado.`);
        this.router.navigate(['/estudiantes']);
      },
      error: (err) => {
        if (err.status !== 409) {
          this.notification.showError('Error desconocido al crear.');
        }
      }
    });
  }
}