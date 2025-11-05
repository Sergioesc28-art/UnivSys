using UnivSys.API.Models;

namespace UnivSys.API.Core.Factories
{
    // El Creador (Clase Estática con el Factory Method)
    public static class UsuarioFactory
    {
        public static IUsuario CrearInstancia(Estudiante estudiante)
        {
            if (estudiante.EsBecado && !estudiante.EsEgresado)
            {
                // Es un becado
                return new EstudianteBecado(estudiante);
            }
            else if (estudiante.EsEgresado && !estudiante.EsBecado)
            {
                // Es un egresado
                return new EstudianteEgresado(estudiante);
            }
            else
            {
                // Es un estudiante regular (o falla la validación de exclusividad, que se revisa antes)
                return new EstudianteRegular(estudiante);
            }
        }
    }
}