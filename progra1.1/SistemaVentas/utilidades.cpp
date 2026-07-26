#include "utilidades.h"
#include <iostream>
#include <limits>
#include <stdexcept>

void Util::mostrarEncabezado() {
    std::cout << "==================================================\n";
    std::cout << "     SISTEMA DE GESTION DE VENTAS E INVENTARIO    \n";
    std::cout << "     Desarrollador: Israel Soto Garcia\n";
    std::cout << "==================================================\n";
}

int Util::leerEntero(const char* mensaje) {
    int valor;
    while (true) {
        std::cout << mensaje;
        if (std::cin >> valor) {
            std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n'); 
            return valor;
        }
        std::cout << "[Error]: Ingrese un numero entero valido.\n";
        std::cin.clear();
        std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
    }
}

double Util::leerDouble(const char* mensaje) {
    double valor;
    while (true) {
        std::cout << mensaje;
        if (std::cin >> valor && valor >= 0) {
            std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n'); 
            return valor;
        }
        std::cout << "[Error]: Ingrese un valor decimal positivo.\n";
        std::cin.clear();
        std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
    }
}

void Util::leerCadena(const char* mensaje, char* destino, int tamano) {
    std::cout << mensaje;
    std::cin.getline(destino, tamano); 
}

void Util::limpiarPantalla() {
#ifdef _WIN32
    system("cls");
#else
    system("clear");
#endif
}

void Util::pausar() {
    std::cout << "\nPresione Enter para continuar...";
    std::cin.get(); 
}