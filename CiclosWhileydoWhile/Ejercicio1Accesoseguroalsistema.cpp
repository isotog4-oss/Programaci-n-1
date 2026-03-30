#include <iostream>
#include <string>
using namespace std;

int main() {
    string usuario;
    int password;
    int intentos = 0;

    cout << "Nombre del estudiante: Israel Roberto Soto Garcia\n\n";

    while (usuario != "admin" || password != 2026) {
        cout << "Ingrese usuario: ";
        cin >> usuario;

        cout << "Ingrese contraseña: ";
        cin >> password;

        if (usuario != "admin" || password != 2026) {
            cout << "Datos incorrectos.\n\n";
            intentos++;
        }
    }

    cout << "\nBienvenido al sistema.\n";
    cout << "Acceso concedido.\n";
    cout << "Intentos incorrectos: " << intentos << endl;

    return 0;
}