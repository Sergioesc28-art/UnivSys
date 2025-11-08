// --- src/app/features/estudiantes/components/estudiante-form/estudiante-form.component.ts ---
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { EstudianteRegistroDTO } from '../../models/estudiante.model';
import { RouterLink } from '@angular/router';
import { factoryFlagValidator } from './factory-flag.validator'; 

@Component({
  selector: 'app-estudiante-form',
  standalone: true,
  imports: [
    CommonModule, 
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './estudiante-form.component.html',
  styleUrl: './estudiante-form.component.css'
})
export class EstudianteFormComponent implements OnInit, OnChanges {
  
  @Input() initialData: EstudianteRegistroDTO | null | undefined = null;
  @Input() isEditMode = false;
  @Output() save = new EventEmitter<EstudianteRegistroDTO>();
  
  estudianteForm!: FormGroup;
  carreras = [
    { id: 1, nombre: 'Ingeniería de Software' },
    { id: 2, nombre: 'Medicina' },
    { id: 3, nombre: 'Derecho' },
    { id: 4, nombre: 'Administración de Empresas' },
    { id: 5, nombre: 'Arquitectura' },
    { id: 6, nombre: 'Psicología' },
    { id: 7, nombre: 'Marketing Digital' },
    { id: 8, nombre: 'Contaduría Pública' },
    { id: 9, nombre: 'Diseño Gráfico' },
    { id: 10, nombre: 'Relaciones Internacionales' }
  ];
  semestres: number[] = Array.from({length: 15}, (_, i) => i + 1);

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initForm();
    this.setupConditionalValidation();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['initialData'] && this.estudianteForm && this.initialData) {
      this.estudianteForm.patchValue(this.initialData);
      if(this.isEditMode) {
        this.f['idEstudiante'].disable();
      }
    }
  }

  private initForm(): void {
    this.estudianteForm = this.fb.group({
      idEstudiante: ['', [Validators.required, Validators.maxLength(20)]],
      nombres: ['', [Validators.required, Validators.pattern('^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]*$')]],
      apellidoPaterno: ['', [Validators.required, Validators.pattern('^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]*$')]],
      apellidoMaterno: ['', [Validators.required, Validators.pattern('^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]*$')]],
      semestre: [null, [Validators.required, Validators.min(1), Validators.max(15)]],
      carreraID: [null, [Validators.required]],
      esBecado: [false],
      esEgresado: [false],
      porcentajeBeca: [null as (number | null)],
      fechaEgreso: ['']
    }, {
      validators: [factoryFlagValidator]
    });

    if (this.initialData) {
      this.estudianteForm.patchValue(this.initialData);
      if(this.isEditMode) {
        this.f['idEstudiante'].disable();
      }
      if (this.initialData.fechaEgreso) {
        const fecha = new Date(this.initialData.fechaEgreso);
        const formattedDate = fecha.toISOString().substring(0, 10);
        this.estudianteForm.controls['fechaEgreso'].setValue(formattedDate);
      }
    }
  }

  private setupConditionalValidation(): void {
    this.f['esBecado'].valueChanges.subscribe(isBecado => {
      if (isBecado) {
        this.f['porcentajeBeca'].setValidators([Validators.required, Validators.min(1), Validators.max(100)]);
      } else {
        this.f['porcentajeBeca'].clearValidators();
        this.f['porcentajeBeca'].setValue(null); // Limpiar valor
      }
      this.f['porcentajeBeca'].updateValueAndValidity();
    });

    this.f['esEgresado'].valueChanges.subscribe(isEgresado => {
      if (isEgresado) {
        this.f['fechaEgreso'].setValidators([Validators.required]);
      } else {
        this.f['fechaEgreso'].clearValidators();
        this.f['fechaEgreso'].setValue(''); // Limpiar valor
      }
      this.f['fechaEgreso'].updateValueAndValidity();
    });
  }

  get f() { return this.estudianteForm.controls; }

  onSubmit(): void {
    if (this.estudianteForm.invalid) {
      this.estudianteForm.markAllAsTouched();
      return;
    }
    const dto: EstudianteRegistroDTO = this.estudianteForm.getRawValue();
    this.save.emit(dto);
  }

  // Helper para clases de Tailwind
  inputClass(controlName: string): string {
    const control = this.f[controlName];
    return control.touched && control.invalid ? 'border-red-500 focus:border-red-500 focus:ring-red-500' : 'border-gray-300 focus:border-blue-500 focus:ring-blue-500';
  }
}