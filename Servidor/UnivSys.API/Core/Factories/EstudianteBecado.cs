using UnivSys.API.Models;

namespace UnivSys.API.Core.Factories
{
    // Producto Concreto
    public class EstudianteBecado : IUsuario
    {
        public Estudiante EstudianteData { get; set; }

        public EstudianteBecado(Estudiante estudiante)
        {
            EstudianteData = estudiante;
        }

        public List<string> ValidarDatosEspecificos()
        {
            var errores = new List<string>();
            
            // 1. Validar que exista el detalle de beca
            if (EstudianteData.DetalleBecado == null)
            {
                errores.Add("El estudiante es Becado, pero no se proporcionó el detalle del porcentaje de Beca.");
                return errores;
            }

            // 2. Restricción: Porcentaje de Beca entre 1 y 100
            if (EstudianteData.DetalleBecado.PorcentajeBeca < 1 || EstudianteData.DetalleBecado.PorcentajeBeca > 100)
            {
                errores.Add($"Restricción: El Porcentaje de Beca debe estar entre 1 y 100. Valor actual: {EstudianteData.DetalleBecado.PorcentajeBeca}.");
            }
            
            return errores;
        }

        public void AplicarLogica()
        {
            // Lógica: Podría ser registrar un evento o actualizar el promedio (ejemplo)
            // Console.WriteLine($"Lógica Aplicada: Descuento de {EstudianteData.DetalleBecado.PorcentajeBeca}% registrado.");
        }
    }
}