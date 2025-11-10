# Documentación de los Endpoints Principales — API “PayLink”

---
## 🔹 Recurso principal: **Business (Negocio)**

**Propiedades (nombre – tipo):**

- `nombre` – string → Nombre comercial del negocio registrado en PayLink.  
- `cuit` – string → Número fiscal único del negocio.  
- `apiUrl` – string → URL base de la API del negocio, usada por PayLink para consultar facturas externas.  
- `apiKey` – string → Clave generada automáticamente al registrarse, usada para autenticación en PayLink.  

---

## **Rutas del recurso principal — `/api/business`**

### 1) Obtener todos los negocios
`GET /api/business`  
Devuelve el listado completo de negocios registrados en PayLink.  
**Header requerido:** `X-API-KEY`

**Ejemplo de respuesta:**
```json
[
  {
    "id": 1,
    "nombre": "Tienda Luna",
    "cuit": "20-12345678-9",
    "apiUrl": "https://tiendaluna.com/api",
    "apiKey": "a1b2c3d4e5"
  }
]
```

### 2) Obtener negocio por ID
`GET /api/business/{id}`  
Obtiene la información de un negocio específico.  
**Ejemplo:**
```json
{
  "id": 1,
  "nombre": "Tienda Luna",
  "cuit": "20-12345678-9",
  "apiUrl": "https://tiendaluna.com/api",
  "apiKey": "a1b2c3d4e5"
}
```

### 3) Crear nuevo negocio
`POST /api/business`  
Crea un nuevo negocio y genera su `apiKey`.  
**Body:**
```json
{
  "nombre": "Tienda Luna",
  "cuit": "20-12345678-9",
  "apiUrl": "https://tiendaluna.com/api"
}
```
**Respuesta:**
```json
{
  "id": 1,
  "nombre": "Tienda Luna",
  "apiKey": "a1b2c3d4e5"
}
```

### 4) Actualizar negocio
`PUT /api/business/{id}`  
Actualiza nombre, CUIT o URL.  
**Body:**
```json
{
  "nombre": "Tienda Luna Actualizada",
  "cuit": "20-12345678-9",
  "apiUrl": "https://api.tiendaluna.com"
}
```

### 5) Eliminar negocio
`DELETE /api/business/{id}`  
Elimina un negocio **solo si no tiene pagos asociados**.

---

## 🔹 Operación de negocio (acción de dominio)
**Ruta:** `GET /api/bills/{billId}?businessId=#`  
**Descripción:**  
PayLink utiliza esta operación para consultar la API del negocio registrado y obtener los datos de una factura específica.  
Primero, PayLink busca el negocio en su base de datos con el parámetro `businessId`, obtiene su `apiUrl` y luego realiza internamente un request GET hacia la API externa del negocio (`{apiUrl}/bills/{billId}`).  
Ejemplo: `GET https://tiendaluna.com/api/bills/45`

**Respuesta ejemplo:**
```json
{
  "id": 45,
  "codigoFactura": "FAC-2024-00045",
  "cliente": "Juan Pérez",
  "montoTotal": 15000.0,
  "estado": "Confirmado"
}
```

---

## 🔹 Recurso relacionado: **Payment (Pago)**

**Propiedades:**
- `transactionId` – string → identificador único de la transacción.  
- `facturaId` – string  → código o número de la factura asociada.  
- `monto` – decimal  → monto del pago.  
- `fecha` – date  → fecha y hora en que se realizó el pago.  
- `estado` – string  → estado del pago (“Confirmado”, “Pendiente”, “Rechazado”).  
- `businessId` – number → referencia al negocio que generó el pago. 

---

### Rutas del recurso relacionado `/api/payments`

#### 1) Obtener todos los pagos
`GET /api/payments`  
Devuelve todos los pagos registrados.  
**Ejemplo de respuesta:**
```json
[
  {
    "id": 1,
    "transactionId": "TX-001",
    "facturaId": "FAC-2024-00045",
    "monto": 15000.0,
    "fecha": "2024-11-09T10:30:00",
    "estado": "Confirmado",
    "businessId": 1
  }
]
```

#### 2) Obtener un pago por ID
`GET /api/payments/{id}`  
Devuelve un pago específico.

#### 3) Crear un pago
`POST /api/payments`  
Registra un nuevo pago recibido desde una API externa.  
**Body:**
```json
{
  "transactionId": "TX-001",
  "facturaId": "FAC-2024-00045",
  "monto": 15000.0
}
```

#### 4) Buscar pago por Transaction ID
`GET /api/payments/transaction/{transactionId}`  
Busca un pago específico según su identificador único.

#### 5) Buscar pagos por Factura
`GET /api/payments/bill/{billId}`  
Devuelve los pagos asociados a una factura determinada.

---

## 🔐 Autenticación
Todos los endpoints requieren el encabezado:
```http
X-API-KEY: <clave_asignada_al_negocio>
```

Excepto `POST /api/business`, que se usa para el registro inicial.
