using UnivSys.API.Models;

namespace UnivSys.API.Core.Factories
{
    // Producto Concreto
    public class EstudianteRegular : IUsuario
    {
        public Estudiante EstudianteData { get; set; }

        public EstudianteRegular(Estudiante estudiante)
        {
            EstudianteData = estudiante;
        }

        public List<string> ValidarDatosEspecificos()
        {
            // Un estudiante regular no tiene validaciones específicas de beca o egreso.
            return new List<string>();
        }

        public void AplicarLogica()
        {
            // No se aplica lógica extra.
        }
    }
}