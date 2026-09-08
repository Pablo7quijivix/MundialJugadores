using MundialJugadores.Estructuras.BPlusTree;

namespace MundialJugadores.Modulos
{
    /// <summary>
    /// Módulo de recorrido general del Árbol B+.
    /// CONSTRUCCIÓN: clase estática dedicada exclusivamente a mostrar el recorrido
    /// in-order de TODOS los jugadores, separada del Módulo de Imprimir (que se
    /// encarga de reportes más elaborados como Top 5 y listados ordenados por
    /// categoría usando los heaps).
    /// ACCIÓN QUE REALIZA: invoca ArbolBMas.RecorrerInOrden() -que recorre las
    /// hojas enlazadas del B+ de izquierda a derecha- y muestra cada jugador en
    /// consola en el orden natural de su Id.
    /// DATOS QUE MANIPULA: la lista de Jugador devuelta por el recorrido del árbol.
    /// POR QUÉ ESTÁ AQUÍ: en Modulos/, como archivo específico para la operación
    /// de "recorrer", según la organización modular solicitada.
    /// </summary>
    public static class ModuloRecorrer
    {
        public static void MostrarRecorridoCompleto(ArbolBMas arbol)
        {
            var jugadores = arbol.RecorrerInOrden();

            Console.WriteLine($"\n--- Recorrido completo del Árbol B+ (ordenado por Id) — {jugadores.Count} jugador(es) ---");
            if (jugadores.Count == 0)
            {
                Console.WriteLine("(No hay jugadores registrados)");
                return;
            }

            foreach (var jugador in jugadores)
            {
                Console.WriteLine(jugador);
            }
        }
    }
}
