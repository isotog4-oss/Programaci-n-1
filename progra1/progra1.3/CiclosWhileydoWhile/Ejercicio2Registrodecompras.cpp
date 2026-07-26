#include <iostream>
using namespace std;

int main() {
    float monto, total = 0;
    int cantidad = 0;
    int mayores100 = 0, menores100 = 0;

    cout << "Nombre del estudiante: Israel Roberto Soto Garcia\n\n";

    cout << "Ingrese monto de compra (0 para finalizar): ";
    cin >> monto;

    while (monto != 0) {

        if (monto < 0) {
            cout << "Monto invalido. Intente nuevamente.\n";
        } else {
            total += monto;
            cantidad++;

            if (monto >= 100) {
                mayores100++;
            } else {
                menores100++;
            }
        }

        cout << "Ingrese monto de compra (0 para finalizar): ";
        cin >> monto;
    }

    if (cantidad > 0) {
        cout << "\nTotal acumulado: Q" << total << endl;
        cout << "Cantidad de compras: " << cantidad << endl;
        cout << "Promedio de compra: Q" << (total / cantidad) << endl;
        cout << "Compras >= Q100: " << mayores100 << endl;
        cout << "Compras < Q100: " << menores100 << endl;
    } else {
        cout << "\nNo se ingresaron compras validas.\n";
    }

    return 0;
}