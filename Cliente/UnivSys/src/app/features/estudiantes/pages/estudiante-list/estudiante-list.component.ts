// --- src/app/features/estudiantes/pages/estudiante-list/estudiante-list.component.ts ---
import { Component, OnInit } from '@angular/core';
import { EstudianteService } from '../../../estudiantes/services/estudiantes.service';
import { EstudianteDetalleDTO, EstudianteFiltros } from '../../models/estudiante.model';
import { Observable, BehaviorSubject, switchMap, tap } from 'rxjs';
import { NotificationService } from '../../../../core/services/notification.service';
import { CommonModule } from '@angular/common'; // Para *ngIf, *ngFor, async
import { RouterLink } from '@angular/router';
import { EstudianteFilterComponent } from '../../components/estudiante-filter/estudiante-filter.component';
import { ConfirmDialogComponent } from '../../../../Shared/components/confirm-dialog/confirm-dialog.component';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-estudiante-list',
  standalone: true,
  imports: [
    CommonModule, 
    RouterLink,
    EstudianteFilterComponent, // Standalone
    ConfirmDialogComponent // Standalone
  ],
  templateUrl: './estudiante-list.component.html',
  styleUrl: './estudiante-list.component.css'
})
export class EstudianteListComponent implements OnInit {
  
  private refresh$ = new BehaviorSubject<EstudianteFiltros>({});
  estudiantes$!: Observable<EstudianteDetalleDTO[]>;

  // Estado para el diálogo de confirmación
  showConfirmDialog = false;
  estudianteAEliminar: EstudianteDetalleDTO | null = null;
  userRol: string | null = null;

  constructor(
    private estudianteService: EstudianteService,
    private notification: NotificationService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.userRol = this.authService.getRol();
    this.estudiantes$ = this.refresh$.pipe(
      switchMap(filtros => this.estudianteService.getEstudiantes(filtros)),
      // 2. Añade el 'tap' aquí para interceptar y mostrar la respuesta
      tap(datosEstudiantes => {
        console.log('Respuesta de /api/estudiantes:', datosEstudiantes);
        
        // (Opcional) Puedes verificar si los nombres vienen aquí
        if (datosEstudiantes && datosEstudiantes.length > 0) {
          console.log('Datos del primer estudiante:', datosEstudiantes[0].nombres);
        }
      })
    );
    console.log(this.estudiantes$)
  }

  onFilterChange(filtros: EstudianteFiltros): void {
    this.refresh$.next(filtros);
  }

  // Abre el diálogo
  askToDelete(estudiante: EstudianteDetalleDTO): void {
    this.estudianteAEliminar = estudiante;
    this.showConfirmDialog = true;
  }

  // Cierra el diálogo
  onCancelDelete(): void {
    this.estudianteAEliminar = null;
    this.showConfirmDialog = false;
  }

  // Confirma eliminación
  onConfirmDelete(): void {
    if (!this.estudianteAEliminar) return;

    this.estudianteService.deleteEstudiante(this.estudianteAEliminar.idEstudiante).subscribe({
      next: () => {
        this.notification.showSuccess('Estudiante eliminado exitosamente.');
        this.refresh$.next(this.refresh$.value);
        this.onCancelDelete(); // Cierra el diálogo
      },
      error: () => this.onCancelDelete() // Cierra el diálogo en caso de error
    });
  }

  // confirma eliminación por rol
  canDelete(): boolean {
    const rol = this.userRol;
    return rol === 'Director' || rol === 'Admin';
  }
}