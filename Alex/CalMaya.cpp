#include <iostream>
#include <vector>
#include <string>
#include <iomanip>
#include <sstream>

using namespace std;

const int LIMITEDENIVELES = 20;
const int BASE = 20;

void dibujarPunto()
{
    cout << ".";
}

void dibujarBarra()
{
    cout << "-----";
}

void dibujarConcha()
{
    cout << "(=)";
}

void mostrarNivel(int valor)
{
    if (valor == 0)
    {
        dibujarConcha();
        cout << endl;
        return;
    }

    int barras = valor / 5;
    int puntos = valor % 5;

    for (int i = 0; i < barras; i++)
    {
        dibujarBarra();
        cout << endl;
    }

    if (puntos > 0)
    {
        for (int i = 0; i < puntos; i++)
        {
            dibujarPunto();
        }

        cout << endl;
    }
}

bool validarNivel(string entrada)
{
    int puntos = 0;
    int barras = 0;
    bool concha = false;

    for (char c : entrada)
    {
        if (c == '.')
        {
            puntos++;
        }
        else if (c == '-')
        {
            barras++;
        }
        else if (c == 'P' || c == 'p')
        {
            concha = true;
        }
        else
        {
            return false;
        }
    }

    if (concha)
    {
        return puntos == 0 && barras == 0;
    }

    if (puntos > 4 || barras > 3)
    {
        return false;
    }

    return (puntos + barras * 5) <= 19;
}

int convertirNivel(string entrada)
{
    int puntos = 0;
    int barras = 0;

    for (char c : entrada)
    {
        if (c == '.')
            puntos++;

        if (c == '-')
            barras++;
    }

    return puntos + barras * 5;
}

int leerNivel()
{
    string entrada;

    while (true)
    {
        cin >> entrada;

        if (validarNivel(entrada))
        {
            return convertirNivel(entrada);
        }

        cout << "Nivel invalido, intente otra vez: ";
    }
}

vector<int> leerNumeroMaya()
{
    vector<int> numero(LIMITEDENIVELES, 0);

    int cantidad;

    cout << "Cantidad de niveles: ";
    cin >> cantidad;

    if (cantidad > LIMITEDENIVELES)
        cantidad = LIMITEDENIVELES;

    for (int i = cantidad - 1; i >= 0; i--)
    {
        cout << "Nivel " << i + 1 << ": ";

        numero[i] = leerNivel();
    }

    return numero;
}

string formatoNumero(double numero)
{
    stringstream ss;

    ss << fixed << setprecision(2) << numero;

    string texto = ss.str();

    int punto = texto.find('.');

    string entero = texto.substr(0, punto);
    string decimal = texto.substr(punto);

    int contador = 0;

    for (int i = entero.length() - 1; i > 0; i--)
    {
        contador++;

        if (contador == 3)
        {
            entero.insert(i, ",");
            contador = 0;
        }
    }

    return entero + decimal;
}

unsigned long long mayaADecimal(vector<int> numero)
{
    unsigned long long resultado = 0;

    unsigned long long potencia = 1;

    for (int i = 0; i < LIMITEDENIVELES; i++)
    {
        resultado += numero[i] * potencia;

        potencia *= BASE;
    }

    return resultado;
}

vector<int> decimalAMaya(unsigned long long numero)
{
    vector<int> resultado(LIMITEDENIVELES, 0);

    int posicion = 0;

    if (numero == 0)
    {
        resultado[0] = 0;
        return resultado;
    }

    while (numero > 0 && posicion < LIMITEDENIVELES)
    {
        resultado[posicion] = numero % BASE;

        numero /= BASE;

        posicion++;
    }

    return resultado;
}

void mostrarNumeroMaya(vector<int> numero)
{
    int nivelMayor = LIMITEDENIVELES - 1;

    while (nivelMayor > 0 && numero[nivelMayor] == 0)
    {
        nivelMayor--;
    }

    cout << endl;
    cout << "Representacion Maya:" << endl;
    cout << endl;

    for (int i = nivelMayor; i >= 0; i--)
    {
        mostrarNivel(numero[i]);
    }
}

vector<int> ingresarNumeroConfirmado()
{
    vector<int> numero;

    char respuesta;

    do
    {
        numero = leerNumeroMaya();

        unsigned long long valor =
            mayaADecimal(numero);

        cout << endl;
        cout << "Niveles utilizados: ";

        int cantidad = 0;

        for (int i = LIMITEDENIVELES - 1; i >= 0; i--)
        {
            if (numero[i] != 0 || i == 0)
            {
                cantidad = i + 1;
                break;
            }
        }

        cout << cantidad << endl;

        cout << "Valor decimal: "
             << formatoNumero(valor)
             << endl;

        cout << "¿Desea cambiar este numero? (S/N): ";
        cin >> respuesta;

    } while (respuesta == 'S' || respuesta == 's');

    return numero;
}

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
    if (a < b)
        return 0;

    return a - b;
}

unsigned long long multiplicar(
    unsigned long long a,
    unsigned long long b)
{
    return a * b;
}

double dividir(
    double a,
    double b)
{
    if (b == 0)
    {
        return 0;
    }

    return a / b;
}

void menu()
{
    int opcion;

    do
    {
        cout << endl;
        cout << "     CALCULADORA MAYA     " << endl;
        cout << endl;  
        cout << "1. Sumar" << endl;
        cout << "2. Restar" << endl;
        cout << "3. Multiplicar" << endl;
        cout << "4. Dividir" << endl;
        cout << "5. Salir" << endl;
        cout << endl;
        cout << "Seleccione: ";
        cin >> opcion;

        if (opcion >= 1 && opcion <= 4)
        {

            cout << endl;
            cout << "PRIMER NUMERO MAYA" << endl;

            vector<int> numero1 =
                ingresarNumeroConfirmado();

            cout << endl;
            cout << "SEGUNDO NUMERO MAYA" << endl;

            vector<int> numero2 =
                ingresarNumeroConfirmado();

            unsigned long long valor1 =
                mayaADecimal(numero1);

            unsigned long long valor2 =
                mayaADecimal(numero2);

            if (opcion == 4)
            {
                double resultado =
                    dividir(valor1, valor2);

                cout << endl;
                cout << "Resultado decimal: "
                     << formatoNumero(resultado)
                     << endl;

                vector<int> mayaResultado =
                    decimalAMaya((unsigned long long)resultado);

                mostrarNumeroMaya(mayaResultado);
            }
            else
            {
                unsigned long long resultado;

                switch (opcion)
                {
                case 1:
                    resultado = sumar(valor1, valor2);
                    break;

                case 2:
                    resultado = restar(valor1, valor2);
                    break;

                case 3:
                    resultado = multiplicar(valor1, valor2);
                    break;
                }

                cout << endl;
                cout << "Resultado decimal: "
                     << formatoNumero(resultado)
                     << endl;

                vector<int> mayaResultado =
                    decimalAMaya(resultado);

                mostrarNumeroMaya(mayaResultado);
            }
        }

    } while (opcion != 5);
}

int main()
{
    menu();

    return 0;
}