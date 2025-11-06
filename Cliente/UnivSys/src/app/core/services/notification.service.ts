import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  showSuccess(message: string): void {
    console.log(`%c[ÉXITO] ${message}`, 'color: green; font-weight: bold;');
  }
  showError(message: string): void {
    console.error(`[ERROR] ${message}`);
  }
  showInfo(message: string): void {
    console.info(`[INFO] ${message}`);
  }
}