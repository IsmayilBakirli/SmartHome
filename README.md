# 🚀 SmartHome API Documentation

The **SmartHome API** is a secure, modular and role-driven backend designed for managing smart home automation devices, their sensor data, and user-role interactions.
It supports multi-tenant management through structured roles (Admin, Host, Member) and provides analytics for monitoring usage and health.

---

## 📑 Table of Contents

* [Overview](#overview)
* [Architecture & Design](#architecture--design)
* [Authentication](#authentication)
* [Controllers](#controllers)

  * [UsersController](#userscontroller)
  * [DevicesController](#devicescontroller)
  * [CategoriesController](#categoriescontroller)
  * [LocationsController](#locationscontroller)
  * [AnalyticsController](#analyticscontroller)
* [Role-Based Access](#role-based-access)
* [Response Format](#response-format)
* [Technologies & Middleware](#technologies--middleware)
* [Conclusion](#conclusion)

---

## 📘 Overview

The **SmartHome API** enables:

* Smart device management (add, update, assign, monitor)
* Sensor data collection and visualization
* Hierarchical role-based access control
* Physical location and category-based device grouping
* Energy and health analytics per device/location

---

## 🧱 Architecture & Design

### ✅ Clean Layered Architecture

* **Controller Layer** – Handles HTTP requests/responses
* **Service Layer** – Contains business logic and role validations
* **Repository Layer** – Handles data access (via EF Core)
* **Entities & DTOs** – Clear separation of concerns with AutoMapper

---

### 🔐 Role-Based Ownership Hierarchy

| Role   | Manages          | Description                                                     |
| ------ | ---------------- | --------------------------------------------------------------- |
| Admin  | Hosts, Devices   | Can create Hosts and assign devices                             |
| Host   | Members, Devices | Can create Members, manage and assign devices to them           |
| Member | Their devices    | Can only view (or update if ReadWrite) devices assigned to them |

---

### 🧩 Device Model Breakdown

The device-related data is structured into three abstraction layers for scalability:

1. **DeviceCategory**: Shared template-level configuration (e.g. Smart Bulb)
2. **Device**: Individual physical device, tied to user/location
3. **Location**: Physical grouping of devices (e.g. “Kitchen”)

```plaintext
DeviceCategory → Device → Location (1:N relationships)
```

This structure ensures:
✅ Reusability
✅ No data duplication
✅ Logical ownership per role
✅ Real-world device representation

---

## 🔐 Authentication

* Based on **JWT Token Authentication**
* All requests (except login) must include:

```http
Authorization: Bearer {token}
```

* Users authenticate via `POST /api/users/login`

---

## 🧩 Controllers

### 👤 UsersController

Handles authentication and role-based user creation.

| Endpoint                   | Method | Description                 | Roles       |
| -------------------------- | ------ | --------------------------- | ----------- |
| `/api/users/login`         | POST   | Login and receive JWT token | Public      |
| `/api/users/create-host`   | POST   | Create a Host               | Admin       |
| `/api/users/create-member` | POST   | Create a Member             | Admin, Host |
| `/api/users/all`           | GET    | List users filtered by role | Admin, Host |

**Design Notes**:

* Each user is linked to the creator (OwnerId)
* Role hierarchy enforces who can create or see whom

---

### 📱 DevicesController

Manages IoT devices and sensor data.

| Endpoint                             | Method | Description                     | Roles               |
| ------------------------------------ | ------ | ------------------------------- | ------------------- |
| `/api/devices/all`                   | GET    | Get all accessible devices      | Admin, Host         |
| `/api/devices/create`                | POST   | Create a new device             | Admin               |
| `/api/devices/assignHost`            | POST   | Assign device to a Host         | Admin               |
| `/api/devices/assignMember`          | POST   | Assign device to a Member       | Host                |
| `/api/devices/details/{deviceId}`    | GET    | Get device details              | Admin, Host, Member |
| `/api/devices/delete/{deviceId}`     | DELETE | Delete a device                 | Admin, Host         |
| `/api/devices/update/{deviceId}`     | PUT    | Update device information       | Admin, Host         |
| `/api/devices/{deviceId}/addreading` | POST   | Add a sensor reading            | Admin, Host, Member |
| `/api/devices/{id}/getreadings`      | GET    | Get paged sensor readings       | Admin, Host, Member |
| `/api/devices/{id}/latestreading`    | GET    | Get the latest sensor reading   | Admin, Host, Member |
| `/api/devices/{deviceId}/setStatus`  | PUT    | Set device status (on/off/etc.) | Admin, Host         |

**Design Notes**:

* All device operations enforce ownership
* Sensor readings are stored in a separate table (1\:N)
* Device assignment is traceable via audit fields (AssignedBy, AssignedTo)

---

### 🗂️ CategoriesController

Allows Admins to define shared configuration templates for devices.

| Endpoint                      | Method | Description         | Roles |
| ----------------------------- | ------ | ------------------- | ----- |
| `/api/categories/all`         | GET    | Get all categories  | Admin |
| `/api/categories/create`      | POST   | Create new category | Admin |
| `/api/categories/delete/{id}` | DELETE | Delete a category   | Admin |

**Design Notes**:

* Reduces data duplication by centralizing shared device properties
* Useful for analytics and standardization

---

### 📍 LocationsController

Manages physical locations linked to devices.

| Endpoint                              | Method | Description             | Roles               |
| ------------------------------------- | ------ | ----------------------- | ------------------- |
| `/api/locations/all`                  | GET    | List all locations      | Admin               |
| `/api/locations/create`               | POST   | Create a new location   | Admin               |
| `/api/locations/delete/{id}`          | DELETE | Remove a location       | Admin               |
| `/api/locations/{locationId}/devices` | GET    | Get devices by location | Admin, Host, Member |

**Design Notes**:

* Location acts as an organizing container
* Enables reporting and filtering per physical area

---

### 📊 AnalyticsController

Provides real-time and historical insights.

| Endpoint                                         | Method | Description                          | Roles       |
| ------------------------------------------------ | ------ | ------------------------------------ | ----------- |
| `/api/analytics/devices/{deviceId}/energy-usage` | GET    | Get total energy usage               | Admin, Host |
| `/api/analytics/device-status`                   | GET    | Device status summary                | Admin, Host |
| `/api/analytics/location/usage`                  | GET    | Usage stats per location             | Admin, Host |
| `/api/analytics/devices/health`                  | GET    | Device health (battery, signal etc.) | Admin, Host |

**Design Notes**:

* Analytics is calculated in the service layer using LINQ/grouping
* Caching these responses is recommended in production

---

## 🛡️ Role-Based Access

| Role   | Permissions                                                                     |
| ------ | ------------------------------------------------------------------------------- |
| Admin  | Full control over Hosts, Members, devices, categories, locations, and analytics |
| Host   | Can manage assigned devices, create/manage Members, view analytics              |
| Member | Can only view (or update if ReadWrite) their assigned devices and sensor data   |

All controller methods are protected with `[Authorize(Roles = "...")]` attributes and service-layer validation.

---

## 📦 Response Format

Standard API responses follow this format:

```json
{
  "statusCode": 200,
  "message": "Request succeeded",
  "data": {
    // Optional payload
  }
}
```

---

## 🧪 Technologies & Middleware

* **.NET 8 Web API**
* **Entity Framework Core**
* **AutoMapper**
* **JWT Authentication**
* **Custom Exception Handling Middleware**
* **Swagger UI** (for testing and docs)

---

## ✅ Conclusion

The **SmartHome API** is built with clarity, role hierarchy, and data abstraction in mind. It enables secure, scalable, and maintainable management of smart home devices, users, and environments — designed for both extensibility and real-world scenarios.
