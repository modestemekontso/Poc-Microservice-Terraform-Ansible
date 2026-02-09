                        ┌───────────────┐
                        │    Client /    │
                        │   Front-End    │
                        └───────┬───────┘
                                │ HTTP
                                ▼
        ┌─────────────────────────────────────────────┐
        │                Microservices                 │
        │                                             │
        │   ┌────────────────┐    ┌────────────────┐ │
        │   │ CustomerService│    │  OrderService  │ │
        │   └───────┬────────┘    └───────┬────────┘ │
        │           │                     │          │
        │           ▼                     ▼          │
        │   PostgreSQL (customerdb)  PostgreSQL(orderdb)
        │                                             │
        │   ┌────────────────┐                       │
        │   │ CatalogService │                       │
        │   └───────┬────────┘                       │
        │           ▼                                │
        │   PostgreSQL (catalogdb)                   │
        └─────────────────────────────────────────────┘

                    Communication Asynchrone (Events)
        ┌─────────────────────────────────────────────┐
        │                    RabbitMQ                  │
        └─────────────────────────────────────────────┘

CustomerService  ── publish customer.created ─────────▶ RabbitMQ
OrderService     ◀─ consume customer.created ───────── RabbitMQ

OrderService     ── publish order.created ───────────▶ RabbitMQ
CatalogService   ◀─ consume order.created ─────────── RabbitMQ
CustomerService ◀─ consume order.created ─────────── RabbitMQ

OrderService ── HTTP GET ─────────▶ CustomerService
OrderService ── HTTP GET ─────────▶ CatalogService
