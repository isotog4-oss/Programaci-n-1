#include "ventas.h"
#include "productos.h"
#include "utilidades.h"
#include <iostream>
#include <fstream>
#include <cmath>
#include <limits> 

void descontarStock(int codigo, int cant) {
    std::fstream archivo("productos.dat", std::ios::binary | std::ios::in | std::ios::out);
    Producto p;
    while (archivo.read(reinterpret_cast<char*>(&p), sizeof(Producto))) {
        if (p.codigo == codigo) {
            p.stock -= cant;
            p.ventasAcumuladas += cant;
            archivo.seekp((long)archivo.tellg() - sizeof(Producto));
            archivo.write(reinterpret_cast<char*>(&p), sizeof(Producto));
            break;
        }
    }
    archivo.close();
}

void calcularTotales(Venta &v) {
    v.subtotalGeneral = 0;
    int items = 0;
    for (int i = 0; i < v.cantidadDetalles; i++) {
        v.subtotalGeneral += v.detalles[i].subtotal;
        items += v.detalles[i].cantidad;
    }

    double desc = 0;
    if (v.subtotalGeneral > 500) desc += 0.05;
    if (items % 5 == 0) desc += 0.03;

    v.descuento = v.subtotalGeneral * desc;
    v.iva = (v.subtotalGeneral - v.descuento) * IVA_GUATEMALA;
    v.total = (v.subtotalGeneral - v.descuento) + v.iva;
}

void crearVenta() {
    Venta v;
    v.cantidadDetalles = 0;
    v.idVenta = Util::leerEntero("ID Factura: ");
    v.dia = Util::leerEntero("Dia: ");
    v.mes = Util::leerEntero("Mes: ");
    v.anio = Util::leerEntero("Anio: ");

    char opcion;
    do {
        int cod = Util::leerEntero("Codigo producto: ");
        Producto p;
        if (obtenerProducto(cod, p)) {
            int cant = Util::leerEntero("Cantidad: ");
            if (cant > p.stock) {
                std::cout << "Stock insuficiente (Disponible: " << p.stock << ")\n";
            } else {
                v.detalles[v.cantidadDetalles] = {p.codigo, cant, p.precio, p.precio * cant};
                v.cantidadDetalles++;
                std::cout << "Agregado: " << p.nombre << "\n";
            }
        } else {
            std::cout << "Producto no existe.\n";
        }
        std::cout << "Agregar otro? (s/n): "; 
        std::cin >> opcion;
        std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n'); 
    } while ((opcion == 's' || opcion == 'S') && v.cantidadDetalles < MAX_DETALLES);

    if (v.cantidadDetalles > 0) {
        calcularTotales(v);
        std::ofstream f("ventas.dat", std::ios::binary | std::ios::app);
        f.write(reinterpret_cast<char*>(&v), sizeof(Venta));
        f.close();

        for (int i = 0; i < v.cantidadDetalles; i++)
            descontarStock(v.detalles[i].codigoProducto, v.detalles[i].cantidad);

        std::cout << "VENTA PROCESADA. TOTAL: Q" << v.total << "\n";
    }
}