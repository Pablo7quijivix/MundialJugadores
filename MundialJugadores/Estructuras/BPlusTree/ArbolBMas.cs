using MundialJugadores.Modelos;

namespace MundialJugadores.Estructuras.BPlusTree
{
    /// <summary>
    /// Árbol B+ (estructura OBLIGATORIA del proyecto).
    /// CONSTRUCCIÓN: implementado desde cero con nodos propios (NodoB) que usan
    /// arreglos de tamaño fijo, sin usar List/Dictionary/SortedDictionary/SortedSet
    /// como estructura principal. Es la estructura de ALMACENAMIENTO PRINCIPAL del
    /// sistema: cada jugador se guarda una única vez, en una hoja, indexado por su Id.
    /// ACCIONES QUE REALIZA: Insertar, Buscar, Eliminar y Recorrer (in-order, gracias
    /// a la lista enlazada de hojas propia del B+).
    /// JUSTIFICACIÓN TÉCNICA DE LA ELECCIÓN: las operaciones más frecuentes del
    /// problema son "buscar rápidamente las estadísticas de un jugador en específico"
    /// e "insertar/actualizar" jugadores conforme avanza el torneo, y además se pide
    /// "mostrar el listado general ordenado". Un Árbol B+ resuelve las tres cosas con
    /// una sola estructura: búsqueda/inserción/eliminación en O(log n) igual que un
    /// AVL o un B normal, PERO además mantiene todos los datos en las hojas enlazadas
    /// entre sí, lo que permite recorrer TODOS los registros en orden alfabético/por
    /// Id en O(n) sin tener que hacer un recorrido recursivo del árbol completo. Por
    /// eso se prefiere sobre un BST/AVL (que no tienen ese enlace horizontal) para
    /// este caso de uso.
    /// DATOS QUE MANIPULA: nodos NodoB (internos y hojas) y, dentro de las hojas,
    /// objetos Jugador completos.
    /// POR QUÉ ESTÁ AQUÍ: vive en Estructuras/BPlusTree porque es una estructura de
    /// datos genérica y reutilizable, separada de la lógica de menú o persistencia.
    /// </summary>
    public class ArbolBMas
    {
        private readonly int orden;      // Orden M del árbol (máx. de hijos por nodo interno)
        private NodoB raiz;

        // Cantidad mínima de claves permitidas antes de forzar una redistribución/fusión.
        private int MinClavesHoja => (orden + 1) / 2 - 1;
        private int MinClavesInterno => (orden + 1) / 2 - 1;

        public ArbolBMas(int orden = 4)
        {
            this.orden = orden;
            raiz = new NodoB(orden, esHoja: true);
        }

        // ---------------------------------------------------------------
        // BÚSQUEDA
        // ---------------------------------------------------------------

        /// <summary>
        /// Desciende desde la raíz hasta la hoja donde DEBERÍA estar 'clave',
        /// comparando contra las claves guía de cada nodo interno. Es el método base
        /// que reutilizan Insertar, Buscar y Eliminar para ubicarse en el árbol.
        /// </summary>
        private NodoB DescenderHastaHoja(string clave)
        {
            NodoB actual = raiz;
            while (!actual.EsHoja)
            {
                int pos = actual.PosicionParaClave(clave);
                // Si la clave es igual a una clave guía, se desciende por el hijo derecho
                // de esa clave (convención estándar de B+ para claves guía repetidas).
                if (pos < actual.NumClaves && string.Compare(clave, actual.Claves[pos], StringComparison.Ordinal) == 0)
                {
                    pos++;
                }
                actual = actual.Hijos[pos]!;
            }
            return actual;
        }

        /// <summary>
        /// Busca un jugador por Id. Complejidad O(log n): desciende un nivel del
        /// árbol por cada comparación de nodo, en vez de recorrer todos los registros.
        /// Se usa en el módulo "Buscar" para la opción del menú de búsqueda puntual.
        /// </summary>
        public Jugador? Buscar(string clave)
        {
            NodoB hoja = DescenderHastaHoja(clave);
            int pos = hoja.PosicionParaClave(clave);
            if (pos < hoja.NumClaves && hoja.Claves[pos] == clave)
            {
                return hoja.Valores[pos];
            }
            return null;
        }

        // ---------------------------------------------------------------
        // INSERCIÓN
        // ---------------------------------------------------------------

        /// <summary>
        /// Inserta (o actualiza, si el Id ya existe) un jugador en el árbol.
        /// ACCIÓN: ubica la hoja correspondiente, inserta ordenadamente y, si el nodo
        /// queda con más claves de las permitidas (Orden), lo divide en dos y propaga
        /// la clave separadora hacia el nodo padre (pudiendo dividir en cascada hasta
        /// la raíz, que es como el árbol crece en altura de forma balanceada).
        /// </summary>
        public void Insertar(Jugador jugador)
        {
            NodoB hoja = DescenderHastaHoja(jugador.Id);

            // Si el Id ya existe, se actualiza el registro (evita duplicados).
            int posExistente = hoja.PosicionParaClave(jugador.Id);
            if (posExistente < hoja.NumClaves && hoja.Claves[posExistente] == jugador.Id)
            {
                hoja.Valores[posExistente] = jugador;
                return;
            }

            hoja.InsertarEnHoja(jugador.Id, jugador);

            if (hoja.NumClaves == orden) // se pasó del máximo permitido (Orden - 1)
            {
                DividirHoja(hoja);
            }
        }

        private void DividirHoja(NodoB hoja)
        {
            int mitad = orden / 2;
            NodoB nuevaHoja = new NodoB(orden, esHoja: true);

            // La segunda mitad de claves/valores pasa al nuevo nodo hermano.
            int total = hoja.NumClaves;
            for (int i = mitad; i < total; i++)
            {
                nuevaHoja.Claves[i - mitad] = hoja.Claves[i];
                nuevaHoja.Valores[i - mitad] = hoja.Valores[i];
                hoja.Claves[i] = string.Empty;
                hoja.Valores[i] = null;
            }
            nuevaHoja.NumClaves = total - mitad;
            hoja.NumClaves = mitad;

            // Se mantiene la lista enlazada de hojas (característica propia del B+).
            nuevaHoja.Siguiente = hoja.Siguiente;
            hoja.Siguiente = nuevaHoja;

            string claveQueSube = nuevaHoja.Claves[0];

            if (hoja.Padre == null)
            {
                // La hoja dividida era la raíz: se crea una nueva raíz interna.
                NodoB nuevaRaiz = new NodoB(orden, esHoja: false);
                nuevaRaiz.Claves[0] = claveQueSube;
                nuevaRaiz.Hijos[0] = hoja;
                nuevaRaiz.Hijos[1] = nuevaHoja;
                nuevaRaiz.NumClaves = 1;
                hoja.Padre = nuevaRaiz;
                nuevaHoja.Padre = nuevaRaiz;
                raiz = nuevaRaiz;
            }
            else
            {
                nuevaHoja.Padre = hoja.Padre;
                InsertarEnPadre(hoja.Padre, claveQueSube, nuevaHoja, hoja);
            }
        }

        /// <summary>
        /// Inserta una clave guía + puntero al nuevo nodo hijo dentro de un nodo
        /// interno, justo a la derecha de 'hijoIzquierdo'. Si el nodo interno queda
        /// sobrecargado, también se divide en cascada.
        /// </summary>
        private void InsertarEnPadre(NodoB padre, string clave, NodoB nuevoHijo, NodoB hijoIzquierdo)
        {
            int posHijo = 0;
            while (padre.Hijos[posHijo] != hijoIzquierdo) posHijo++;

            for (int i = padre.NumClaves; i > posHijo; i--)
            {
                padre.Claves[i] = padre.Claves[i - 1];
            }
            for (int i = padre.NumClaves + 1; i > posHijo + 1; i--)
            {
                padre.Hijos[i] = padre.Hijos[i - 1];
            }
            padre.Claves[posHijo] = clave;
            padre.Hijos[posHijo + 1] = nuevoHijo;
            padre.NumClaves++;

            if (padre.NumClaves == orden)
            {
                DividirInterno(padre);
            }
        }

        private void DividirInterno(NodoB nodo)
        {
            int mitad = orden / 2;
            string claveQueSube = nodo.Claves[mitad];

            NodoB nuevoNodo = new NodoB(orden, esHoja: false);
            int totalClaves = nodo.NumClaves;

            for (int i = mitad + 1; i < totalClaves; i++)
            {
                nuevoNodo.Claves[i - mitad - 1] = nodo.Claves[i];
            }
            for (int i = mitad + 1; i <= totalClaves; i++)
            {
                nuevoNodo.Hijos[i - mitad - 1] = nodo.Hijos[i];
                if (nodo.Hijos[i] != null) nodo.Hijos[i]!.Padre = nuevoNodo;
            }
            nuevoNodo.NumClaves = totalClaves - mitad - 1;
            nodo.NumClaves = mitad;

            if (nodo.Padre == null)
            {
                NodoB nuevaRaiz = new NodoB(orden, esHoja: false);
                nuevaRaiz.Claves[0] = claveQueSube;
                nuevaRaiz.Hijos[0] = nodo;
                nuevaRaiz.Hijos[1] = nuevoNodo;
                nuevaRaiz.NumClaves = 1;
                nodo.Padre = nuevaRaiz;
                nuevoNodo.Padre = nuevaRaiz;
                raiz = nuevaRaiz;
            }
            else
            {
                nuevoNodo.Padre = nodo.Padre;
                InsertarEnPadre(nodo.Padre, claveQueSube, nuevoNodo, nodo);
            }
        }

        // ---------------------------------------------------------------
        // ELIMINACIÓN
        // ---------------------------------------------------------------

        /// <summary>
        /// Elimina un jugador por Id. ACCIÓN: ubica la hoja y borra la clave; si tras
        /// borrar el nodo queda por debajo del mínimo permitido de claves, se intenta
        /// primero pedir prestada una clave a un hermano (redistribución) y, si ningún
        /// hermano puede prestar, se fusionan dos nodos (pudiendo propagar la fusión
        /// hacia arriba, incluso reduciendo la altura del árbol si la raíz se vacía).
        /// </summary>
        public bool Eliminar(string clave)
        {
            NodoB hoja = DescenderHastaHoja(clave);
            int pos = hoja.PosicionParaClave(clave);
            if (pos >= hoja.NumClaves || hoja.Claves[pos] != clave)
            {
                return false; // no existe
            }

            hoja.EliminarDeHoja(pos);

            if (hoja == raiz)
            {
                return true; // la raíz-hoja puede tener menos del mínimo sin problema
            }

            if (hoja.NumClaves < MinClavesHoja)
            {
                RebalancearHoja(hoja);
            }

            return true;
        }

        private int IndiceHijo(NodoB padre, NodoB hijo)
        {
            for (int i = 0; i <= padre.NumClaves; i++)
            {
                if (padre.Hijos[i] == hijo) return i;
            }
            return -1;
        }

        private void RebalancearHoja(NodoB hoja)
        {
            NodoB padre = hoja.Padre!;
            int idx = IndiceHijo(padre, hoja);
            NodoB? hermanoDer = idx + 1 <= padre.NumClaves ? padre.Hijos[idx + 1] : null;
            NodoB? hermanoIzq = idx - 1 >= 0 ? padre.Hijos[idx - 1] : null;

            // 1) Intentar pedir prestado al hermano derecho.
            if (hermanoDer != null && hermanoDer.NumClaves > MinClavesHoja)
            {
                hoja.Claves[hoja.NumClaves] = hermanoDer.Claves[0];
                hoja.Valores[hoja.NumClaves] = hermanoDer.Valores[0];
                hoja.NumClaves++;
                hermanoDer.EliminarDeHoja(0);
                padre.Claves[idx] = hermanoDer.Claves[0];
                return;
            }

            // 2) Intentar pedir prestado al hermano izquierdo.
            if (hermanoIzq != null && hermanoIzq.NumClaves > MinClavesHoja)
            {
                for (int i = hoja.NumClaves; i > 0; i--)
                {
                    hoja.Claves[i] = hoja.Claves[i - 1];
                    hoja.Valores[i] = hoja.Valores[i - 1];
                }
                hoja.Claves[0] = hermanoIzq.Claves[hermanoIzq.NumClaves - 1];
                hoja.Valores[0] = hermanoIzq.Valores[hermanoIzq.NumClaves - 1];
                hoja.NumClaves++;
                hermanoIzq.EliminarDeHoja(hermanoIzq.NumClaves - 1);
                padre.Claves[idx - 1] = hoja.Claves[0];
                return;
            }

            // 3) Ningún hermano puede prestar: se fusiona con uno de ellos.
            if (hermanoDer != null)
            {
                FusionarHojas(hoja, hermanoDer, padre, idx);
            }
            else if (hermanoIzq != null)
            {
                FusionarHojas(hermanoIzq, hoja, padre, idx - 1);
            }
        }

        private void FusionarHojas(NodoB izquierda, NodoB derecha, NodoB padre, int idxSeparador)
        {
            for (int i = 0; i < derecha.NumClaves; i++)
            {
                izquierda.Claves[izquierda.NumClaves + i] = derecha.Claves[i];
                izquierda.Valores[izquierda.NumClaves + i] = derecha.Valores[i];
            }
            izquierda.NumClaves += derecha.NumClaves;
            izquierda.Siguiente = derecha.Siguiente;

            EliminarDeInterno(padre, idxSeparador);

            if (padre == raiz && padre.NumClaves == 0)
            {
                raiz = izquierda;
                izquierda.Padre = null;
            }
            else if (padre != raiz && padre.NumClaves < MinClavesInterno)
            {
                RebalancearInterno(padre);
            }
        }

        private void EliminarDeInterno(NodoB padre, int posClave)
        {
            for (int i = posClave; i < padre.NumClaves - 1; i++)
            {
                padre.Claves[i] = padre.Claves[i + 1];
            }
            for (int i = posClave + 1; i < padre.NumClaves; i++)
            {
                padre.Hijos[i] = padre.Hijos[i + 1];
            }
            padre.Hijos[padre.NumClaves] = null;
            padre.NumClaves--;
        }

        private void RebalancearInterno(NodoB nodo)
        {
            if (nodo == raiz)
            {
                if (nodo.NumClaves == 0 && nodo.Hijos[0] != null)
                {
                    raiz = nodo.Hijos[0]!;
                    raiz.Padre = null;
                }
                return;
            }

            NodoB padre = nodo.Padre!;
            int idx = IndiceHijo(padre, nodo);
            NodoB? hermanoDer = idx + 1 <= padre.NumClaves ? padre.Hijos[idx + 1] : null;
            NodoB? hermanoIzq = idx - 1 >= 0 ? padre.Hijos[idx - 1] : null;

            // 1) Rotación desde el hermano derecho.
            if (hermanoDer != null && hermanoDer.NumClaves > MinClavesInterno)
            {
                nodo.Claves[nodo.NumClaves] = padre.Claves[idx];
                nodo.Hijos[nodo.NumClaves + 1] = hermanoDer.Hijos[0];
                if (nodo.Hijos[nodo.NumClaves + 1] != null) nodo.Hijos[nodo.NumClaves + 1]!.Padre = nodo;
                nodo.NumClaves++;
                padre.Claves[idx] = hermanoDer.Claves[0];

                for (int i = 0; i < hermanoDer.NumClaves - 1; i++) hermanoDer.Claves[i] = hermanoDer.Claves[i + 1];
                for (int i = 0; i < hermanoDer.NumClaves; i++) hermanoDer.Hijos[i] = hermanoDer.Hijos[i + 1];
                hermanoDer.NumClaves--;
                return;
            }

            // 2) Rotación desde el hermano izquierdo.
            if (hermanoIzq != null && hermanoIzq.NumClaves > MinClavesInterno)
            {
                for (int i = nodo.NumClaves; i > 0; i--) nodo.Claves[i] = nodo.Claves[i - 1];
                for (int i = nodo.NumClaves + 1; i > 0; i--) nodo.Hijos[i] = nodo.Hijos[i - 1];

                nodo.Claves[0] = padre.Claves[idx - 1];
                nodo.Hijos[0] = hermanoIzq.Hijos[hermanoIzq.NumClaves];
                if (nodo.Hijos[0] != null) nodo.Hijos[0]!.Padre = nodo;
                nodo.NumClaves++;
                padre.Claves[idx - 1] = hermanoIzq.Claves[hermanoIzq.NumClaves - 1];
                hermanoIzq.NumClaves--;
                return;
            }

            // 3) Fusión con un hermano (propaga hacia arriba si es necesario).
            if (hermanoDer != null)
            {
                FusionarInternos(nodo, hermanoDer, padre, idx);
            }
            else if (hermanoIzq != null)
            {
                FusionarInternos(hermanoIzq, nodo, padre, idx - 1);
            }
        }

        private void FusionarInternos(NodoB izquierda, NodoB derecha, NodoB padre, int idxSeparador)
        {
            izquierda.Claves[izquierda.NumClaves] = padre.Claves[idxSeparador];
            izquierda.NumClaves++;

            for (int i = 0; i < derecha.NumClaves; i++)
            {
                izquierda.Claves[izquierda.NumClaves + i] = derecha.Claves[i];
            }
            for (int i = 0; i <= derecha.NumClaves; i++)
            {
                izquierda.Hijos[izquierda.NumClaves + i] = derecha.Hijos[i];
                if (derecha.Hijos[i] != null) derecha.Hijos[i]!.Padre = izquierda;
            }
            izquierda.NumClaves += derecha.NumClaves;

            EliminarDeInterno(padre, idxSeparador);

            if (padre == raiz && padre.NumClaves == 0)
            {
                raiz = izquierda;
                izquierda.Padre = null;
            }
            else if (padre != raiz && padre.NumClaves < MinClavesInterno)
            {
                RebalancearInterno(padre);
            }
        }

        // ---------------------------------------------------------------
        // RECORRIDO
        // ---------------------------------------------------------------

        /// <summary>
        /// Recorre TODOS los jugadores en orden ascendente de Id. ACCIÓN: en vez de
        /// recorrer el árbol de forma recursiva (como un BST/AVL), se aprovecha la
        /// característica distintiva del B+: bajar una sola vez hasta la primera hoja
        /// (la más a la izquierda) y luego avanzar linealmente por el enlace
        /// "Siguiente" entre hojas. Esto hace el recorrido más simple y eficiente
        /// (O(n) sin recursión) que en un árbol tradicional.
        /// </summary>
        public List<Jugador> RecorrerInOrden()
        {
            List<Jugador> resultado = new List<Jugador>();

            NodoB actual = raiz;
            while (!actual.EsHoja)
            {
                actual = actual.Hijos[0]!;
            }

            while (actual != null)
            {
                for (int i = 0; i < actual.NumClaves; i++)
                {
                    resultado.Add(actual.Valores[i]!);
                }
                actual = actual.Siguiente!;
            }

            return resultado;
        }
    }
}
