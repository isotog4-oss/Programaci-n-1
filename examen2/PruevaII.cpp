#include <iostream>
#include <fstream>
#include <string>
#include <limits> 

using namespace std;

double calcularSubtotal(double precio, int cantidad);
double calcularDescuento(double subtotal, int tipoPago); 
double calcularTotal(double subtotal, double descuento);

void mostrarMenu();
void registrarVenta();
void mostrarReporte();
void limpiarBufferEntrada(); 

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
        case 1: 
            descuento = subtotal * 0.05; 
            break;
        case 2: 
            descuento = subtotal * 0.02; 
            break;
        case 3: 
            descuento = 0.0; 
            break;
        default:
     
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

   
    cout << "Nombre del cliente: ";
    getline(cin, nombreCliente);

  
    cout << "Nombre del producto: ";
    getline(cin, nombreProducto);

 
    while (true) {
        cout << "Cantidad: ";
        cin >> cantidad;
        if (cin.fail() || cantidad <= 0) {
            cout << "Error: La cantidad debe ser un número entero mayor que 0." << endl;
            cin.clear();
            limpiarBufferEntrada();
            continue;
        }
        limpiarBufferEntrada(); 
    }

  
    while (true) {
        cout << "Precio unitario: ";
        cin >> precioUnitario;
        if (cin.fail() || precioUnitario <= 0) {
            cout << "Error: El precio debe ser un número mayor que 0." << endl;
            cin.clear();
            limpiarBufferEntrada();
            continue; 
        }
        limpiarBufferEntrada(); 
    }

  
    while (true) {
        cout << "Tipo de pago (1: Efectivo, 2: Tarjeta, 3: Transferencia): ";
        cin >> tipoPago;
        if (cin.fail() || (tipoPago != 1 && tipoPago != 2 && tipoPago != 3)) {
            cout << "Error: El tipo de pago debe ser 1, 2 o 3." << endl;
            cin.clear();
            limpiarBufferEntrada();
            continue; 
        }
        limpiarBufferEntrada();
    }

   
    subtotal = calcularSubtotal(precioUnitario, cantidad);
    descuento = calcularDescuento(subtotal, tipoPago);
    total = calcularTotal(subtotal, descuento);

    
    ofstream archivoVentas("ventas.txt", ios::app); 

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
        if (getline(archivoVentas, linea)) { 
            do {
                cout << numeroVenta << ". " << linea << endl;
                numeroVenta++;
            } while (getline(archivoVentas, linea)); 
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


int main() {
    int opcion;

    do {
        mostrarMenu();
        cin >> opcion;

        if (cin.fail()) {
            cout << "Entrada inválida. Por favor, ingrese un número." << endl;
            cin.clear(); 
            limpiarBufferEntrada(); 
            continue; 
        }
        limpiarBufferEntrada(); 

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
                continue; 
        }
    } while (opcion != 3); 

    return 0;
}