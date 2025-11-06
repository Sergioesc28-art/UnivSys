// --- src/app/core/services/auth.service.ts ---
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, of, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { 
  LoginRequestDTO, 
  LoginResponseDTO,
  RegistroRequestDTO 
} from '../models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`; 
  private readonly TOKEN_KEY = 'jwt_token';
  private readonly ROL_KEY = 'user_rol';

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasToken());
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  private hasToken(): boolean {
    return !!localStorage.getItem(this.TOKEN_KEY);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  getRol(): string | null {
    return localStorage.getItem(this.ROL_KEY);
  }

  isAuthenticated(): boolean {
    return this.hasToken();
  }

  /**
   * POST /api/auth/register
   */
  register(dto: RegistroRequestDTO): Observable<{ mensaje: string }> {
    return this.http.post<{ mensaje: string }>(`${this.apiUrl}/register`, dto);
  }

  /**
   * POST /api/auth/login
   */
login(credentials: LoginRequestDTO): Observable<LoginResponseDTO> {
  return this.http.post<LoginResponseDTO>(`${this.apiUrl}/login`, credentials).pipe(
    // 1. TAPA DE DEPURACIÓN: Se ejecuta primero si la respuesta HTTP es 200 OK.
    // Esto garantiza que ves la respuesta siempre que la API sea accesible.
    tap(response => {
      console.log('✅ RESPUESTA DE LOGIN EXITOSA (Debugging):', response);
      // Nota: Si el error es 401 o 400, esta tapa NUNCA se ejecuta, 
      // y el error es manejado por el errorInterceptor.
    }),
    
    // 2. TAPA DE LÓGICA: Realiza la acción de guardar los datos.
    tap(response => {
      localStorage.setItem(this.TOKEN_KEY, response.token);
      localStorage.setItem(this.ROL_KEY, response.rol);
      this.isAuthenticatedSubject.next(true);
    })
  );
}

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.ROL_KEY); 
    this.isAuthenticatedSubject.next(false);
    this.router.navigate(['/login']);
  }
}