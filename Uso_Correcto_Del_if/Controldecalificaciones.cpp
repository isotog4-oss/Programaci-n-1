#include <iostream>
using namespace std;

int main() {
    float nota, suma = 0, promedio;
    int aprobadas = 0, reprobadas = 0;

    cout << "===== CONTROL DE CALIFICACIONES =====" << endl;

    for (int i = 1; i <= 5; i++) {
        cout << "Ingrese la nota #" << i << ": ";
        cin >> nota;

        suma += nota; // acumulador

        if (nota >= 61) {
            aprobadas++;
        } else {
            reprobadas++;
        }
    }

    promedio = suma / 5;

    cout << "\n===== RESULTADOS =====" << endl;
    cout << "Suma total: " << suma << endl;
    cout << "Promedio: " << promedio << endl;
    cout << "Notas aprobadas: " << aprobadas << endl;
    cout << "Notas reprobadas: " << reprobadas << endl;

    if (promedio >= 61) {
        cout << "Resultado final: Aprueba" << endl;
    } else {
        cout << "Resultado final: Reprueba" << endl;
    }

    return 0;
}