#include <iostream>
using namespace std;

int main() {
    double monto;
    double Porcentaje;

    cout << "Monto Inicial: ";
    cin >> monto;

    cout << "Porsentaje de incremento: ";
    cin >> Porcentaje;

 
    double Total = monto +(monto * Porcentaje / 100);


    cout << "El monto total con incremento es: " << Total << endl;


    return 0;
}