// --- src/app/features/estudiantes/components/estudiante-filter/estudiante-filter.component.ts ---
import { Component, EventEmitter, Output, OnInit } from '@angular/core'; // Agregamos OnInit
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms'; // Agregamos FormGroup
import { CommonModule } from '@angular/common';
import { EstudianteFiltros } from '../../models/estudiante.model';
import { debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-estudiante-filter',
  standalone: true,
  imports: [
    CommonModule, 
    ReactiveFormsModule
  ],
  templateUrl: './estudiante-filter.component.html'
})
// Implementar OnInit no es estrictamente necesario si se hace todo en el constructor, 
// pero ayuda a estructurar.
export class EstudianteFilterComponent implements OnInit { 
  @Output() filterChange = new EventEmitter<EstudianteFiltros>();

  carreras = [
    { id: 1, nombre: 'Ing. en Software' },
    { id: 2, nombre: 'Lic. en Nutrición' }
  ];

  // 1. Declarar la propiedad sin inicializarla de inmediato
  filterForm!: FormGroup; 

  constructor(private fb: FormBuilder) {
    // 2. Inicializar el formulario DENTRO del constructor
    this.filterForm = this.fb.group({
        carreraId: [''],
        semestre: ['']
    });

    // 3. Suscribirse a los cambios, usando el formulario ya inicializado
    this.filterForm.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(values => {
      const filtros: EstudianteFiltros = {
        carreraId: values.carreraId ? Number(values.carreraId) : undefined,
        semestre: values.semestre ? Number(values.semestre) : undefined
      };
      this.filterChange.emit(filtros);
    });
  }
  
  // Puedes dejar el ngOnInit vacío o eliminarlo si solo usas el constructor
  ngOnInit(): void {
    
  }
}