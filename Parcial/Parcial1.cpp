#include <iostream>
#include <iomanip>
#include <string>

using namespace std;

int main() {

    // Declaración de variables
    string nombreCliente;
    double precioProducto;
    int cantidadComprada;
    double subtotal;
    double iva;
    double total;

    // Entrada de datos
    cout << "Ingrese el nombre del cliente: ";
    getline(cin, nombreCliente);

    cout << "Ingrese el precio del producto: ";
    cin >> precioProducto;

    cout << "Ingrese la cantidad comprada: ";
    cin >> cantidadComprada;

 
    subtotal = precioProducto * cantidadComprada;
    iva = subtotal * 0.12;
    total = subtotal + iva;

    cout << fixed << setprecision(2);

    cout << "\nCliente: " << nombreCliente << endl;
    cout << "Subtotal: Q" << subtotal << endl;
    cout << "IVA: Q" << iva << endl;
    cout << "Total a pagar: Q" << total << endl;

    cout << "\nEstudiante: Israel Roberto Soto Garcia" << endl;

    return 0;
}