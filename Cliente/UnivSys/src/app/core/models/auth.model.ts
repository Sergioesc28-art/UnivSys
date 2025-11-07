export interface LoginRequestDTO {
  Username: string;
  Password: string;
}

export interface LoginResponseDTO {
  username: string;
  rol: 'Maestro' | 'Director' | 'Admin';
  token: string;
}

export interface RegistroRequestDTO {
  Username: string;
  Password: string;
  idRol: number;
}