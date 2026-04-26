#include <iostream>
#include <fstream>
#include <string>
#include <limits> // Para numeric_limits

using namespace std;

// --- Declaración de Funciones y Procedimientos ---

// Funciones para cálculos
double calcularSubtotal(double precio, int cantidad);
double calcularDescuento(double subtotal, int tipoPago); // 1: Efectivo, 2: Tarjeta, 3: Transferencia
double calcularTotal(double subtotal, double descuento);

// Procedimientos para la interfaz y lógica del programa
void mostrarMenu();
void registrarVenta();
void mostrarReporte();
void limpiarBufferEntrada(); // Procedimiento auxiliar para limpiar el buffer

// --- Variables Globales (si son necesarias y justificadas) ---
// En este caso, se evitarán variables globales para mantener un mejor control.

// --- Implementación de Funciones y Procedimientos ---

void mostrarMenu() {
    cout << "\n--- Menú Principal ---" << endl;
    cout << "1. Registrar Venta" << endl;
    cout << "2. Mostrar Reporte de Ventas" << endl;
    cout << "3. Salir" << endl;
    cout << "----------------------" << endl;
    cout << "Seleccione una opción: ";
}

double calcularSubtotal(double precio, int cantidad) {
    return precio * cantidad;
}

double calcularDescuento(double subtotal, int tipoPago) {
    double descuento = 0.0;
    switch (tipoPago) {
        case 1: // Efectivo
            descuento = subtotal * 0.05; // 5% de descuento
            break;
        case 2: // Tarjeta
            descuento = subtotal * 0.02; // 2% de descuento
            break;
        case 3: // Transferencia
            descuento = 0.0; // Sin descuento
            break;
        default:
            // Este caso no debería ocurrir si las validaciones son correctas,
            // pero se incluye por robustez.
            cout << "Tipo de pago desconocido para descuento." << endl;
            break;
    }
    return descuento;
}

double calcularTotal(double subtotal, double descuento) {
    return subtotal - descuento;
}

void registrarVenta() {
    string nombreCliente, nombreProducto;
    int cantidad;
    double precioUnitario;
    int tipoPago;
    double subtotal, descuento, total;

    cout << "\n--- Registrar Nueva Venta ---" << endl;

    // Solicitar y validar Nombre del Cliente
    cout << "Nombre del cliente: ";
    getline(cin, nombreCliente);

    // Solicitar y validar Nombre del Producto
    cout << "Nombre del producto: ";
    getline(cin, nombreProducto);

    // Solicitar y validar Cantidad
    while (true) {
        cout << "Cantidad: ";
        cin >> cantidad;
        if (cin.fail() || cantidad <= 0) {
            cout << "Error: La cantidad debe ser un número entero mayor que 0." << endl;
            cin.clear();
            limpiarBufferEntrada();
            continue; // Vuelve a pedir la cantidad
        }
        limpiarBufferEntrada(); // Limpiar buffer después de una lectura exitosa
        break; // Salir del bucle si la entrada es válida
    }

    // Solicitar y validar Precio Unitario
    while (true) {
        cout << "Precio unitario: ";
        cin >> precioUnitario;
        if (cin.fail() || precioUnitario <= 0) {
            cout << "Error: El precio debe ser un número mayor que 0." << endl;
            cin.clear();
            limpiarBufferEntrada();
            continue; // Vuelve a pedir el precio
        }
        limpiarBufferEntrada(); // Limpiar buffer después de una lectura exitosa
        break; // Salir del bucle si la entrada es válida
    }

    // Solicitar y validar Tipo de Pago
    while (true) {
        cout << "Tipo de pago (1: Efectivo, 2: Tarjeta, 3: Transferencia): ";
        cin >> tipoPago;
        if (cin.fail() || (tipoPago != 1 && tipoPago != 2 && tipoPago != 3)) {
            cout << "Error: El tipo de pago debe ser 1, 2 o 3." << endl;
            cin.clear();
            limpiarBufferEntrada();
            continue; // Vuelve a pedir el tipo de pago
        }
        limpiarBufferEntrada(); // Limpiar buffer después de una lectura exitosa
        break; // Salir del bucle si la entrada es válida
    }

    // --- Cálculos ---
    subtotal = calcularSubtotal(precioUnitario, cantidad);
    descuento = calcularDescuento(subtotal, tipoPago);
    total = calcularTotal(subtotal, descuento);

    // --- Escritura en Archivo ---
    ofstream archivoVentas("ventas.txt", ios::app); // 'app' para agregar al final

    if (archivoVentas.is_open()) {
        archivoVentas << nombreCliente << ","
                      << nombreProducto << ","
                      << cantidad << ","
                      << precioUnitario << ","
                      << subtotal << ","
                      << descuento << ","
                      << total << ",";

        switch (tipoPago) {
            case 1: archivoVentas << "Efectivo" << endl; break;
            case 2: archivoVentas << "Tarjeta" << endl; break;
            case 3: archivoVentas << "Transferencia" << endl; break;
        }
        archivoVentas.close();
        cout << "\nVenta registrada exitosamente." << endl;
    } else {
        cout << "Error: No se pudo abrir el archivo para registrar la venta." << endl;
    }
}

void mostrarReporte() {
    ifstream archivoVentas("ventas.txt");
    string linea;
    int numeroVenta = 1;

    cout << "\n--- Reporte de Ventas ---" << endl;

    if (archivoVentas.is_open()) {
        if (getline(archivoVentas, linea)) { // Intenta leer la primera línea
            do {
                cout << numeroVenta << ". " << linea << endl;
                numeroVenta++;
            } while (getline(archivoVentas, linea)); // Lee las siguientes líneas
            archivoVentas.close();
        } else {
            cout << "No hay ventas registradas para mostrar." << endl;
            archivoVentas.close();
        }
    } else {
        cout << "Error: No se pudo abrir el archivo para mostrar el reporte." << endl;
    }
    cout << "-------------------------" << endl;
}

void limpiarBufferEntrada() {
    cin.ignore(numeric_limits<streamsize>::max(), '\n');
}

// --- Función Principal (main) ---
int main() {
    int opcion;

    do {
        mostrarMenu();
        cin >> opcion;

        if (cin.fail()) {
            cout << "Entrada inválida. Por favor, ingrese un número." << endl;
            cin.clear(); // Limpiar el estado de error
            limpiarBufferEntrada(); // Descartar la entrada incorrecta
            continue; // Volver al inicio del bucle
        }
        limpiarBufferEntrada(); // Limpiar el buffer después de leer la opción

        switch (opcion) {
            case 1:
                registrarVenta();
                break;
            case 2:
                mostrarReporte();
                break;
            case 3:
                cout << "Saliendo del programa. ¡Hasta luego!" << endl;
                break;
            default:
                cout << "Opción no válida. Por favor, intente de nuevo." << endl;
                continue; // Usar continue para volver a mostrar el menú
        }
    } while (opcion != 3); // El bucle continúa hasta que el usuario elija la opción 3 (Salir)

    return 0;
}