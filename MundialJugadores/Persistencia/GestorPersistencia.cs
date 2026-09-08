using MundialJugadores.Modelos;

namespace MundialJugadores.Persistencia
{
    /// <summary>
    /// Gestor de persistencia de datos.
    /// CONSTRUCCIÓN: clase estática de utilería (no guarda estado propio), pensada
    /// para ser llamada desde los módulos de Insertar/Eliminar/Actualizar cada vez
    /// que el Árbol B+ cambia.
    /// ACCIONES QUE REALIZA: reescribe por completo los archivos .csv y .txt a partir
    /// del recorrido ordenado actual del Árbol B+ (fuente de verdad en memoria). Esto
    /// garantiza que el archivo en disco siempre refleje el estado real del sistema
    /// (persistencia en tiempo real: "cada proceso" deja los archivos actualizados).
    /// DATOS QUE MANIPULA: la lista de Jugador que entrega ArbolBMas.RecorrerInOrden()
    /// y las rutas físicas de los archivos jugadores.csv / jugadores.txt.
    /// POR QUÉ ESTÁ AQUÍ: se separa en su propia carpeta "Persistencia" (organización
    /// modular) porque su responsabilidad (escribir a disco) es independiente de la
    /// lógica de estructuras de datos y de la lógica de menú.
    /// </summary>
    public static class GestorPersistencia
    {
        public const string RutaCsv = "Datos/jugadores.csv";
        public const string RutaTxt = "Datos/jugadores.txt";

        private const string EncabezadoCsv = "Id,Nombre,Seleccion,Posicion,MinutosJugados,Goles,Asistencias,TarjetasAmarillas,TarjetasRojas,PartidosJugados";

        /// <summary>
        /// Vuelve a escribir jugadores.csv y jugadores.txt completos con el estado
        /// actual del árbol. Se invoca después de cada inserción, actualización o
        /// eliminación exitosa para mantener persistencia en tiempo real.
        /// </summary>
        public static void GuardarTodo(List<Jugador> jugadoresOrdenados)
        {
            GuardarCsv(jugadoresOrdenados);
            GuardarTxt(jugadoresOrdenados);
        }

        private static void GuardarCsv(List<Jugador> jugadores)
        {
            using StreamWriter escritor = new StreamWriter(RutaCsv, append: false);
            escritor.WriteLine(EncabezadoCsv);
            foreach (Jugador j in jugadores)
            {
                escritor.WriteLine(j.AFilaCsv());
            }
        }

        private static void GuardarTxt(List<Jugador> jugadores)
        {
            using StreamWriter escritor = new StreamWriter(RutaTxt, append: false);
            foreach (Jugador j in jugadores)
            {
                escritor.WriteLine(j.AFilaTxt());
            }
        }
    }
}
