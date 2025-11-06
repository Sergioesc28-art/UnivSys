// --- src/app/features/estudiantes/pages/estudiante-edit/estudiante-edit.component.ts ---
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { EstudianteService } from '../../../estudiantes/services/estudiantes.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { EstudianteDetalleDTO, EstudianteRegistroDTO } from '../../models/estudiante.model';
import { EstudianteFormComponent } from '../../components/estudiante-form/estudiante-form.component';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common'; // Para *ngIf, async

@Component({
  selector: 'app-estudiante-edit',
  standalone: true,
  imports: [
    CommonModule, 
    EstudianteFormComponent
  ],
  templateUrl: './estudiante-edit.component.html'
})
export class EstudianteEditComponent implements OnInit {
  
  @Input() idEstudiante!: string;
  estudiante$!: Observable<EstudianteDetalleDTO>;

  constructor(
    private estudianteService: EstudianteService,
    private router: Router,
    private notification: NotificationService
  ) {}

  ngOnInit(): void {
    if (!this.idEstudiante) {
      this.router.navigate(['/estudiantes']);
      return;
    }
    this.estudiante$ = this.estudianteService.getEstudianteById(this.idEstudiante);
  }

  onSave(dto: EstudianteRegistroDTO): void {
    this.estudianteService.updateEstudiante(this.idEstudiante, dto).subscribe({
      next: (estudianteActualizado) => {
        this.notification.showSuccess(`Estudiante ${estudianteActualizado.nombres} actualizado.`);
        this.router.navigate(['/estudiantes', this.idEstudiante]); 
      },
    });
  }
}