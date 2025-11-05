using UnivSys.API.Models;

namespace UnivSys.API.Core.Factories
{
    // Producto Concreto
    public class EstudianteEgresado : IUsuario
    {
        public Estudiante EstudianteData { get; set; }

        public EstudianteEgresado(Estudiante estudiante)
        {
            EstudianteData = estudiante;
        }

        public List<string> ValidarDatosEspecificos()
        {
            var errores = new List<string>();

            // 1. Validar que exista el detalle de egreso
            if (EstudianteData.DetalleEgresado == null)
            {
                errores.Add("El estudiante es Egresado, pero no se proporcionó la Fecha de Egreso.");
                return errores;
            }

            // 2. Restricción: La fecha de egreso no puede ser futura
            if (EstudianteData.DetalleEgresado.FechaEgreso > DateTime.Today)
            {
                errores.Add("Restricción: La Fecha de Egreso no puede ser una fecha futura.");
            }
            
            return errores;
        }

        public void AplicarLogica()
        {
            // Lógica: Podría ser deshabilitar el acceso al campus o generar un diploma (ejemplo)
            // Console.WriteLine("Lógica Aplicada: Acceso al campus deshabilitado para Egresados.");
        }
    }
}