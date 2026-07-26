#include <iostream>
#include <string>

using namespace std;


struct Mascota {
    string nombre;
    string especie;
    string raza;
    int edad;
    float peso;
};


void registrarMascota(Mascota &m) {
    cout << "\n===== REGISTRO DE MASCOTA =====\n";

    cin.ignore();

    cout << "Ingrese el nombre: ";
    getline(cin, m.nombre);

    cout << "Ingrese la especie: ";
    getline(cin, m.especie);

    cout << "Ingrese la raza: ";
    getline(cin, m.raza);

    cout << "Ingrese la edad: ";
    cin >> m.edad;

    cout << "Ingrese el peso en kg: ";
    cin >> m.peso;

    cout << "\nMascota registrada correctamente.\n";
}


void mostrarMascota(Mascota m) {
    cout << "\n===== DATOS DE LA MASCOTA =====\n";
    cout << "Nombre: " << m.nombre << endl;
    cout << "Especie: " << m.especie << endl;
    cout << "Raza: " << m.raza << endl;
    cout << "Edad: " << m.edad << " anios" << endl;
    cout << "Peso: " << m.peso << " kg" << endl;
}


void calcularEdadHumana(Mascota m) {
    int edadHumana = m.edad * 7;

    cout << "\n===== EDAD EQUIVALENTE =====\n";
    cout << "La edad equivalente en anios humanos es: "
         << edadHumana << " anios.\n";
}


void clasificarPeso(Mascota m) {
    cout << "\n===== CLASIFICACION POR PESO =====\n";

    if (m.peso < 5) {
        cout << "Mascota pequena.\n";
    }
    else if (m.peso >= 5 && m.peso <= 20) {
        cout << "Mascota mediana.\n";
    }
    else {
        cout << "Mascota grande.\n";
    }
}


void modificarDatos(Mascota &m) {
    int opcion;

    cout << "\n===== MODIFICAR DATOS =====\n";
    cout << "1. Nombre\n";
    cout << "2. Especie\n";
    cout << "3. Raza\n";
    cout << "4. Edad\n";
    cout << "5. Peso\n";
    cout << "Seleccione una opcion: ";
    cin >> opcion;

    cin.ignore();

    switch(opcion) {
        case 1:
            cout << "Nuevo nombre: ";
            getline(cin, m.nombre);
            break;

        case 2:
            cout << "Nueva especie: ";
            getline(cin, m.especie);
            break;

        case 3:
            cout << "Nueva raza: ";
            getline(cin, m.raza);
            break;

        case 4:
            cout << "Nueva edad: ";
            cin >> m.edad;
            break;

        case 5:
            cout << "Nuevo peso: ";
            cin >> m.peso;
            break;

        default:
            cout << "Opcion invalida.\n";
    }

    cout << "Dato modificado correctamente.\n";
}


int main() {
    Mascota mascota;
    int opcion;

    do {
        cout << "\n===== REGISTRO DE MASCOTA =====\n";
        cout << "1. Registrar mascota\n";
        cout << "2. Mostrar mascota\n";
        cout << "3. Calcular edad equivalente\n";
        cout << "4. Clasificar por peso\n";
        cout << "5. Modificar datos\n";
        cout << "6. Salir\n";
        cout << "Seleccione una opcion: ";
        cin >> opcion;

        switch(opcion) {

            case 1:
                registrarMascota(mascota);
                break;

            case 2:
                mostrarMascota(mascota);
                break;

            case 3:
                calcularEdadHumana(mascota);
                break;

            case 4:
                clasificarPeso(mascota);
                break;

            case 5:
                modificarDatos(mascota);
                break;

            case 6:
                cout << "\nSaliendo del programa...\n";
                break;

            default:
                cout << "\nOpcion invalida.\n";
        }

    } while(opcion != 6);

    return 0;
}