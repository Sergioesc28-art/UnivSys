// --- src/app/features/auth/pages/login/login.component.ts ---
import { Component, OnInit } from '@angular/core'; // Añadimos OnInit (opcional, pero buena práctica)
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms'; // Añadimos FormGroup
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule, 
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
// Implementamos OnInit para inicializar el formulario si queremos separarlo del constructor
export class LoginComponent implements OnInit { 
    
  // 1. Declarar la propiedad, pero sin inicializarla inmediatamente.
  // Usamos '!' (Non-null assertion operator) porque sabemos que se inicializará en ngOnInit.
  loginForm!: FormGroup; 

  // Inyectamos el FormBuilder. TypeScript lo reconoce como propiedad de la clase.
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private notification: NotificationService
  ) {
    // Nota: Aunque es válido inicializarlo aquí, se recomienda ngOnInit para lógica compleja.
  }

  ngOnInit(): void {
    // 2. Inicializar la propiedad *dentro* de un ciclo de vida, donde 'this.fb' ya está disponible.
    this.loginForm = this.fb.group({
        username: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.notification.showError('Por favor, complete el formulario.');
      this.loginForm.markAllAsTouched();
      return;
    }

    // Usamos .value para obtener los datos del formulario
    const credentials = this.loginForm.value as any; 

    this.authService.login(credentials).subscribe({
      next: (response) => {
        this.notification.showSuccess('¡Inicio de sesión exitoso!');
        this.router.navigate(['/estudiantes']);
      },
      error: (err: any) => {
        if (err.status !== 401) {
          this.notification.showError('Usuario o contraseña incorrectos.');
        }
      }
    });
  }

  get f() { 
    return this.loginForm.controls as any; 
  }
}