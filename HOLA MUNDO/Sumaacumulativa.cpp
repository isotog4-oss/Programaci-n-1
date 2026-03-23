#include <iostream>
using namespace std;

int main() {
    int num = 1;
    int suma = 0;

    while (num != 0) {
        cout << "Ingrese un numero (0 para terminar): ";
        cin >> num;

        suma += num;
    }

    cout << "La suma total es: " << suma << endl;

    return 0;
}