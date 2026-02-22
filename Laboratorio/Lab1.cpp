#include <iostream>
using namespace std;
int main() {

    string nombre; 
    int edad;
    float estatura;
    

    cout << "Ingrese su nombre: ";
    cin >> nombre;


    cout << "Ingrese su edad: ";
    cin >> edad;

    cout << "Ingrese su estatura: ";
    cin >> estatura;

    
    cout << "El resumen de lo ingresado es " << nombre << ", " << edad << ", " << estatura << endl;

    return 0;
}
