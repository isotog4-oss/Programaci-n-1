#include <iostream>
using namespace std;

int main() {
    double monto;
    int producto;

    cout << "Ingrese el monto del Producto: ";
    cin >> monto;

    cout << "Ingrese el número del producto que desea comprar: ";
    cin >> producto;

    cout << "El total a pagar es: " << monto * producto << endl;


    return 0;
}