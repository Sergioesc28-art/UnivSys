// --- src/app/features/auth/pages/register/register.component.ts ---
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { CommonModule } from '@angular/common';
import { RegistroRequestDTO } from '../../../../core/models/auth.model';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule, 
    ReactiveFormsModule,
    RouterLink // Para el botón de "Ir a Login"
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  
  registerForm!: FormGroup;
  
  // Opciones de roles disponibles para registrar
  roles = [
    { id: 1, name: 'Maestro' },
    { id: 2, name: 'Director' },
    { id: 3, name: 'Admin' }
  ];

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private notification: NotificationService
  ) {}

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.email, Validators.minLength(5)]],
      password: ['', [Validators.required, Validators.minLength(8)]], // Ajustar longitud
      idRol: [null, [Validators.required]] // Usamos 'null' y Validators.required
    });
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.notification.showError('Por favor, complete el formulario correctamente.');
      this.registerForm.markAllAsTouched();
      return;
    }

    const dto = this.registerForm.value as RegistroRequestDTO;

    this.authService.register(dto).subscribe({
      next: (response) => {
        this.notification.showSuccess(response.mensaje || 'Usuario registrado exitosamente.');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        // Interceptor maneja 400 y 409
        console.error('Error de registro:', err);
      }
    });
  }

  get f() { return this.registerForm.controls; }
}