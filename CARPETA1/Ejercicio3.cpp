#include <iostream>
using namespace std;

int main() {
    float estatura;
    float kilogramo;
    int IMC;

    cout << "Ingrese su estatura en metros: ";
    cin >> estatura;
    
    cout << "Ingrese su peso en kilogramos: ";
    cin >> kilogramo;

    IMC = kilogramo / (estatura * estatura);

    cout << "Su IMC es: " << IMC << endl;

    return 0;
}