#include <iostream>
#include <iomanip>
using namespace std;

int main() {
    string nombre;
    double precio;
    int cantidad;
    
    cout << "Ingrese el nombre completo del cliente: ";
    getline(cin, nombre);
    
    cout << "Ingrese el precio del producto: ";
    cin >> precio;
    
    cout << "Ingrese la cantidad comprada: ";
    cin >> cantidad;
    
    double subtotal = precio * cantidad;
    double iva = subtotal * 0.12;
    double total = subtotal + iva;
    
    cout << fixed << setprecision(2);
    cout << "\nCliente: " << nombre << endl;
    cout << "Subtotal: Q" << subtotal << endl;
    cout << "IVA: Q" << iva << endl;
    cout << "Total a pagar: Q" << total << endl;
    cout << "\nDesarrollado por: Israel Roberto Soto García" << endl;

    return 0;
}