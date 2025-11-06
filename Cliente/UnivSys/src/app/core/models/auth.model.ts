export interface LoginRequestDTO {
  username: string;
  password: string;
}

export interface LoginResponseDTO {
  username: string;
  rol: 'Maestro' | 'Director' | 'Admin';
  token: string;
}

export interface RegistroRequestDTO {
  username: string;
  password: string;
  idRol: number;
}