#include <iostream>
using namespace std;

int main() {
    float saldo = 500;
    int opcion;
    float monto;

    cout << "Nombre del estudiante: Israel Roberto Soto Garcia\n\n";

    do {
        cout << "\n1. Consultar saldo\n";
        cout << "2. Depositar dinero\n";
        cout << "3. Retirar dinero\n";
        cout << "4. Salir\n";
        cout << "Seleccione una opcion: ";
        cin >> opcion;

        switch (opcion) {
            case 1:
                cout << "Saldo actual: Q" << saldo << endl;
                break;

            case 2:
                cout << "Ingrese monto a depositar: ";
                cin >> monto;
                if (monto > 0) {
                    saldo += monto;
                    cout << "Deposito realizado correctamente.\n";
                } else {
                    cout << "Monto invalido.\n";
                }
                break;

            case 3:
                cout << "Ingrese monto a retirar: ";
                cin >> monto;
                if (monto > saldo) {
                    cout << "Fondos insuficientes.\n";
                } else if (monto > 0) {
                    saldo -= monto;
                    cout << "Retiro realizado correctamente.\n";
                } else {
                    cout << "Monto invalido.\n";
                }
                break;

            case 4:
                cout << "Gracias por usar el sistema.\n";
                break;

            default:
                cout << "Opcion invalida.\n";
        }

    } while (opcion != 4);

    return 0;
}