using MundialJugadores.Estructuras.BPlusTree;
using MundialJugadores.Modelos;
using MundialJugadores.Persistencia;

namespace MundialJugadores.Modulos
{
    /// <summary>
    /// Módulo de inserción manual de jugadores desde consola.
    /// CONSTRUCCIÓN: clase estática enfocada únicamente en la interacción de
    /// consola para capturar un nuevo Jugador y en delegar su inserción al árbol.
    /// ACCIÓN QUE REALIZA: pide por consola cada campo (validando que los campos
    /// numéricos realmente sean números y que el Id no esté vacío ni repetido),
    /// arma un objeto Jugador, lo inserta en el Árbol B+ y de inmediato reescribe
    /// los archivos .csv/.txt para dejar persistido el cambio en tiempo real.
    /// DATOS QUE MANIPULA: entradas de consola (Console.ReadLine) y el ArbolBMas.
    /// POR QUÉ ESTÁ AQUÍ: en Modulos/, junto a Buscar/Eliminar/Imprimir/Recorrer,
    /// siguiendo la organización modular pedida (un archivo por operación del CRUD).
    /// </summary>
    public static class ModuloInsertar
    {
        public static void InsertarDesdeConsola(ArbolBMas arbol)
        {
            Console.WriteLine("\n--- Registrar nuevo jugador ---");

            string id = LeerTexto("Id (único, ej. J001): ");
            if (arbol.Buscar(id) != null)
            {
                Console.WriteLine($"Ya existe un jugador con Id '{id}'. Use la opción de actualizar/eliminar primero.");
                return;
            }

            string nombre = LeerTexto("Nombre: ");
            string seleccion = LeerTexto("Selección: ");
            string posicion = LeerTexto("Posición: ");
            int minutos = LeerEntero("Minutos jugados: ");
            int goles = LeerEntero("Goles: ");
            int asistencias = LeerEntero("Asistencias: ");
            int amarillas = LeerEntero("Tarjetas amarillas: ");
            int rojas = LeerEntero("Tarjetas rojas: ");
            int partidos = LeerEntero("Partidos disputados: ");

            Jugador jugador = new Jugador(id, nombre, seleccion, posicion, minutos, goles,
                                           asistencias, amarillas, rojas, partidos);

            arbol.Insertar(jugador);
            GestorPersistencia.GuardarTodo(arbol.RecorrerInOrden());

            Console.WriteLine($"Jugador '{nombre}' insertado y guardado correctamente.");
        }

        private static string LeerTexto(string mensaje)
        {
            Console.Write(mensaje);
            string? valor = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(valor))
            {
                Console.Write("El valor no puede estar vacío. " + mensaje);
                valor = Console.ReadLine();
            }
            return valor.Trim();
        }

        private static int LeerEntero(string mensaje)
        {
            Console.Write(mensaje);
            string? valor = Console.ReadLine();
            while (!int.TryParse(valor, out int numero) || numero < 0)
            {
                Console.Write("Ingrese un número entero válido (>= 0). " + mensaje);
                valor = Console.ReadLine();
            }
            return int.Parse(valor!);
        }
    }
}
