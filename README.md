# Transaction Tracker Console

Aplicacion de consola en .NET para registrar ingresos y gastos diarios.

El objetivo del proyecto es construir una herramienta util mientras se aprenden fundamentos reales de C# y .NET paso a paso.

## Features

- Registrar ingresos.
- Registrar gastos.
- Asignar categorias fijas.
- Usar la fecha del dia automaticamente.
- Listar transacciones.
- Filtrar por categoria.
- Filtrar por mes.
- Ver balance general.
- Ver balance mensual.
- Editar transacciones.
- Eliminar transacciones viendo los IDs en pantalla.
- Guardar y cargar datos desde `transactions.json`.

## Como ejecutar

Desde la carpeta del proyecto:

```powershell
dotnet run
```

Para compilar sin ejecutar:

```powershell
dotnet build
```

## Estructura

```txt
TransactionTracker.Console
|-- Models
|   |-- Transaction.cs
|   |-- TransactionCategory.cs
|   `-- TransactionType.cs
|-- Services
|   `-- TransactionService.cs
|-- Storage
|   `-- TransactionStorage.cs
|-- Program.cs
|-- transactions.json
`-- TransactionTracker.Console.csproj
```

## Responsabilidades

- `Program.cs`: menu, input del usuario y flujo de la consola.
- `Models`: tipos principales del dominio.
- `TransactionService`: logica de negocio, filtros, busquedas y calculos.
- `TransactionStorage`: persistencia en JSON.

## Conceptos practicados

- `record`
- `enum`
- `List<T>`
- LINQ
- `DateTime`
- `decimal`
- `switch`
- nullability
- `TryParse`
- parametros `out`
- tuplas
- `with`
- serializacion JSON
- lectura y escritura de archivos
- separacion de responsabilidades

