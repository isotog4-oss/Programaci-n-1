#include <iostream>
using namespace std;

int main() {
    int pass, intentos = 0;

    while (intentos < 3) {
        cout << "Ingrese la contraseña: ";
        cin >> pass;

        if (pass == 1234) {
            cout << "Bienvenido\n";
            return 0;
        }

        intentos++;
        cout << "Contraseña incorrecta\n";
    }

    cout << "Acceso denegado\n";

    return 0;
}