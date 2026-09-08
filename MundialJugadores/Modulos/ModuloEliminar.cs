using MundialJugadores.Estructuras.BPlusTree;
using MundialJugadores.Persistencia;

namespace MundialJugadores.Modulos
{
    /// <summary>
    /// Módulo de eliminación de un jugador.
    /// CONSTRUCCIÓN: clase estática enfocada solo en la interacción de consola para
    /// pedir el Id a eliminar y confirmar el resultado.
    /// ACCIÓN QUE REALIZA: delega el borrado real al método Eliminar(clave) del
    /// Árbol B+ (que redistribuye/fusiona nodos si hace falta para mantener el
    /// árbol balanceado) y, si la eliminación fue exitosa, reescribe de inmediato
    /// los archivos .csv/.txt para reflejar el cambio (persistencia en tiempo real).
    /// DATOS QUE MANIPULA: el Id ingresado por consola y el ArbolBMas afectado.
    /// POR QUÉ ESTÁ AQUÍ: en Modulos/, siguiendo la organización modular pedida:
    /// un archivo específico solo para la operación de eliminación.
    /// </summary>
    public static class ModuloEliminar
    {
        public static void EliminarDesdeConsola(ArbolBMas arbol)
        {
            Console.Write("\nIngrese el Id del jugador a eliminar: ");
            string? id = Console.ReadLine()?.Trim() ?? string.Empty;

            bool eliminado = arbol.Eliminar(id);
            if (!eliminado)
            {
                Console.WriteLine($"No se encontró ningún jugador con Id '{id}'.");
                return;
            }

            GestorPersistencia.GuardarTodo(arbol.RecorrerInOrden());
            Console.WriteLine($"Jugador con Id '{id}' eliminado y archivos actualizados correctamente.");
        }
    }
}
