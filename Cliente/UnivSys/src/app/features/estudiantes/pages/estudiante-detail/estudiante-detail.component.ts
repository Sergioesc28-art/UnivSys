// --- src/app/features/estudiantes/pages/estudiante-detail/estudiante-detail.component.ts ---
import { Component, Input, OnInit } from '@angular/core';
import { EstudianteService } from '../../../estudiantes/services/estudiantes.service';
import { EstudianteDetalleDTO } from '../../models/estudiante.model';
import { Observable } from 'rxjs'; // Mantener el Observable para el template
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-estudiante-detail',
  standalone: true,
  imports: [
    CommonModule, 
    RouterLink
  ],
  templateUrl: './estudiante-detail.component.html'
})
export class EstudianteDetailComponent implements OnInit {

  @Input() idEstudiante!: string;
  estudiante$!: Observable<EstudianteDetalleDTO>;

  constructor(private estudianteService: EstudianteService) {}

  ngOnInit(): void {
    // 1. Asignar el Observable al componente (para usarlo en el template con el pipe async)
    this.estudiante$ = this.estudianteService.getEstudianteById(this.idEstudiante);
    
    // 2. Suscribirse para ver la respuesta en la consola (DEBUGGING)
    this.estudiante$.subscribe({
      next: (data) => {
        // Se ejecuta cuando la API devuelve una respuesta exitosa
        console.log('✅ Datos del Estudiante recibidos:', data);
      },
      error: (err) => {
        // Se ejecuta si hay un error en la petición HTTP
        console.error('❌ Error al obtener el Estudiante:', err);
      },
      complete: () => {
        // Se ejecuta cuando el Observable se completa
        console.log('🏁 Suscripción de Estudiante completada.');
      }
    });
  }
}