using MundialJugadores.Estructuras.BPlusTree;
using MundialJugadores.Modelos;

namespace MundialJugadores.Modulos
{
    /// <summary>
    /// Módulo de carga de datos desde archivo .csv.
    /// CONSTRUCCIÓN: clase estática con una única responsabilidad (leer el .csv y
    /// poblar el árbol), como parte de la organización modular pedida: un archivo
    /// específico solo para carga de datos CSV.
    /// ACCIÓN QUE REALIZA: abre "Datos/jugadores.csv", omite el encabezado, parsea
    /// cada línea separada por comas y construye objetos Jugador que inserta en el
    /// Árbol B+ recibido por parámetro.
    /// DATOS QUE MANIPULA: líneas de texto del archivo CSV y el ArbolBMas destino.
    /// POR QUÉ ESTÁ AQUÍ: en Modulos/, junto a ModuloCargaTXT, separando claramente
    /// la responsabilidad de "de dónde vienen los datos" de "qué se hace con ellos".
    /// </summary>
    public static class ModuloCargaCSV
    {
        public static int Cargar(ArbolBMas arbol, string ruta)
        {
            if (!File.Exists(ruta)) return 0;

            int cargados = 0;
            string[] lineas = File.ReadAllLines(ruta);

            for (int i = 1; i < lineas.Length; i++) // i = 1 para saltar el encabezado
            {
                string linea = lineas[i].Trim();
                if (string.IsNullOrWhiteSpace(linea)) continue;

                string[] campos = linea.Split(',');
                if (campos.Length < 10) continue; // línea mal formada, se ignora

                try
                {
                    Jugador jugador = new Jugador(
                        id: campos[0].Trim(),
                        nombre: campos[1].Trim(),
                        seleccion: campos[2].Trim(),
                        posicion: campos[3].Trim(),
                        minutosJugados: int.Parse(campos[4].Trim()),
                        goles: int.Parse(campos[5].Trim()),
                        asistencias: int.Parse(campos[6].Trim()),
                        tarjetasAmarillas: int.Parse(campos[7].Trim()),
                        tarjetasRojas: int.Parse(campos[8].Trim()),
                        partidosJugados: int.Parse(campos[9].Trim())
                    );
                    arbol.Insertar(jugador);
                    cargados++;
                }
                catch (FormatException)
                {
                    // Línea con datos numéricos inválidos: se ignora y se continúa
                    // para no detener la carga completa por un solo registro dañado.
                    continue;
                }
            }

            return cargados;
        }
    }
}
