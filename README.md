# Todo Manager API

Este proyecto contiene una API para gestionar tareas (To-Do) junto con una interfaz de usuario (WUI) y una base de datos PostgreSQL, todo orquestado con Docker.

## Requisitos

- Docker
- Docker Compose

## ¿Cómo ejecutar la aplicación?

1.  Abre una terminal en la raíz del proyecto.
2.  Ejecuta el siguiente comando para construir las imágenes y levantar los contenedores en segundo plano:

    ```bash
    docker-compose up -d
    ```

Una vez que los contenedores estén en ejecución:
- La interfaz de usuario (WUI) estará disponible en `http://localhost:8080`.
- La API será accesible a través de la WUI en la ruta `/api`.