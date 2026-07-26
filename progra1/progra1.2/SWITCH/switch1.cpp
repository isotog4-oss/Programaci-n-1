#include <iostream>
using namespace std;

int main() {
    int opcion;
    int numero;

    cout << "Nombre del estudiante: Israel Roberto Soto Garcia\n\n";

    do {
        cout << "\n=== MENU DE OPCIONES ===\n";
        cout << "1. Mostrar los numeros del 1 al 5\n";
        cout << "2. Mostrar numeros pares del 2 al 10\n";
        cout << "3. Ingresar un numero y evaluar\n";
        cout << "4. Salir\n";
        cout << "Seleccione una opcion: ";
        cin >> opcion;

        // Validación antes del switch (para usar continue)
        if (opcion < 1 || opcion > 4) {
            cout << "Opcion invalida. Intente nuevamente.\n";
            continue; // vuelve al inicio del menú
        }

        switch (opcion) {

            case 1:
                cout << "\nNumeros del 1 al 5:\n";
                for (int i = 1; i <= 5; i++) {
                    cout << i << " ";
                }
                cout << endl;
                break;

            case 2:
                cout << "\nNumeros pares del 2 al 10:\n";
                for (int i = 2; i <= 10; i += 2) {
                    cout << i << " ";
                }
                cout << endl;
                break;

            case 3:
                cout << "\nIngrese un numero: ";
                cin >> numero;

                if (numero > 0) {
                    cout << "El numero es positivo.\n";
                } else if (numero < 0) {
                    cout << "El numero es negativo.\n";
                } else {
                    cout << "El numero es cero.\n";
                }
                break;

            case 4:
                cout << "\nGracias por usar el programa. Hasta luego.\n";
                break;
        }

    } while (opcion != 4);

    return 0;
}

