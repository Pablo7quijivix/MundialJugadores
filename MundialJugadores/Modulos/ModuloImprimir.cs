using MundialJugadores.Estructuras.BPlusTree;
using MundialJugadores.Estructuras.Heaps;
using MundialJugadores.Modelos;

namespace MundialJugadores.Modulos
{
    /// <summary>
    /// Módulo de impresión de reportes estadísticos.
    /// CONSTRUCCIÓN: clase estática que NO almacena datos; construye heaps
    /// temporales (MinHeap/MaxHeap) a partir del recorrido del Árbol B+ cada vez
    /// que se pide un reporte, ya que el ranking depende de la categoría elegida
    /// en ese momento por el usuario (no tiene sentido mantenerlo precalculado).
    /// ACCIONES QUE REALIZA:
    ///   1) TopCinco(categoria): usa un MIN HEAP de capacidad fija 5 para hallar,
    ///      de forma eficiente (O(n log 5)), a los 5 jugadores con mayor valor en
    ///      la categoría elegida, sin tener que ordenar la lista completa.
    ///   2) ListadoOrdenadoPorCategoria(categoria): usa un MAX HEAP con TODOS los
    ///      jugadores para extraerlos uno por uno de mayor a menor (O(n log n)) y
    ///      construir el listado general ordenado por la categoría elegida.
    /// DATOS QUE MANIPULA: objetos Jugador obtenidos del Árbol B+ y su valor
    /// numérico en la categoría solicitada (goles, asistencias, minutos, etc.).
    /// POR QUÉ ESTÁ AQUÍ: en Modulos/, como archivo específico para "imprimir",
    /// separado de "recorrer" porque aquí la lógica es de RANKING (heaps) y no de
    /// recorrido puro del árbol.
    /// </summary>
    public static class ModuloImprimir
    {
        private static readonly string[] CategoriasValidas =
            { "goles", "asistencias", "minutos", "tarjetasamarillas", "tarjetasrojas", "partidos" };

        public static string ElegirCategoriaDesdeConsola()
        {
            Console.WriteLine("\nCategorías disponibles: goles, asistencias, minutos, tarjetasamarillas, tarjetasrojas, partidos");
            Console.Write("Elija una categoría: ");
            string categoria = (Console.ReadLine() ?? string.Empty).Trim().ToLower();
            while (Array.IndexOf(CategoriasValidas, categoria) == -1)
            {
                Console.Write("Categoría no válida. Intente de nuevo: ");
                categoria = (Console.ReadLine() ?? string.Empty).Trim().ToLower();
            }
            return categoria;
        }

        /// <summary>
        /// Calcula y muestra el Top 5 de jugadores según 'categoria' usando un
        /// Min Heap de capacidad 5: se recorre el árbol una sola vez; mientras el
        /// heap tenga menos de 5 elementos se insertan directamente; una vez lleno,
        /// cada nuevo jugador se compara solo contra la raíz (el mínimo del top
        /// actual) y, si lo supera, reemplaza a la raíz. Al final se extraen los 5
        /// elementos del heap (ExtraerMinimo los entrega de menor a mayor) y se
        /// invierten para mostrarlos de mayor a menor.
        /// </summary>
        public static void MostrarTopCinco(ArbolBMas arbol, string categoria)
        {
            List<Jugador> todos = arbol.RecorrerInOrden();

            Comparison<Jugador> comparador = (a, b) =>
                a.ObtenerValorCategoria(categoria).CompareTo(b.ObtenerValorCategoria(categoria));

            MinHeap<Jugador> topHeap = new MinHeap<Jugador>(comparador, capacidadInicial: 5);

            foreach (Jugador jugador in todos)
            {
                if (topHeap.Count < 5)
                {
                    topHeap.Insertar(jugador);
                }
                else if (jugador.ObtenerValorCategoria(categoria) > topHeap.VerMinimo().ObtenerValorCategoria(categoria))
                {
                    topHeap.ReemplazarMinimo(jugador);
                }
            }

            // El Min Heap entrega los elementos de menor a mayor; se guardan en un
            // arreglo y se recorren al revés para mostrar el Top 5 de mayor a menor.
            int cantidad = topHeap.Count;
            Jugador[] top = new Jugador[cantidad];
            for (int i = cantidad - 1; i >= 0; i--)
            {
                top[i] = topHeap.ExtraerMinimo();
            }

            Console.WriteLine($"\n--- TOP 5 por '{categoria}' (Min Heap) ---");
            if (cantidad == 0)
            {
                Console.WriteLine("(No hay jugadores registrados)");
                return;
            }
            for (int i = 0; i < cantidad; i++)
            {
                Console.WriteLine($"{i + 1}. {top[i]}  ->  {categoria}: {top[i].ObtenerValorCategoria(categoria)}");
            }
        }

        /// <summary>
        /// Genera el listado general de todos los jugadores ordenados por
        /// 'categoria' usando un Max Heap: se insertan TODOS los jugadores y luego
        /// se van extrayendo con ExtraerMaximo, que siempre entrega el siguiente
        /// mayor disponible, produciendo el orden descendente completo.
        /// </summary>
        public static void MostrarListadoOrdenadoPorCategoria(ArbolBMas arbol, string categoria)
        {
            List<Jugador> todos = arbol.RecorrerInOrden();

            Comparison<Jugador> comparador = (a, b) =>
                a.ObtenerValorCategoria(categoria).CompareTo(b.ObtenerValorCategoria(categoria));

            MaxHeap<Jugador> heap = new MaxHeap<Jugador>(comparador, capacidadInicial: Math.Max(8, todos.Count));

            foreach (Jugador jugador in todos)
            {
                heap.Insertar(jugador);
            }

            Console.WriteLine($"\n--- Listado general ordenado por '{categoria}' (Max Heap, descendente) ---");
            if (heap.Count == 0)
            {
                Console.WriteLine("(No hay jugadores registrados)");
                return;
            }

            int posicion = 1;
            while (heap.Count > 0)
            {
                Jugador jugador = heap.ExtraerMaximo();
                Console.WriteLine($"{posicion}. {jugador}  ->  {categoria}: {jugador.ObtenerValorCategoria(categoria)}");
                posicion++;
            }
        }
    }
}
