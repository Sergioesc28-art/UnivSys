// --- src/app/features/estudiantes/models/estudiante.model.ts ---
export interface EstudianteRegistroDTO {
  idEstudiante: string;
  nombres: string;
  apellidoPaterno: string;
  apellidoMaterno: string;
  semestre: number;
  carreraID: number;
  esBecado: boolean;
  esEgresado: boolean;
  porcentajeBeca?: number;
  fechaEgreso?: string;
}

export interface EstudianteDetalleDTO extends EstudianteRegistroDTO {
  nombreCarrera?: string;
  detalleBeca?: { porcentaje: number; };
  detalleEgreso?: { fechaTitulacion: Date; };
  tipoEstudiante: 'Regular' | 'Becado' | 'Egresado' | string;
  promedio: number | null;
  historialAcademico: HistorialAcademicoItem[];
}

export interface EstudianteFiltros {
  carreraId?: number;
  semestre?: number;
}

export interface HistorialAcademicoItem {
  idRegistro: number;
  materia: string;
  calificacion: number;
  periodo: string; // ej. "2018-01"
}