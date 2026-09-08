namespace MundialJugadores.Estructuras.Heaps
{
    /// <summary>
    /// Min Heap genérico (estructura OBLIGATORIA del proyecto).
    /// CONSTRUCCIÓN: montículo binario implementado sobre un arreglo (T[]) manejado
    /// manualmente (con duplicación de tamaño propia, "DuplicarCapacidad"), NO sobre
    /// List&lt;T&gt; ni PriorityQueue&lt;T,T&gt;, cumpliendo la restricción de no usar
    /// colecciones nativas para la lógica del montículo. El elemento MENOR según el
    /// comparador siempre queda en la raíz (índice 0).
    /// ACCIONES QUE REALIZA: Insertar (sube el elemento nuevo mientras sea menor que
    /// su padre - "SiftUp") y ExtraerMinimo (saca la raíz, sube el último elemento a
    /// la raíz y lo hunde a su posición correcta - "SiftDown").
    /// JUSTIFICACIÓN TÉCNICA: se usa específicamente para calcular el TOP 5 de cada
    /// categoría (goles, asistencias, minutos, etc.) de forma eficiente: se mantiene
    /// un Min Heap de tamaño fijo 5; cada jugador nuevo se compara solo contra el
    /// mínimo actual del heap (la raíz). Si el jugador nuevo es mayor que ese mínimo,
    /// reemplaza a la raíz. Esto da complejidad O(n log 5) para encontrar el Top 5 de
    /// n jugadores, mucho más eficiente que ordenar TODA la lista para luego tomar
    /// los primeros 5.
    /// DATOS QUE MANIPULA: un arreglo genérico de elementos T y un comparador
    /// (Comparison&lt;T&gt;) que define qué campo/categoría se está comparando.
    /// POR QUÉ ESTÁ AQUÍ: junto a MaxHeap, en Estructuras/Heaps, separado del Árbol B+
    /// porque resuelve un problema distinto (ranking/orden) al de almacenamiento.
    /// </summary>
    public class MinHeap<T>
    {
        private T[] datos;
        private int cantidad;
        private readonly Comparison<T> comparar; // comparar(a,b) < 0  => a es "menor" que b

        public int Count => cantidad;

        public MinHeap(Comparison<T> comparador, int capacidadInicial = 8)
        {
            comparar = comparador;
            datos = new T[capacidadInicial];
            cantidad = 0;
        }

        private void DuplicarCapacidad()
        {
            T[] nuevo = new T[datos.Length * 2];
            for (int i = 0; i < cantidad; i++) nuevo[i] = datos[i];
            datos = nuevo;
        }

        /// <summary>Inserta un elemento y restaura la propiedad de montículo mínimo subiéndolo.</summary>
        public void Insertar(T elemento)
        {
            if (cantidad == datos.Length) DuplicarCapacidad();
            datos[cantidad] = elemento;
            cantidad++;
            SiftUp(cantidad - 1);
        }

        /// <summary>Observa el elemento mínimo actual (la raíz) sin sacarlo del heap.</summary>
        public T VerMinimo()
        {
            if (cantidad == 0) throw new InvalidOperationException("El Min Heap está vacío.");
            return datos[0];
        }

        /// <summary>Reemplaza directamente la raíz por un nuevo valor y reordena hundiéndola. Se usa en el algoritmo de Top-K para sustituir el mínimo actual por un candidato mayor.</summary>
        public void ReemplazarMinimo(T nuevoElemento)
        {
            if (cantidad == 0) throw new InvalidOperationException("El Min Heap está vacío.");
            datos[0] = nuevoElemento;
            SiftDown(0);
        }

        /// <summary>Extrae y elimina el elemento mínimo del heap.</summary>
        public T ExtraerMinimo()
        {
            if (cantidad == 0) throw new InvalidOperationException("El Min Heap está vacío.");
            T raizActual = datos[0];
            cantidad--;
            datos[0] = datos[cantidad];
            SiftDown(0);
            return raizActual;
        }

        private void SiftUp(int indice)
        {
            while (indice > 0)
            {
                int padre = (indice - 1) / 2;
                if (comparar(datos[indice], datos[padre]) < 0)
                {
                    (datos[indice], datos[padre]) = (datos[padre], datos[indice]);
                    indice = padre;
                }
                else break;
            }
        }

        private void SiftDown(int indice)
        {
            while (true)
            {
                int izquierdo = indice * 2 + 1;
                int derecho = indice * 2 + 2;
                int menor = indice;

                if (izquierdo < cantidad && comparar(datos[izquierdo], datos[menor]) < 0) menor = izquierdo;
                if (derecho < cantidad && comparar(datos[derecho], datos[menor]) < 0) menor = derecho;

                if (menor == indice) break;

                (datos[indice], datos[menor]) = (datos[menor], datos[indice]);
                indice = menor;
            }
        }
    }
}
