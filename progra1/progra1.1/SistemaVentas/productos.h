#ifndef PRODUCTOS_H
#define PRODUCTOS_H
#include "modelos.h"

void registrarProducto();
void listarProductos();
void buscarProductoPorCodigo();
void buscarProductoPorNombre();
void actualizarStock();
void modificarPrecio();
void eliminarProducto();
bool obtenerProducto(int codigo, Producto &p); 

#endif