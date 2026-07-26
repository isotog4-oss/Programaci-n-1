#include <iostream>
using namespace std;

int main() {
    int num;

    cout << "Ingrese un numero entero mayor que 1: ";
    cin >> num;

    while (num <= 1) {
        cout << "Numero invalido. Intente de nuevo: ";
        cin >> num;
    }

    while (num >= 1) {
        cout << num << endl;
        num--;
    }

    return 0;
}