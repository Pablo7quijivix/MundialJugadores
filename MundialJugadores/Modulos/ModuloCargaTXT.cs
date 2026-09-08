using MundialJugadores.Estructuras.BPlusTree;
using MundialJugadores.Modelos;

namespace MundialJugadores.Modulos
{
    /// <summary>
    /// Módulo de carga de datos desde archivo .txt.
    /// CONSTRUCCIÓN: clase estática con una única responsabilidad (leer el .txt y
    /// poblar el árbol), separado de ModuloCargaCSV aunque el formato es similar,
    /// para respetar la organización modular pedida (un archivo por función).
    /// ACCIÓN QUE REALIZA: abre "Datos/jugadores.txt" (formato con '|' como
    /// separador, sin encabezado) y construye/inserta cada Jugador en el árbol.
    /// Se usa como respaldo/alternativa de carga; en Program.cs se decide si se
    /// carga desde CSV, desde TXT o desde ambos evitando duplicados (el Id ya
    /// existente simplemente se actualiza en vez de duplicarse, gracias a la lógica
    /// de Insertar del Árbol B+).
    /// DATOS QUE MANIPULA: líneas de texto del archivo TXT y el ArbolBMas destino.
    /// POR QUÉ ESTÁ AQUÍ: en Modulos/, junto a ModuloCargaCSV.
    /// </summary>
    public static class ModuloCargaTXT
    {
        public static int Cargar(ArbolBMas arbol, string ruta)
        {
            if (!File.Exists(ruta)) return 0;

            int cargados = 0;
            string[] lineas = File.ReadAllLines(ruta);

            foreach (string lineaCruda in lineas)
            {
                string linea = lineaCruda.Trim();
                if (string.IsNullOrWhiteSpace(linea)) continue;

                string[] campos = linea.Split('|');
                if (campos.Length < 10) continue;

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
                    continue;
                }
            }

            return cargados;
        }
    }
}
