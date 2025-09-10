# Cafe System Api

![License](https://img.shields.io/badge/license-MIT-green.svg)
![.NET](https://img.shields.io/badge/.NET-9-blue)
![Status](https://img.shields.io/badge/status-Development-yellow)

## Описание

Простой проект реализует backend-логику для системы заказов в кафе, делающийся для всестороннего изучения тестирования.  
Основные доменные сущности включают:

- **Product** — Продукт, доступный для заказа.
- **OrderItem** — Единица заказа с продуктом и дополнительными данными.
- **Order** — Заказ.
  
Основные Value Object:

- **Money** — Валютный объект для указания стоимости продукта и проведения расчётных операций.
- **CustomerName** — Имя покупателя.

## Структура проекта

```
/Cafe
│
├── /Domain              # Доменные сущности и value objects
│   ├── /Entities
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   └── Product.cs
│   └── /ValueObjects
│       ├── CustomerName.cs
│       └── Money.cs
│
├── /Application         # Логика приложения, CQRS, обработчики команд и запросов, контроллеры
│
├── /Infrastructure      # Конфигурация EF Core, репозитории, миграции
│
├── /API                 # Веб-слой
│
├── /Tests                 # Веб-слой
│
└── README.md            # Этот файл
```
