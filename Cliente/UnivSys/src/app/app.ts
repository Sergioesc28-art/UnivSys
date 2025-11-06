// --- src/app/app.component.ts ---
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './Shared/components/layout/header/header.component';
import { SidebarComponent } from './Shared/components/layout/sidebar/sidebar.component';
import { AuthService } from './core/services/auth.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  // Importamos los componentes de layout y módulos necesarios
  imports: [
    CommonModule, 
    RouterOutlet,
    HeaderComponent, // Standalone
    SidebarComponent // Standalone
  ],
  templateUrl: './app.html'
  // No hay 'styleUrl' ya que usamos Tailwind global
})
export class App {
  isAuthenticated$: Observable<boolean>;

  constructor(private authService: AuthService) {
    this.isAuthenticated$ = this.authService.isAuthenticated$;
  }
}