#include <iostream>
using namespace std;

int main() {
    int opcion;
    int malo = 0, regular = 0, bueno = 0, excelente = 0;
    int total = 0;

    cout << "Nombre del estudiante: Israel Roberto Soto Garcia\n\n";
    cout << "=== ENCUESTA DE SATISFACCION ===\n";

    do {
        cout << "\n1 = Malo\n";
        cout << "2 = Regular\n";
        cout << "3 = Bueno\n";
        cout << "4 = Excelente\n";
        cout << "0 = Finalizar\n";
        cout << "Ingrese calificacion: ";
        cin >> opcion;

        switch (opcion) {
            case 1:
                malo++;
                total++;
                break;

            case 2:
                regular++;
                total++;
                break;

            case 3:
                bueno++;
                total++;
                break;

            case 4:
                excelente++;
                total++;
                break;

            case 0:
                cout << "\nFinalizando encuesta...\n";
                break;

            default:
                cout << "Opcion invalida. Intente nuevamente.\n";
        }

    } while (opcion != 0);

    if (total > 0) {
        cout << "\n=== RESULTADOS ===\n";
        cout << "Total de respuestas: " << total << endl;

        cout << "Malo: " << malo 
             << " (" << (malo * 100.0 / total) << "%)\n";

        cout << "Regular: " << regular 
             << " (" << (regular * 100.0 / total) << "%)\n";

        cout << "Bueno: " << bueno 
             << " (" << (bueno * 100.0 / total) << "%)\n";

        cout << "Excelente: " << excelente 
             << " (" << (excelente * 100.0 / total) << "%)\n";

    } else {
        cout << "\nNo se ingresaron respuestas validas.\n";
    }

    return 0;
}