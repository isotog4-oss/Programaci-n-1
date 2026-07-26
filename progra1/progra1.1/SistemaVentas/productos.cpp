#include "productos.h"
#include "utilidades.h"
#include <iostream>
#include <fstream>
#include <cstring>

const char* FILE_PROD = "productos.dat";

void registrarProducto() {
    std::ofstream archivo(FILE_PROD, std::ios::binary | std::ios::app);
    if (!archivo) return;

    Producto p;
    p.codigo = Util::leerEntero("Codigo unico: ");
    
    // Validar duplicado
    Producto temp;
    if (obtenerProducto(p.codigo, temp)) {
        std::cout << "Error: El codigo ya existe.\n";
        return;
    }

    Util::leerCadena("Nombre: ", p.nombre, MAX_NOM);
    p.precio = Util::leerDouble("Precio (Q): ");
    p.stock = Util::leerEntero("Stock inicial: ");
    p.ventasAcumuladas = 0;
    p.activo = true;

    archivo.write(reinterpret_cast<char*>(&p), sizeof(Producto));
    archivo.close();
    std::cout << "Producto registrado.\n";
}

void listarProductos() {
    std::ifstream archivo(FILE_PROD, std::ios::binary);
    Producto p;
    std::cout << "\nID\tNombre\t\tPrecio\tStock\n";
    while (archivo.read(reinterpret_cast<char*>(&p), sizeof(Producto))) {
        if (p.activo) {
            std::cout << p.codigo << "\t" << p.nombre << "\tQ" << p.precio << "\t" << p.stock << "\n";
        }
    }
    archivo.close();
}

bool obtenerProducto(int codigo, Producto &p) {
    std::ifstream archivo(FILE_PROD, std::ios::binary);
    if (!archivo) return false;
    while (archivo.read(reinterpret_cast<char*>(&p), sizeof(Producto))) {
        if (p.codigo == codigo && p.activo) {
            archivo.close();
            return true;
        }
    }
    archivo.close();
    return false;
}

void actualizarStock() {
    std::fstream archivo(FILE_PROD, std::ios::binary | std::ios::in | std::ios::out);
    int cod = Util::leerEntero("Codigo a actualizar: ");
    Producto p;
    bool hallado = false;

    while (archivo.read(reinterpret_cast<char*>(&p), sizeof(Producto))) {
        if (p.codigo == cod && p.activo) {
            int mod = Util::leerEntero("Cantidad (+/-): ");
            if (p.stock + mod < 0) {
                std::cout << "Error: Stock insuficiente.\n";
            } else {
                p.stock += mod;
                // Corregir puntero de escritura
                archivo.seekp((long)archivo.tellg() - sizeof(Producto));
                archivo.write(reinterpret_cast<char*>(&p), sizeof(Producto));
                std::cout << "Stock actualizado.\n";
            }
            hallado = true;
            break;
        }
    }
    if (!hallado) std::cout << "No encontrado.\n";
    archivo.close();
}

void eliminarProducto() {
    std::fstream archivo(FILE_PROD, std::ios::binary | std::ios::in | std::ios::out);
    int cod = Util::leerEntero("Codigo a eliminar: ");
    Producto p;
    while (archivo.read(reinterpret_cast<char*>(&p), sizeof(Producto))) {
        if (p.codigo == cod && p.activo) {
            p.activo = false;
            archivo.seekp((long)archivo.tellg() - sizeof(Producto));
            archivo.write(reinterpret_cast<char*>(&p), sizeof(Producto));
            std::cout << "Producto dado de baja.\n";
            break;
        }
    }
    archivo.close();
}
