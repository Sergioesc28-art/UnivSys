using UnivSys.API.Models;

namespace UnivSys.API.Core.Factories
{
    // La Interfaz 'Producto' (Define las operaciones comunes)
    public interface IUsuario
    {
        // Propiedad que permite acceder al modelo de la BD
        Estudiante EstudianteData { get; set; }
        
        // Método para ejecutar validaciones específicas del tipo de estudiante
        // Devuelve una lista de strings con los errores encontrados (si los hay)
        List<string> ValidarDatosEspecificos();

        // Método para realizar acciones específicas (Ej. cálculo de beca)
        void AplicarLogica();
    }
}