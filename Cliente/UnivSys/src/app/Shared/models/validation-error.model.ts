// --- src/app/shared/models/validation-error.model.ts ---
export interface ValidationError {
  type: string;
  title: string;
  status: number;
  traceId: string;
  errors: {
    [key: string]: string[];
  };
}