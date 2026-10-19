# Reservacion-de-cine

Estudiante: Tayler Andres Bello Barrantes
Proyecto: opción 3, Reservación de cine
lenguaje: C#

# Descripción 

La aplicación permite administrar películas y realizar reservaciones de asientos en un cine. Permite reservar y cancelar asientos, agregar, modificar y eliminar películas, buscar reservas por nombre de cliente y ordenar las reservas alfabéticamente.

# Requisitos

- Visual Studio con soporte para C# y Windows Forms.
- Los archivos completos del proyecto.
- El entorno de .NET compatible con el proyecto.

# Instalación y ejecución

- Descargar
- Abrir el archivo de solución .slnx en Visual Studio.
- Esperar a que el proyecto cargue correctamente.
- Ejecutar la aplicación con el botón Iniciar.


# Estructura de datos utilizados

## Estructuras de datos utilizadas

### 1. Arreglo bidimensional

| Explicación | Ubicación en el código |
|---|---|
| Organiza los 20 asientos del cine en 4 filas y 5 columnas. El arreglo de botones permite manejar los asientos visualmente, mientras que la matriz de números indica si están libres o reservados. | Archivo: `Form1.cs`. Clase: `Peliculas`, variable `asientos` y método `MostrarAsientos()`. Clase: `Pelicula`, variable `Asientos`. |

### 2. ArrayList

| Explicación | Ubicación en el código |
|---|---|
| Guarda las películas y permite agregarlas, modificarlas y eliminarlas. | Archivo: `Form1.cs`. Clase: `Peliculas`. Variable: `peliculas`. Métodos: `ActualizarPeliculas()`, `btnAgregarPelicula_Click()` y `btnEliminarPelicula_Click()`. |

### 3. Lista simplemente enlazada

| Explicación | Ubicación en el código |
|---|---|
| Guarda las reservas mediante nodos conectados entre sí. Cada nodo contiene una reserva y una referencia al siguiente nodo. | Archivo: `Forms1.cs`. Clase: `NodoReserva`, variables `Datos` y `Siguiente`. Clase: `Peliculas`, variable `inicio` y métodos `btnReservar_Click()`, `MostrarReservas()` y `btnCancelar_Click()`. |

## Datos de ejemplo para la demostración

Al iniciar la aplicación se cargan las películas Rapidos y furiosos, Spider-Man y Batman.

Para demostrar el funcionamiento:

1. Seleccionar una película.
2. Seleccionar un asiento libre.
3. Escribir el nombre del cliente.
4. Presionar el botón de reservar.
5. Comprobar que el asiento cambia de color y aparece la reserva en la lista.
6. Probar la cancelación, la búsqueda y el ordenamiento de reservas.
