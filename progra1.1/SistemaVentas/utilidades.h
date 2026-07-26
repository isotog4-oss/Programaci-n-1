#ifndef UTILIDADES_H
#define UTILIDADES_H

namespace Util {
    void mostrarEncabezado();
    int leerEntero(const char* mensaje);
    double leerDouble(const char* mensaje);
    void leerCadena(const char* mensaje, char* destino, int tamano);
    void limpiarPantalla();
    void pausar();
}

#endif