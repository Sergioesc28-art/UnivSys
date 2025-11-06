// --- src/app/features/estudiantes/components/estudiante-form/factory-flag.validator.ts ---
import { AbstractControl, FormGroup, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Validador personalizado para la regla de negocio del Factory Method:
 * Solo una de las banderas (EsBecado, EsEgresado) puede ser true.
 */
export const factoryFlagValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  const group = control as FormGroup;
  const esBecado = group.get('esBecado')?.value;
  const esEgresado = group.get('esEgresado')?.value;

  if (esBecado && esEgresado) {
    return { flagsExclusivas: true };
  }
  return null;
};