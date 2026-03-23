#include <iostream>
using namespace std;

int main() {
    int pass;

    cout << "Ingrese la contraseña: ";
    cin >> pass;

    while (pass != 1234) {
        cout << "Contraseña incorrecta. Intente de nuevo: ";
        cin >> pass;
    }

    cout << "Acceso concedido." << endl;

    return 0;
}