#include <iostream>
using namespace std;

int main() {
   
    char letra;

    cout << "Ingrese una letra: ";
    cin >> letra;

    cout << "La letra ingresada es: " << letra << endl;

    std::cout << "La letra ingresada 3 veces: " << letra << letra << letra << std::endl;

    cout << "Valor asignado: " << int(letra) << endl;

    return 0;
}