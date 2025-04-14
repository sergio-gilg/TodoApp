# TodoApp

Este proyecto es una aplicación de gestión de tareas que incluye una API, una aplicación de consola y un cliente web. A continuación, se describen los pasos para configurar y ejecutar cada componente.

---

## Requisitos previos

Antes de comenzar, asegúrate de tener instalados los siguientes programas:

- [Node.js](https://nodejs.org/) (versión 16 o superior)
- [npm](https://www.npmjs.com/) (incluido con Node.js)
- [SQLite](https://sqlite.org/) o cualquier base de datos compatible
- [Git](https://git-scm.com/)
- [SDK de .NET](https://dotnet.microsoft.com/) (versión 6.0 o superior)

---

## Configuración inicial

1. Clona el repositorio en tu máquina local:

   ```bash
   git clone https://github.com/sergio-gilg/TodoApp.git
   cd TodoApp
   ```

2. Instala las dependencias necesarias para la API:

   ```bash
   cd src/TodoApp.Api
   dotnet restore
   ```

---

## Migración de la base de datos

1. Asegúrate de que SQLite esté instalado y configurado en tu máquina.
2. Ejecuta las migraciones para crear las tablas necesarias:

   ```bash
   cd src/TodoApp.Api
   dotnet ef database update
   ```

---

## Ejecución de la API

1. Inicia el servidor de la API:

   ```bash
   cd src/TodoApp.Api
   dotnet run
   ```

2. La API estará disponible en `http://localhost:5000` o `https://localhost:5001`.

---

## Ejecución de la aplicación de consola

1. Ve al directorio de la aplicación de consola:

   ```bash
   cd src/TodoApp.Console
   ```

2. Ejecuta la aplicación de consola:

   ```bash
   dotnet run
   ```

3. Sigue las instrucciones en la consola para interactuar con la aplicación.

---

## Ejecución del cliente web

1. Ve al directorio del cliente:

   ```bash
   cd client
   ```

2. Inicia el cliente web usando **live-server** o cualquier servidor HTTP. Por ejemplo, con **live-server**:

   ```bash
   live-server
   ```

3. Abre el navegador y accede a `http://127.0.0.1:8080`.

---

## Estructura del proyecto

```plaintext
TodoApp/
├── client/                          # Cliente web (HTML, CSS, JS)
├── src/
│   ├── TodoApp.Api/                 # API y lógica del servidor
│   ├── TodoApp.Console/             # Aplicación de consola
│   ├── TodoApp.Domain/              # Lógica de dominio
│   ├── TodoApp.Infrastructure/      # Implementaciones de infraestructura (DB, servicios externos)
│   ├── TodoApp.Application/         # Casos de uso y lógica de aplicación
├── tests/                           # Pruebas unitarias y de integración
└── README.md                        # Documentación del proyecto
```

---

¡Gracias por usar TodoApp! Si tienes preguntas o problemas, no dudes en abrir un issue en el repositorio.
