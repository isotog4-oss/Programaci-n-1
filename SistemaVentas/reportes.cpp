#include "reportes.h"
#include "modelos.h"
#include <iostream>
#include <fstream>
#include <iomanip>

void reportarVentasPorMes() {
    double matriz[12][31] = {0}; 
    std::ifstream archivo("ventas.dat", std::ios::binary);
    if (!archivo) return;

    Venta v;
    while (archivo.read(reinterpret_cast<char*>(&v), sizeof(Venta))) {
        if (v.mes >= 1 && v.mes <= 12 && v.dia >= 1 && v.dia <= 31)
            matriz[v.mes-1][v.dia-1] += v.total;
    }
    archivo.close();

    std::cout << "\nRECAUDACION MENSUAL:\n";
    const char* meses[] = {"Ene","Feb","Mar","Abr","May","Jun","Jul","Ago","Sep","Oct","Nov","Dic"};
    for (int i = 0; i < 12; i++) {
        double totalMes = 0;
        for (int j = 0; j < 31; j++) totalMes += matriz[i][j];
        if (totalMes > 0)
            std::cout << meses[i] << ": Q" << std::fixed << std::setprecision(2) << totalMes << "\n";
    }
}

void ordenarCatálogo() {
    
    Producto lista[500];
    int n = 0;
    std::ifstream f("productos.dat", std::ios::binary);
    while (f.read(reinterpret_cast<char*>(&lista[n]), sizeof(Producto))) {
        if (lista[n].activo) n++;
    }
    f.close();

    
    for (int i = 0; i < n - 1; i++) {
        for (int j = 0; j < n - i - 1; j++) {
            if (lista[j].precio > lista[j+1].precio) {
                Producto aux = lista[j];
                lista[j] = lista[j+1];
                lista[j+1] = aux;
            }
        }
    }

    std::cout << "CATALOGO ORDENADO POR PRECIO:\n";
    for (int i = 0; i < n; i++)
        std::cout << lista[i].nombre << " - Q" << lista[i].precio << "\n";
}

void exportarTexto() {
    std::ifstream bin("productos.dat", std::ios::binary);
    std::ofstream txt("Reporte_Inventario.txt");
    Producto p;
    txt << "SISTEMA INVENTARIO - REPORTE\n\n";
    while (bin.read(reinterpret_cast<char*>(&p), sizeof(Producto))) {
        if (p.activo)
            txt << "COD: " << p.codigo << " | " << p.nombre << " | Stock: " << p.stock << "\n";
    }
    bin.close(); txt.close();
    std::cout << "Exportado a 'Reporte_Inventario.txt'.\n";
}