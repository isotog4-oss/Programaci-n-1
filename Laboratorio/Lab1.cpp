#include <iostream>
using namespace std;
int main() {

    string nombre; 
    int edad;
    float estatura;
    

    cout << "Ingrese su nombre: ";
    getline(cin, nombre);


    cout << "Ingrese su edad: ";
    cin >> edad;

    cout << "Ingrese su estatura: ";
    cin >> estatura;

    
    cout << "Su nombre es " << nombre <<  endl;
    cout << "Su edad es " << edad <<  endl;
    cout << "Su estatura es " << estatura <<  endl;

    return 0;
}
