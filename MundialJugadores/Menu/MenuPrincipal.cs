using MundialJugadores.Estructuras.BPlusTree;
using MundialJugadores.Modulos;

namespace MundialJugadores.Menu
{
    /// <summary>
    /// Menú principal del sistema (capa de presentación / consola).
    /// CONSTRUCCIÓN: clase que orquesta el bucle de interacción con el usuario,
    /// delegando cada opción al módulo correspondiente (Insertar, Buscar, Eliminar,
    /// Recorrer, Imprimir), sin implementar lógica de negocio propia — respeta la
    /// organización modular pedida: el menú solo "dirige el tráfico".
    /// ACCIÓN QUE REALIZA: muestra las opciones disponibles en bucle hasta que el
    /// usuario elige salir, y llama al módulo correspondiente según la opción.
    /// DATOS QUE MANIPULA: la opción de texto ingresada por consola y una
    /// referencia al Árbol B+ (única fuente de verdad de los jugadores en memoria).
    /// POR QUÉ ESTÁ AQUÍ: en su propia carpeta Menu/, separada de Modulos/, porque
    /// su responsabilidad es de interfaz/orquestación, no de lógica de datos.
    /// </summary>
    public class MenuPrincipal
    {
        private readonly ArbolBMas arbol;

        public MenuPrincipal(ArbolBMas arbol)
        {
            this.arbol = arbol;
        }

        public void Ejecutar()
        {
            bool continuar = true;
            while (continuar)
            {
                Console.WriteLine("\n==================== SISTEMA DE JUGADORES DEL MUNDIAL ====================");
                Console.WriteLine("1. Insertar nuevo jugador");
                Console.WriteLine("2. Buscar jugador por Id");
                Console.WriteLine("3. Eliminar jugador");
                Console.WriteLine("4. Recorrer todos los jugadores (Árbol B+, ordenado por Id)");
                Console.WriteLine("5. Top 5 por categoría (Min Heap)");
                Console.WriteLine("6. Listado general ordenado por categoría (Max Heap)");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opción: ");

                string opcion = (Console.ReadLine() ?? string.Empty).Trim();

                switch (opcion)
                {
                    case "1":
                        ModuloInsertar.InsertarDesdeConsola(arbol);
                        break;
                    case "2":
                        ModuloBuscar.BuscarDesdeConsola(arbol);
                        break;
                    case "3":
                        ModuloEliminar.EliminarDesdeConsola(arbol);
                        break;
                    case "4":
                        ModuloRecorrer.MostrarRecorridoCompleto(arbol);
                        break;
                    case "5":
                        {
                            string categoria = ModuloImprimir.ElegirCategoriaDesdeConsola();
                            ModuloImprimir.MostrarTopCinco(arbol, categoria);
                            break;
                        }
                    case "6":
                        {
                            string categoria = ModuloImprimir.ElegirCategoriaDesdeConsola();
                            ModuloImprimir.MostrarListadoOrdenadoPorCategoria(arbol, categoria);
                            break;
                        }
                    case "0":
                        continuar = false;
                        Console.WriteLine("Saliendo del sistema. ¡Hasta luego!");
                        break;
                    default:
                        Console.WriteLine("Opción no válida, intente de nuevo.");
                        break;
                }
            }
        }
    }
}
