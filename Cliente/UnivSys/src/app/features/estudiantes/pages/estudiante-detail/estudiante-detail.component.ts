// --- src/app/features/estudiantes/pages/estudiante-detail/estudiante-detail.component.ts ---
import { Component, Input, OnInit } from '@angular/core';
import { EstudianteService } from '../../../estudiantes/services/estudiantes.service';
import { EstudianteDetalleDTO } from '../../models/estudiante.model';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common'; // Para *ngIf, async
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
    this.estudiante$ = this.estudianteService.getEstudianteById(this.idEstudiante);
  }
}