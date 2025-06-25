# SmartHome API

The SmartHome API allows users to manage smart home automation devices and access sensor data associated with them. The API facilitates operations such as adding, updating, 
deleting, assigning devices, and monitoring sensor data. The DevicesController handles all operations related to managing and interacting with these devices. 
It enables users to add, remove, and update devices, assign them to specific users, and track sensor readings.

---

## 📌 Table of Contents

- [Overview](#overview)
- [Authentication](#authentication)
- [Controllers](#controllers)
  - [usersController](#scontroller)
  - [DevicesController](#devicescontroller)
  - [CategoriesController](#categoriescontroller)
  - [LocationsController](#locationscontroller)
  - [AnalyticsController](#analyticscontroller)
- [Role-Based Access](#role-based-access)
- [Response Format](#response-format)
- [Technologies & Middleware](#technologies--middleware)

---

## 📘 Overview

The SmartHome system provides:
- Device and  management
- Sensor data tracking
- Role-based access (Admin, Host, Member)
- Energy consumption and health analytics
- Location and category control

---

## 🔐 Authentication

The API uses **JWT Token Authentication**.  
- s must first authenticate via the `POST /api/s/login` endpoint.
- Upon successful login, a JWT token is returned.
- This token must be included in the `Authorization` header as:  
  `Authorization: Bearer {token}`

---

## 🧩 Controllers

### 👤 UsersController

Handles  authentication and role-based  creation.

| Endpoint                         | Method | Description                          | Roles          |
|----------------------------------|--------|--------------------------------------|----------------|
| `/api/s/login`              | POST   | Login and receive JWT token          | Public         |
| `/api/s/create-host`        | POST   | Create Host                      | Admin          |
| `/api/s/create-member`      | POST   | Create Member                    | Admin, Host    |
| `/api/s/all`                | GET    | Get all s (filtered by role)     | Admin, Host    |

---

### 📱 DevicesController

Manages smart devices and their sensor readings.

| Endpoint                                 | Method | Description                            | Roles          |
|------------------------------------------|--------|----------------------------------------|----------------|
| `/api/devices/all`                      | GET    | Get all devices                        | Admin, Host,  |
| `/api/devices/create`                   | POST   | Create a new device                    | Admin          |
| `/api/devices/assignHost`              | POST   | Assign device to a Host                | Admin          |
| `/api/devices/assignMember`            | POST   | Assign device to a Member              | Host           |
| `/api/devices/details/{deviceId}`      | GET    | Get device details                     | Admin, Host, Member |
| `/api/devices/delete/{deviceId}`       | DELETE | Delete a device                        | Admin, Host    |
| `/api/devices/update/{deviceId}`       | PUT    | Update device                          | Admin, Host    |
| `/api/devices/{deviceId}/addreading`   | POST   | Add a sensor reading                   | Admin, Host, Member |
| `/api/devices/{id}/getreadings`        | GET    | Get paged sensor readings              | Admin, Host, Member |
| `/api/devices/{id}/latestreading`      | GET    | Get latest sensor reading              | Admin, Host, Member |
| `/api/devices/{deviceId}/setStatus`    | PUT    | Update device status                   | Admin, Host    |

---

### 🗂️ CategoriesController

Allows Admins to manage device categories.

| Endpoint                   | Method | Description                | Roles  |
|----------------------------|--------|----------------------------|--------|
| `/api/categories/all`     | GET    | Get all categories         | Admin  |
| `/api/categories/create`  | POST   | Create new category        | Admin  |
| `/api/categories/delete/{id}` | DELETE | Delete category         | Admin  |

---

### 📍 LocationsController

Used for managing physical locations and associated devices.

| Endpoint                             | Method | Description                          | Roles          |
|--------------------------------------|--------|--------------------------------------|----------------|
| `/api/locations/all`                | GET    | Get all locations                    | Admin          |
| `/api/locations/create`             | POST   | Create new location                  | Admin          |
| `/api/locations/delete/{id}`        | DELETE | Delete a location                    | Admin          |
| `/api/locations/{locationId}/devices` | GET  | Get devices for a specific location  | Admin, Host, Member |

---

### 📊 AnalyticsController

Provides insights and reports on devices.

| Endpoint                                      | Method | Description                               | Roles          |
|-----------------------------------------------|--------|-------------------------------------------|----------------|
| `/api/analytics/devices/{deviceId}/energy-usage` | GET  | Get total energy consumption for device   | Admin, Host    |
| `/api/analytics/device-status`               | GET    | Get statuses of all devices               | Admin, Host    |
| `/api/analytics/location/usage`              | GET    | Get usage stats by location               | Admin, Host    |
| `/api/analytics/devices/health`              | GET    | Get health report for all devices         | Admin, Host    |

---

## 🛡️ Role-Based Access

| Role    | Permissions                                                                                                                                  |
|---------|----------------------------------------------------------------------------------------------------------------------------------------------|
| Admin   | Full control (create/update/delete s, devices, categories, locations, access analytics)                                                  |
| Host    | Can manage assigned devices, assign members, create members, view analytics                                                                 |
| Member  | Has access to assigned device data                                                                                                           |
|     | Can view public data like sensor readings                                                                                                    |

---

## 📦 Response Format

All API responses follow a consistent format:
	
```json
{
  "statusCode": 200,
  "message": "Message describing result",
  "data": { ... }
}
```

---

## 🧪 Technologies & Middleware

- **.NET Core** Web API with clean layered architecture  
- **AutoMapper** used for mapping DTOs to entities and vice versa  
- **Custom Exception Handling Middleware** handles API errors gracefully  
- **JWT Authentication** with role-based access control  
- **Entity Framework Core** for data access  
- **Swagger** support for endpoint testing (if enabled)  

---

## 🚀 Conclusion

The **SmartHome API** provides a secure, scalable and role-based interface for managing smart devices and s in a smart home environment. It supports powerful analytics and flexible device- mapping.

For further development or integration, refer to controller-specific documentation or contact the API maintainer.

---