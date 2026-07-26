#include <iostream>
#include <vector>
#include <string>

using namespace std;

// =====================================
// CONSTANTES
// =====================================

const int MAX_NIVELES = 20;
const int BASE_MAYA = 20;


// =====================================
// FUNCIONES DE DIBUJO MAYA
// =====================================

// Dibuja un punto maya
void dibujarPunto()
{
    cout << ".";
}


// Dibuja una barra maya
void dibujarBarra()
{
    cout << "-----";
}


// Dibuja la concha maya (cero)
void dibujarConcha()
{
    cout << "(=)";
}



// =====================================
// MOSTRAR UN VALOR MAYA (0 - 19)
// =====================================

void mostrarNivel(int valor)
{
    if(valor == 0)
    {
        dibujarConcha();
        cout << endl;
        return;
    }


    int barras = valor / 5;
    int puntos = valor % 5;


    // Las barras van arriba
    for(int i = 0; i < barras; i++)
    {
        dibujarBarra();
        cout << endl;
    }


    // Los puntos van abajo
    if(puntos > 0)
    {
        for(int i = 0; i < puntos; i++)
        {
            dibujarPunto();
        }

        cout << endl;
    }
}



// =====================================
// VALIDAR UN NIVEL MAYA
// =====================================

bool validarNivel(string entrada)
{
    int puntos = 0;
    int barras = 0;
    bool concha = false;


    for(char c : entrada)
    {
        if(c == '.')
        {
            puntos++;
        }
        else if(c == '-')
        {
            barras++;
        }
        else if(c == 'P' || c == 'p')
        {
            concha = true;
        }
        else
        {
            return false;
        }
    }


    // Si es concha debe estar sola
    if(concha)
    {
        return puntos == 0 && barras == 0;
    }


    // Restricciones del sistema maya
    if(puntos > 4 || barras > 3)
    {
        return false;
    }


    return (puntos + barras * 5) <= 19;
}



// =====================================
// CONVERTIR UN NIVEL MAYA A DECIMAL
// =====================================

int convertirNivel(string entrada)
{
    int puntos = 0;
    int barras = 0;


    for(char c : entrada)
    {
        if(c == '.')
        {
            puntos++;
        }
        else if(c == '-')
        {
            barras++;
        }
    }


    return (barras * 5) + puntos;
}



// =====================================
// LEER UN NIVEL MAYA
// =====================================

int leerNivel()
{
    string entrada;


    while(true)
    {
        cin >> entrada;


        if(validarNivel(entrada))
        {
            return convertirNivel(entrada);
        }
        else
        {
            cout << "Nivel invalido. Intente nuevamente: ";
        }
    }
}



// =====================================
// LEER NUMERO MAYA COMPLETO
// GUARDA DE ABAJO HACIA ARRIBA
// =====================================

vector<int> leerNumeroMaya()
{
    vector<int> numero(MAX_NIVELES,0);


    int cantidad;


    cout << "Cantidad de niveles: ";
    cin >> cantidad;


    if(cantidad > MAX_NIVELES)
    {
        cantidad = MAX_NIVELES;
    }


    cout << endl;


    for(int i = cantidad - 1; i >= 0; i--)
    {
        cout << "Nivel " << i + 1 << ": ";

        numero[i] = leerNivel();
    }


    return numero;
}

// =====================================
// MAYA COMPLETO A DECIMAL
// =====================================

unsigned long long mayaADecimal(vector<int> numero)
{
    unsigned long long resultado = 0;

    unsigned long long potencia = 1;


    for(int i = 0; i < MAX_NIVELES; i++)
    {
        resultado += numero[i] * potencia;

        potencia *= BASE_MAYA;
    }


    return resultado;
}



// =====================================
// DECIMAL A MAYA
// =====================================

vector<int> decimalAMaya(unsigned long long numero)
{
    vector<int> resultado(MAX_NIVELES,0);


    int posicion = 0;


    if(numero == 0)
    {
        resultado[0] = 0;
        return resultado;
    }


    while(numero > 0 && posicion < MAX_NIVELES)
    {
        resultado[posicion] = numero % BASE_MAYA;

        numero = numero / BASE_MAYA;

        posicion++;
    }


    return resultado;
}



// =====================================
// MOSTRAR NUMERO MAYA COMPLETO
// DE ARRIBA HACIA ABAJO
// =====================================

void mostrarNumeroMaya(vector<int> numero)
{
    int nivelMayor = MAX_NIVELES - 1;


    while(nivelMayor > 0 && numero[nivelMayor] == 0)
    {
        nivelMayor--;
    }


    cout << endl;
    cout << "Resultado Maya:" << endl;
    cout << endl;


    for(int i = nivelMayor; i >= 0; i--)
    {
        mostrarNivel(numero[i]);
    }
}



// =====================================
// OPERACIONES
// =====================================

unsigned long long sumar(
    unsigned long long a,
    unsigned long long b)
{
    return a + b;
}



unsigned long long restar(
    unsigned long long a,
    unsigned long long b)
{
    if(a < b)
    {
        return 0;
    }

    return a - b;
}



unsigned long long multiplicar(
    unsigned long long a,
    unsigned long long b)
{
    return a * b;
}



unsigned long long dividir(
    unsigned long long a,
    unsigned long long b)
{
    if(b == 0)
    {
        return 0;
    }

    return a / b;
}



// =====================================
// MENU PRINCIPAL
// =====================================

void menu()
{
    int opcion;


    do
    {
        cout << endl;
        cout << "==============================" << endl;
        cout << "     CALCULADORA MAYA" << endl;
        cout << "==============================" << endl;

        cout << "1. Sumar" << endl;
        cout << "2. Restar" << endl;
        cout << "3. Multiplicar" << endl;
        cout << "4. Dividir" << endl;
        cout << "5. Salir" << endl;

        cout << "Seleccione una opcion: ";
        cin >> opcion;


        if(opcion >= 1 && opcion <= 4)
        {
            cout << endl;
            cout << "Ingrese primer numero maya" << endl;

            vector<int> numero1 = leerNumeroMaya();


            cout << endl;
            cout << "Ingrese segundo numero maya" << endl;

            vector<int> numero2 = leerNumeroMaya();



            unsigned long long valor1 =
                mayaADecimal(numero1);


            unsigned long long valor2 =
                mayaADecimal(numero2);



            unsigned long long resultado;



            switch(opcion)
            {
                case 1:
                    resultado = sumar(valor1,valor2);
                    break;


                case 2:
                    resultado = restar(valor1,valor2);
                    break;


                case 3:
                    resultado = multiplicar(valor1,valor2);
                    break;


                case 4:
                    resultado = dividir(valor1,valor2);
                    break;
            }



            cout << endl;
            cout << "Resultado decimal: "
                 << resultado << endl;



            vector<int> mayaResultado =
                decimalAMaya(resultado);


            mostrarNumeroMaya(mayaResultado);

        }


    }while(opcion != 5);

}



// =====================================
// MAIN
// =====================================

int main()
{
    menu();

    return 0;
}