using MundialJugadores.Modelos;

namespace MundialJugadores.Estructuras.BPlusTree
{
    /// <summary>
    /// Nodo del Árbol B+.
    /// CONSTRUCCIÓN: se implementa con arreglos (T[]) de tamaño fijo (Orden), NO con
    /// List/Dictionary, cumpliendo el requisito de no usar colecciones nativas de .NET
    /// para resolver la lógica interna de la estructura. Un nodo puede ser de dos tipos:
    ///   - Nodo HOJA: guarda las claves y, junto a cada clave, el Jugador real
    ///     (los datos completos SOLO viven en las hojas, como corresponde a un B+).
    ///     Además cada hoja apunta a la "Siguiente" hoja (lista enlazada de hojas),
    ///     lo que permite recorrer TODOS los jugadores en orden con un solo barrido.
    ///   - Nodo INTERNO: solo guarda claves "guía" y punteros a Hijos; no guarda datos.
    /// ACCIONES QUE REALIZA: expone métodos utilitarios para insertar una clave dentro
    /// del arreglo del propio nodo, manteniéndolo siempre ordenado (necesario para que
    /// la búsqueda binaria/lineal descienda correctamente por el árbol).
    /// DATOS QUE MANIPULA: Claves (string, usadas como ID del jugador), Valores
    /// (Jugador, solo en hojas), Hijos (NodoB, solo en internos), Padre y Siguiente.
    /// POR QUÉ ESTÁ AQUÍ: se separa del árbol en sí para mantener el principio de
    /// responsabilidad única: el nodo sabe manipular SU propio contenido; el árbol
    /// (ArbolBMas) sabe cómo coordinar división, fusión y descenso entre nodos.
    /// </summary>
    public class NodoB
    {
        public int Orden { get; }          // Orden M del árbol (máx. de hijos por nodo interno)
        public bool EsHoja { get; set; }
        public int NumClaves { get; set; }
        public string[] Claves { get; }
        public Jugador?[] Valores { get; }   // Solo se usa si EsHoja == true
        public NodoB?[] Hijos { get; }       // Solo se usa si EsHoja == false
        public NodoB? Padre { get; set; }
        public NodoB? Siguiente { get; set; } // Enlace entre hojas (propio del B+)

        public NodoB(int orden, bool esHoja)
        {
            Orden = orden;
            EsHoja = esHoja;
            NumClaves = 0;
            // Se reserva una posición extra (orden) para poder insertar temporalmente
            // "de más" antes de dividir el nodo (técnica estándar de inserción en B/B+).
            Claves = new string[orden];
            Valores = new Jugador?[orden];
            Hijos = new NodoB?[orden + 1];
            Padre = null;
            Siguiente = null;
        }

        /// <summary>
        /// Busca la posición donde debería ir 'clave' dentro del arreglo ordenado de
        /// claves de este nodo (búsqueda lineal simple, suficiente para el orden bajo
        /// típico de un B+ usado en proyectos académicos). Se usa tanto para descender
        /// por nodos internos como para insertar/buscar dentro de una hoja.
        /// </summary>
        public int PosicionParaClave(string clave)
        {
            int i = 0;
            while (i < NumClaves && string.Compare(clave, Claves[i], StringComparison.Ordinal) > 0)
            {
                i++;
            }
            return i;
        }

        /// <summary>
        /// Inserta una clave (y su jugador, si es hoja) en la posición ordenada
        /// correspondiente, recorriendo el resto de elementos una posición a la
        /// derecha. Se usa durante la inserción antes de evaluar si el nodo quedó
        /// sobrecargado (NumClaves == Orden) y debe dividirse.
        /// </summary>
        public void InsertarEnHoja(string clave, Jugador jugador)
        {
            int pos = PosicionParaClave(clave);
            for (int i = NumClaves; i > pos; i--)
            {
                Claves[i] = Claves[i - 1];
                Valores[i] = Valores[i - 1];
            }
            Claves[pos] = clave;
            Valores[pos] = jugador;
            NumClaves++;
        }

        /// <summary>
        /// Elimina la clave (y su jugador) en la posición indicada, recorriendo el
        /// resto de elementos una posición a la izquierda. Se usa en la eliminación
        /// una vez localizada la hoja y posición exacta de la clave a borrar.
        /// </summary>
        public void EliminarDeHoja(int posicion)
        {
            for (int i = posicion; i < NumClaves - 1; i++)
            {
                Claves[i] = Claves[i + 1];
                Valores[i] = Valores[i + 1];
            }
            Claves[NumClaves - 1] = string.Empty;
            Valores[NumClaves - 1] = null;
            NumClaves--;
        }
    }
}
