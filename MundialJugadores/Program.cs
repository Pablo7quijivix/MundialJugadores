using MundialJugadores.Estructuras.BPlusTree;
using MundialJugadores.Menu;
using MundialJugadores.Modulos;
using MundialJugadores.Persistencia;

/// <summary>
/// Punto de entrada del programa.
/// CONSTRUCCIÓN: script mínimo (top-level statements de C#) que arma las piezas
/// del sistema: crea el Árbol B+ vacío, carga automáticamente los datos desde
/// jugadores.csv y jugadores.txt (sin duplicar, ya que Insertar actualiza si el Id
/// ya existe), y finalmente entrega el control al MenuPrincipal.
/// ACCIÓN QUE REALIZA: orquesta el arranque del sistema en tres pasos: 1) crear
/// estructuras, 2) cargar datos persistidos, 3) iniciar el menú interactivo.
/// DATOS QUE MANIPULA: el ArbolBMas raíz del sistema y las rutas de los archivos
/// de datos definidas en GestorPersistencia.
/// POR QUÉ ESTÁ AQUÍ: en la raíz del proyecto, como es convención en C#/.NET para
/// el archivo Program.cs.
/// </summary>
ArbolBMas arbol = new ArbolBMas(orden: 4);

Console.WriteLine("Cargando datos persistidos...");
int cargadosCsv = ModuloCargaCSV.Cargar(arbol, GestorPersistencia.RutaCsv);
int cargadosTxt = ModuloCargaTXT.Cargar(arbol, GestorPersistencia.RutaTxt);
Console.WriteLine($"Se cargaron {cargadosCsv} registro(s) desde el .csv y {cargadosTxt} desde el .txt (los Id repetidos solo se actualizan, no se duplican).");

// Se deja el estado inicial ya normalizado y sincronizado en ambos archivos.
GestorPersistencia.GuardarTodo(arbol.RecorrerInOrden());

MenuPrincipal menu = new MenuPrincipal(arbol);
menu.Ejecutar();
