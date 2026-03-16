#include <iostream>
#include <string>
#include <iomanip>

using namespace std;

int main() {

    string nombre;
    int edad;
    float promedio;
    int club;

    cout << "Ingrese su nombre: ";
    getline(cin, nombre);

    // Regla secreta
    if(nombre == "David Alvarez"){
        cout << "Acceso especial concedido." << endl;
        return 0;
    }

    cout << "Ingrese su edad: ";
    cin >> edad;

    cout << "Ingrese su promedio academico: ";
    cin >> promedio;

    cout << "Pertenece al club de programacion? (1 = Si, 0 = No): ";
    cin >> club;

    cout << fixed << setprecision(2);
    cout << "Promedio ingresado: " << promedio << endl;

    if(edad < 18){
        cout << "Acceso denegado." << endl;
    }
    else{

        if((club == 1 && promedio >= 75) ||
           (club == 0 && promedio >= 85) ||
           (promedio >= 95)){
            
            cout << "Acceso autorizado." << endl;
        }
        else{
            cout << "Acceso denegado." << endl;
        }

    }

    return 0;
}