#ifndef VENTAS_H
#define VENTAS_H
#include "modelos.h"

void crearVenta();
void calcularTotales(Venta &v);
void descontarStock(int codigo, int cant);

#endif