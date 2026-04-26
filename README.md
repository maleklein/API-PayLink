# API PayLink

API backend desarrollada en .NET para la gestión de pagos y negocios, permitiendo la integración con servicios externos de facturación mediante endpoints REST.

---

##  Descripción

Este proyecto consiste en una API que permite administrar negocios y procesar pagos, facilitando la comunicación con sistemas externos para la consulta y validación de facturas.

Está orientado a simular un sistema real de integración entre aplicaciones, donde distintos clientes pueden interactuar mediante una API segura y estructurada.

---

##  Funcionalidades principales

- Gestión de negocios (Business)
- Procesamiento de pagos
- Consumo de APIs externas para obtener información de facturación
- Estructura basada en controladores (Controllers) y servicios
- Manejo de endpoints REST (GET, POST, etc.)
- Separación de responsabilidades (arquitectura por capas)

---

##  Tecnologías utilizadas

- C#
- .NET / ASP.NET Core
- API REST
- JSON
- Inyección de dependencias

---

##  Arquitectura

El proyecto sigue una estructura basada en capas, separando claramente:

- **Controllers** → manejo de las solicitudes HTTP
- **Services** → lógica de negocio
- **ExternalApiService** → integración con APIs externas

Esto permite un código más mantenible, escalable y organizado.

---

##  Integraciones

La API se conecta con servicios externos para consultar información de facturación mediante URLs configurables por negocio.

---
