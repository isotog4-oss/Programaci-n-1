#include <iostream>
using namespace std;

int main() {
    float altura;
    float base;
    int area;

    cout << "Ingrese la altura del triángulo: ";
    cin >> altura;
    
    cout << "Ingrese la base del triángulo: ";
    cin >> base;

    area = (base * altura) / 2;

    cout << "El área del triángulo es: " << area << endl;

    return 0;
}