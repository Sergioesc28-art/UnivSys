// --- src/app/shared/components/layout/sidebar/sidebar.component.ts ---
import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common'; // Necesario para *ngFor

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    CommonModule, 
    RouterLink, 
    RouterLinkActive
  ],
  templateUrl: './sidebar.component.html'
})
export class SidebarComponent {
  navItems = [
    { label: 'Dashboard', link: '/estudiantes', icon: '🏠' },
    { label: 'Registrar', link: '/estudiantes/nuevo', icon: '➕' },
  ];
}