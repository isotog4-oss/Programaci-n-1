#include "sistema.h"
#include "utilidades.h"
#include "productos.h"
#include "ventas.h"
#include "reportes.h"
#include <iostream>

void menuPrincipal() {
    int op;
    do {
        Util::limpiarPantalla();
        Util::mostrarEncabezado();
        std::cout << "1. Productos\n2. Ventas\n3. Reportes\n4. Salir\n";
        op = Util::leerEntero("Opcion: ");
        switch(op) {
            case 1: menuProductos(); break;
            case 2: crearVenta(); Util::pausar(); break;
            case 3: menuReportes(); break;
        }
    } while (op != 4);
}

void menuProductos() {
    int op;
    do {
        Util::limpiarPantalla();
        std::cout << "1. Registrar\n2. Listar\n3. Stock\n4. Eliminar\n5. Volver\n";
        op = Util::leerEntero("Opcion: ");
        if (op == 1) registrarProducto();
        if (op == 2) listarProductos();
        if (op == 3) actualizarStock();
        if (op == 4) eliminarProducto();
        if (op != 5) Util::pausar();
    } while (op != 5);
}

void menuReportes() {
    int op;
    do {
        Util::limpiarPantalla();
        std::cout << "1. Ventas por Mes\n2. Ordenar por Precio\n3. Exportar TXT\n4. Volver\n";
        op = Util::leerEntero("Opcion: ");
        if (op == 1) reportarVentasPorMes();
        if (op == 2) ordenarCatálogo();
        if (op == 3) exportarTexto();
        if (op != 4) Util::pausar();
    } while (op != 4);
}