#ifndef MODELOS_H
#define MODELOS_H

const int MAX_NOM = 50;
const int MAX_DETALLES = 10;
const double IVA_GUATEMALA = 0.12;

struct Producto {
    int codigo;
    char nombre[MAX_NOM];
    double precio;
    int stock;
    int ventasAcumuladas;
    bool activo;
};

struct DetalleVenta {
    int codigoProducto;
    int cantidad;
    double precioUnitario;
    double subtotal;
};

struct Venta {
    int idVenta;
    int dia, mes, anio;
    DetalleVenta detalles[MAX_DETALLES];
    int cantidadDetalles;
    double subtotalGeneral;
    double iva;
    double descuento;
    double total;
};

#endif