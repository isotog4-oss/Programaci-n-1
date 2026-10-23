-- Se puede ejecutar muchas veces: cada tabla se crea solo si no existe.
-- Las fechas se guardan como texto "yyyy-MM-dd" y Atendida como 0/1
-- (asi funciona SQLite).

PRAGMA foreign_keys = ON;

-- Antes: Datos/Carnet/carnet_X.txt (bloque CARNET)
CREATE TABLE IF NOT EXISTS Carnets (
    NumeroCarnet TEXT PRIMARY KEY,
    NombreAnimal TEXT NOT NULL,
    Especie      TEXT NOT NULL,
    DpiDueno     TEXT NOT NULL,
    Celular      TEXT NOT NULL,
    Direccion    TEXT NOT NULL
);

-- Antes: bloques CITA dentro del .txt del carnet (1 carnet : muchas citas)
CREATE TABLE IF NOT EXISTS Citas (
    IdCita       INTEGER PRIMARY KEY AUTOINCREMENT,
    NumeroCarnet TEXT NOT NULL,
    Fecha        TEXT NOT NULL,
    Hora         TEXT NOT NULL,
    Atendida     INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (NumeroCarnet) REFERENCES Carnets(NumeroCarnet) ON DELETE CASCADE
);

-- Antes: bloque RECETA (1 cita : maximo 1 receta, por eso IdCita es la llave primaria)
CREATE TABLE IF NOT EXISTS Recetas (
    IdCita       INTEGER PRIMARY KEY,
    Medicamento  TEXT NOT NULL,
    Dosis        TEXT NOT NULL,
    Indicaciones TEXT NOT NULL,
    FOREIGN KEY (IdCita) REFERENCES Citas(IdCita) ON DELETE CASCADE
);

-- Antes: bloques PROCEDIMIENTO (1 cita : muchos procedimientos)
CREATE TABLE IF NOT EXISTS Procedimientos (
    IdProcedimiento INTEGER PRIMARY KEY AUTOINCREMENT,
    IdCita          INTEGER NOT NULL,
    Descripcion     TEXT NOT NULL,
    Fecha           TEXT NOT NULL,
    FOREIGN KEY (IdCita) REFERENCES Citas(IdCita) ON DELETE CASCADE
);

-- Antes: Datos/Usuarios/usuarios.txt (recepcionistas y doctores)
CREATE TABLE IF NOT EXISTS Usuarios (
    IdUsuario  INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre     TEXT NOT NULL,
    Contrasena TEXT NOT NULL,
    Tipo       TEXT NOT NULL,
    UNIQUE (Nombre, Tipo)
);

-- Antes: Datos/Usuarios/desarrolladores.txt
CREATE TABLE IF NOT EXISTS Desarrolladores (
    Nombre     TEXT PRIMARY KEY,
    Contrasena TEXT NOT NULL
);

-- Antes: Datos/Logs/<usuario>.txt (un archivo por persona)
-- Ahora: una sola tabla, y cada fila dice de quien es.
CREATE TABLE IF NOT EXISTS Logs (
    IdLog      INTEGER PRIMARY KEY AUTOINCREMENT,
    Usuario    TEXT NOT NULL,
    Linea      TEXT NOT NULL,
    Registrado TEXT NOT NULL DEFAULT (datetime('now', 'localtime'))
);

CREATE INDEX IF NOT EXISTS IX_Citas_NumeroCarnet ON Citas(NumeroCarnet);
CREATE INDEX IF NOT EXISTS IX_Procedimientos_IdCita ON Procedimientos(IdCita);
