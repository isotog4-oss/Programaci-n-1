#include <iostream>
using namespace std;

int main() {
    int opcion;
    int numero;
    int contador = 0;
    int suma = 0;
    int pares = 0;
    int impares = 0;

    int numeros[100]; 

    do {
        cout << "\n===== SISTEMA DE GESTION SIMPLE =====\n";
        cout << "Nombre: Israel Roberto Soto Garcia\n";
        cout << "1. Registrar numeros\n";
        cout << "2. Mostrar estadisticas\n";
        cout << "3. Clasificar numeros\n";
        cout << "4. Salir\n";
        cout << "Seleccione una opcion: ";
        cin >> opcion;

        switch (opcion) {

            case 1: {
                char continuar;
                do {
                    cout << "Ingrese un numero: ";
                    cin >> numero;

                    
                    if (numero < 0) {
                        cout << "Error: No se permiten numeros negativos.\n";
                        continue;
                    }

                    
                    numeros[contador] = numero;
                    contador++;
                    suma += numero;

                    cout << "Desea ingresar otro numero? (s/n): ";
                    cin >> continuar;

                } while (continuar == 's' || continuar == 'S');
                break;
            }

            case 2:
                if (contador == 0) {
                    cout << "No hay datos disponibles.\n";
                } else {
                    cout << "Total de numeros: " << contador << endl;
                    cout << "Suma total: " << suma << endl;
                    cout << "Promedio: " << (float)suma / contador << endl;
                }
                break;

            case 3:
                if (contador == 0) {
                    cout << "No hay datos para clasificar.\n";
                } else {
                    pares = 0;
                    impares = 0;

                    for (int i = 0; i < contador; i++) {
                        if (numeros[i] % 2 == 0 && numeros[i] >= 0) {
                            pares++;
                        } else if (numeros[i] % 2 != 0 && numeros[i] >= 0) {
                            impares++;
                        }
                    }

                    cout << "Cantidad de numeros pares: " << pares << endl;
                    cout << "Cantidad de numeros impares: " << impares << endl;
                }
                break;

            case 4:
                cout << "Gracias por usar el sistema. ¡Hasta luego!\n";
                break;

            default:
                cout << "Opcion invalida.\n";
        }

    } while (opcion != 4);

    return 0;
}