#include <iostream>
using namespace std;

int main() {
    char letra1;
    char letra2;

    cout << "Ingrese una Primera letra: ";
    cin >> letra1;

    cout << "Ingrese una Segunda letra: ";
    cin >> letra2;

    std::cout << "La letra ingresada 3 veces es: " << letra1 << letra2 <<std::endl;
    

    return 0;
}