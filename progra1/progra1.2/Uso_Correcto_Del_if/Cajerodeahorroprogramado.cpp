#include <iostream>
using namespace std;

int main() {
    int meses;
    float ahorro, total = 0, promedio;
    float mayor = 0;
    int mesMayor = 0;
    int meses500 = 0;

    cout << "===== CAJERO DE AHORRO PROGRAMADO =====" << endl;

    cout << "Ingrese la cantidad de meses a ahorrar: ";
    cin >> meses;

    if (meses <= 0) {
        cout << "Cantidad de meses invalida." << endl;
        return 0;
    }

    for (int i = 1; i <= meses; i++) {
        cout << "Ingrese el ahorro del mes #" << i << ": Q";
        cin >> ahorro;

        total += ahorro; // acumulador

        if (i == 1 || ahorro > mayor) {
            mayor = ahorro;
            mesMayor = i;
        }

        if (ahorro >= 500) {
            meses500++;
        }
    }

    promedio = total / meses;

    cout << "\n===== RESULTADOS =====" << endl;
    cout << "Total ahorrado: Q" << total << endl;
    cout << "Promedio mensual: Q" << promedio << endl;
    cout << "Mayor ahorro realizado en el mes: " << mesMayor << endl;
    cout << "Meses con ahorro de Q500 o mas: " << meses500 << endl;

    return 0;
}