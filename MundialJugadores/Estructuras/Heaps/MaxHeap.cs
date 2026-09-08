namespace MundialJugadores.Estructuras.Heaps
{
    /// <summary>
    /// Max Heap genérico (estructura OBLIGATORIA del proyecto).
    /// CONSTRUCCIÓN: montículo binario implementado sobre un arreglo (T[]) manejado
    /// manualmente, igual que MinHeap pero invirtiendo la comparación: el elemento
    /// MAYOR según el comparador siempre queda en la raíz (índice 0). No usa
    /// List&lt;T&gt; ni PriorityQueue&lt;T,T&gt; para su lógica interna.
    /// ACCIONES QUE REALIZA: Insertar (SiftUp) y ExtraerMaximo (SiftDown).
    /// JUSTIFICACIÓN TÉCNICA: se usa para generar el "listado general de todos los
    /// jugadores ordenados por la categoría que se elija" (goles, asistencias,
    /// minutos, etc.). Se insertan TODOS los jugadores en el Max Heap según la
    /// categoría elegida y luego se extraen uno por uno con ExtraerMaximo: cada
    /// extracción da el siguiente jugador en orden DESCENDENTE, construyendo el
    /// ranking completo en O(n log n), sin necesitar un algoritmo de ordenamiento
    /// aparte ni una colección nativa de ordenamiento (Sort/OrderBy).
    /// DATOS QUE MANIPULA: un arreglo genérico de elementos T y un comparador.
    /// POR QUÉ ESTÁ AQUÍ: junto a MinHeap, en Estructuras/Heaps, porque comparten la
    /// misma familia de estructura (montículo binario) y solo cambia el criterio de
    /// orden (mínimo vs. máximo en la raíz).
    /// </summary>
    public class MaxHeap<T>
    {
        private T[] datos;
        private int cantidad;
        private readonly Comparison<T> comparar; // comparar(a,b) < 0  => a es "menor" que b

        public int Count => cantidad;

        public MaxHeap(Comparison<T> comparador, int capacidadInicial = 8)
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

        /// <summary>Inserta un elemento y restaura la propiedad de montículo máximo subiéndolo.</summary>
        public void Insertar(T elemento)
        {
            if (cantidad == datos.Length) DuplicarCapacidad();
            datos[cantidad] = elemento;
            cantidad++;
            SiftUp(cantidad - 1);
        }

        /// <summary>Observa el elemento máximo actual (la raíz) sin sacarlo del heap.</summary>
        public T VerMaximo()
        {
            if (cantidad == 0) throw new InvalidOperationException("El Max Heap está vacío.");
            return datos[0];
        }

        /// <summary>Extrae y elimina el elemento máximo del heap.</summary>
        public T ExtraerMaximo()
        {
            if (cantidad == 0) throw new InvalidOperationException("El Max Heap está vacío.");
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
                if (comparar(datos[indice], datos[padre]) > 0)
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
                int mayor = indice;

                if (izquierdo < cantidad && comparar(datos[izquierdo], datos[mayor]) > 0) mayor = izquierdo;
                if (derecho < cantidad && comparar(datos[derecho], datos[mayor]) > 0) mayor = derecho;

                if (mayor == indice) break;

                (datos[indice], datos[mayor]) = (datos[mayor], datos[indice]);
                indice = mayor;
            }
        }
    }
}
