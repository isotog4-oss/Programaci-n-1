#include <iostream>
#include <fstream>

using namespace std;


struct Producto {
    int codigo;
    char nombre[40];
    char categoria[30];
    int stock;
    float precio;
    bool activo;
};

void menu();
void registrarProducto();
void listarProductos();
void mostrarEncabezado();
void pausarSistema();
void verificarYCrearArchivo(); 

int main() {
    verificarYCrearArchivo(); 
    menu();
    return 0;
}

void verificarYCrearArchivo() {
   
    ifstream comprobar("productos.dat", ios::binary);
    
    if (!comprobar) {
        ofstream nuevoArchivo("productos.dat", ios::binary | ios::app);
        nuevoArchivo.close();
    } else {
        comprobar.close();
    }
}

void menu() {
    int opcion;
    do {
        mostrarEncabezado();
        cout << "1. Registrar producto\n";
        cout << "2. Listar productos\n";
        cout << "3. Salir\n";
        cout << "===============================\n";
        cout << "Seleccione una opcion: ";
        
        if (!(cin >> opcion)) {
            cout << "\n[!] Error: Debe ingresar un numero entero.\n";
            cin.clear();
            cin.ignore(10000, '\n');
            pausarSistema();
            continue;
        }

        switch(opcion) {
            case 1:
                registrarProducto();
                break;
            case 2:
                listarProductos();
                break;
            case 3:
                cout << "\nPrograma finalizado correctamente. ¡Hasta luego!\n";
                break;
            default:
                cout << "\nOpcion invalida. Intente de nuevo.\n";
                pausarSistema();
        }
    } while(opcion != 3);
}

void mostrarEncabezado() {
    cout << "\n===========================================================\n";
    cout << " SISTEMA DE VENTAS E INVENTARIO - Israel Roberto Soto Garcia\n";
    cout << "===========================================================\n";
}

void pausarSistema() {
    cout << "\nPresione Enter para continuar...";
    cin.ignore(10000, '\n'); 
}

void registrarProducto() {
    Producto producto;
    ofstream archivo("productos.dat", ios::binary | ios::app);

    if(!archivo) {
        cout << "\nError: No se pudo abrir el archivo productos.dat\n";
        pausarSistema();
        return;
    }

    cout << "\n>>> REGISTRAR NUEVO PRODUCTO <<<\n";
    cout << "Codigo (Entero): ";
    cin >> producto.codigo;
    cin.ignore(10000, '\n'); 

    cout << "Nombre del producto: ";
    cin.getline(producto.nombre, 40);

    cout << "Categoria: ";
    cin.getline(producto.categoria, 30);

    cout << "Stock inicial: ";
    cin >> producto.stock;

    cout << "Precio unitario: ";
    cin >> producto.precio;
    cin.ignore(10000, '\n'); 

    producto.activo = true; 

  
    archivo.write((char*)&producto, sizeof(Producto));
    archivo.close();

    cout << "\n[OK] Producto registrado y guardado exitosamente.\n";
    pausarSistema();
}

void listarProductos() {
    Producto producto;
    ifstream archivo("productos.dat", ios::binary);

    if(!archivo) {
        cout << "\n[!] No existe informacion almacenada.\n";
        pausarSistema();
        return;
    }

    cout << "\n=======================================================\n";
    cout << "                 LISTADO DE PRODUCTOS                  \n";
    cout << "=======================================================\n";

    bool registrosEncontrados = false;

    while(archivo.read((char*)&producto, sizeof(Producto))) {
        if(producto.activo) { 
            registrosEncontrados = true;
            cout << "Codigo:    " << producto.codigo << "\n";
            cout << "Nombre:    " << producto.nombre << "\n";
            cout << "Categoria: " << producto.categoria << "\n";
            cout << "Stock:     " << producto.stock << " unidades\n";
            cout << "Precio:    Q" << producto.precio << "\n"; 
            cout << "-------------------------------------------------------\n";
        }
    }

    if(!registrosEncontrados) {
        cout << "No hay productos registrados en la base de datos actualmente.\n";
        cout << "-------------------------------------------------------\n";
    }

    archivo.close();
  
    cout << "\nPresione Enter para continuar...";
    cin.get(); 
}