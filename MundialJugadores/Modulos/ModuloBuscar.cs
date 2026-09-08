using MundialJugadores.Estructuras.BPlusTree;

namespace MundialJugadores.Modulos
{
    /// <summary>
    /// Módulo de búsqueda de un jugador específico.
    /// CONSTRUCCIÓN: clase estática enfocada solo en la interacción de consola para
    /// pedir el Id a buscar y mostrar el resultado.
    /// ACCIÓN QUE REALIZA: delega la búsqueda real al método Buscar(clave) del
    /// Árbol B+ (O(log n)) y presenta el resultado formateado en consola.
    /// DATOS QUE MANIPULA: el Id ingresado por consola y el ArbolBMas consultado.
    /// POR QUÉ ESTÁ AQUÍ: en Modulos/, siguiendo la organización modular pedida:
    /// un archivo específico solo para la operación de búsqueda.
    /// </summary>
    public static class ModuloBuscar
    {
        public static void BuscarDesdeConsola(ArbolBMas arbol)
        {
            Console.Write("\nIngrese el Id del jugador a buscar: ");
            string? id = Console.ReadLine()?.Trim() ?? string.Empty;

            var jugador = arbol.Buscar(id);
            if (jugador == null)
            {
                Console.WriteLine($"No se encontró ningún jugador con Id '{id}'.");
            }
            else
            {
                Console.WriteLine("\nJugador encontrado:");
                Console.WriteLine(jugador);
            }
        }
    }
}
