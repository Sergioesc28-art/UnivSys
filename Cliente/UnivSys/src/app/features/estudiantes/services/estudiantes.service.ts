// --- src/app/features/estudiantes/services/estudiante.service.ts ---
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { 
  EstudianteDetalleDTO, 
  EstudianteFiltros, 
  EstudianteRegistroDTO 
} from '../models/estudiante.model';

@Injectable({
  providedIn: 'root'
})
export class EstudianteService {
  private apiUrl = `${environment.apiUrl}/estudiantes`;

  constructor(private http: HttpClient) { }

  getEstudiantes(filtros: EstudianteFiltros = {}): Observable<EstudianteDetalleDTO[]> {
    let params = new HttpParams();
    if (filtros.carreraId) {
      params = params.set('carreraId', filtros.carreraId.toString());
    }
    if (filtros.semestre) {
      params = params.set('semestre', filtros.semestre.toString());
    }
    return this.http.get<EstudianteDetalleDTO[]>(this.apiUrl, { params });
  }

  getEstudianteById(idEstudiante: string): Observable<EstudianteDetalleDTO> {
    return this.http.get<EstudianteDetalleDTO>(`${this.apiUrl}/${idEstudiante}`);
  }

  createEstudiante(dto: EstudianteRegistroDTO): Observable<EstudianteDetalleDTO> {
    return this.http.post<EstudianteDetalleDTO>(this.apiUrl, dto);
  }

  updateEstudiante(idEstudiante: string, dto: EstudianteRegistroDTO): Observable<EstudianteDetalleDTO> {
    return this.http.put<EstudianteDetalleDTO>(`${this.apiUrl}/${idEstudiante}`, dto);
  }

  deleteEstudiante(idEstudiante: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${idEstudiante}`);
  }
}