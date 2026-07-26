#include <iostream>
using namespace std;
int main() {

    string nombre;
    char apellido; 
    int edad;
    float estatura;
    double monto;

    cout << "Ingrese su nombre: ";
    cin >> nombre;

    cout << "Ingrese la inicial desu apellido: ";
    cin >> apellido;

    cout << "Ingrese su edad: ";
    cin >> edad;

    cout << "Ingrese su estatura: ";
    cin >> estatura;

    cout << "Ingrese el precio del producto que quiere adquirir: ";
    cin >> monto;
    
    cout << "El resumen de lo ingresado es " << nombre << ", " << apellido << ", " << edad << ", " << estatura << ", " << monto << endl;

    return 0;
}