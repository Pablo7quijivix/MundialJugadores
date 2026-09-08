namespace MundialJugadores.Modelos
{
    /// <summary>
    /// Clase de dominio "Jugador".
    /// CONSTRUCCIÓN: es una clase POO simple (POCO) que representa el registro completo
    /// de un jugador del mundial, con sus datos personales/deportivos y sus estadísticas
    /// acumuladas de desempeño.
    /// ACCIONES QUE REALIZA: no contiene lógica de negocio (esa vive en los Módulos y
    /// Estructuras); su responsabilidad única es transportar y representar los datos de
    /// un jugador (principio de responsabilidad única de POO).
    /// DATOS QUE MANIPULA: Id (clave única usada como clave del Árbol B+), Nombre,
    /// Selección, Posición y las estadísticas: MinutosJugados, Goles, Asistencias,
    /// TarjetasAmarillas, TarjetasRojas y PartidosJugados.
    /// POR QUÉ ESTÁ AQUÍ: se coloca en la carpeta "Modelos" siguiendo la organización
    /// modular pedida, separando el "qué es un dato" de "qué se hace con el dato".
    /// </summary>
    public class Jugador
    {
        public string Id { get; set; }          // Clave única (usada como clave del Árbol B+)
        public string Nombre { get; set; }
        public string Seleccion { get; set; }
        public string Posicion { get; set; }
        public int MinutosJugados { get; set; }
        public int Goles { get; set; }
        public int Asistencias { get; set; }
        public int TarjetasAmarillas { get; set; }
        public int TarjetasRojas { get; set; }
        public int PartidosJugados { get; set; }

        public Jugador(string id, string nombre, string seleccion, string posicion,
                        int minutosJugados, int goles, int asistencias,
                        int tarjetasAmarillas, int tarjetasRojas, int partidosJugados)
        {
            Id = id;
            Nombre = nombre;
            Seleccion = seleccion;
            Posicion = posicion;
            MinutosJugados = minutosJugados;
            Goles = goles;
            Asistencias = asistencias;
            TarjetasAmarillas = tarjetasAmarillas;
            TarjetasRojas = tarjetasRojas;
            PartidosJugados = partidosJugados;
        }

        /// <summary>
        /// Devuelve el jugador en formato de línea CSV, en el mismo orden de columnas
        /// que se usa al cargar el archivo. Se usa en el módulo de persistencia para
        /// reescribir el archivo .csv cada vez que hay un cambio (inserción, edición o
        /// eliminación) y así mantener los datos sincronizados en tiempo real.
        /// </summary>
        public string AFilaCsv()
        {
            return $"{Id},{Nombre},{Seleccion},{Posicion},{MinutosJugados},{Goles},{Asistencias},{TarjetasAmarillas},{TarjetasRojas},{PartidosJugados}";
        }

        /// <summary>
        /// Devuelve el jugador en formato de línea .txt (separado por '|', más legible
        /// para lectura humana). Se usa para mantener sincronizado el respaldo .txt.
        /// </summary>
        public string AFilaTxt()
        {
            return $"{Id}|{Nombre}|{Seleccion}|{Posicion}|{MinutosJugados}|{Goles}|{Asistencias}|{TarjetasAmarillas}|{TarjetasRojas}|{PartidosJugados}";
        }

        /// <summary>
        /// Obtiene el valor numérico de una categoría de estadística por nombre.
        /// Se usa desde los módulos de Top 5 / listado ordenado para poder pedirle
        /// al Max Heap / Min Heap que comparen jugadores según la categoría elegida
        /// por el usuario en el menú (goles, asistencias, minutos, tarjetas, partidos),
        /// sin tener que duplicar código de comparación para cada categoría.
        /// </summary>
        public int ObtenerValorCategoria(string categoria)
        {
            return categoria.ToLower() switch
            {
                "goles" => Goles,
                "asistencias" => Asistencias,
                "minutos" => MinutosJugados,
                "tarjetasamarillas" => TarjetasAmarillas,
                "tarjetasrojas" => TarjetasRojas,
                "partidos" => PartidosJugados,
                _ => 0
            };
        }

        public override string ToString()
        {
            return $"[{Id}] {Nombre,-20} | {Seleccion,-15} | {Posicion,-12} | Min:{MinutosJugados,4} | Gol:{Goles,3} | Ast:{Asistencias,3} | TA:{TarjetasAmarillas,2} | TR:{TarjetasRojas,2} | PJ:{PartidosJugados,2}";
        }
    }
}
