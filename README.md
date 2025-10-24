# 🎟️ Mobile Ticket System App

> **Scan, Pay, Enjoy.**  
> A modern mobile ticket system for attractions and events — built with **.NET MAUI** and **ASP.NET Core**.

---

## 📱 Overview

**Mobile App:** .NET MAUI (Android 14+ recommended)  
**Backend:** ASP.NET Core Web API (.NET 8)  
**Design:** [Figma Prototype](https://www.figma.com/design/Tz5BUBW3r5NIi8htdOuNGI/In%C5%BCynierka-Mobilna-Apka?node-id=0-1&t=opy3C3r6ZKKe4kr2-1)

---

## ⚙️ Tech Stack

| Layer | Stack |
|-------|--------|
| **Frontend** | .NET MAUI, XAML + Shell, MVVM, Dependency Injection |
| **Backend** | ASP.NET Core (.NET 8), MediatR (CQRS), Feature Pattern |
| **Patterns** | CQRS, Feature, Result Pattern |
| **Auth** | JWT Bearer |
| **Validation** | FluentValidation |
| **UI Toolkit** | CommunityToolkit.Maui, BarcodeScanning, LocalNotification |

---

## 🧩 Project Structure

### 🖥️ Frontend
```

MobileApp/
├── App.xaml
├── App.xaml.cs
├── MauiProgram.cs
├── GlobalUsings.cs
│
├── Models/
│ ├── Common/
│ ├── Enums/
│ ├── Requests/
│ └── Responses/
│
├── View/
│ ├── Authentication/
│ ├── Common/
│ │ └── AppShell.xaml
│ ├── CustomControls/
│ ├── Extensions/
│ ├── FlyoutMenu/
│ ├── FunFair/
│ ├── QR/
│ ├── ResetPassword/
│ └── SkiResort/
│
├── ViewModels/
│ ├── Authentication/
│ ├── Base/
│ ├── Common/
│ ├── Extensions/
│ ├── FlyoutMenu/
│ ├── FunFair/
│ ├── QR/
│ ├── Settings/
│ ├── SkiResort/
│ └── ResetPassword/
│
├── Services/
│ ├── Api/
│ ├── Local/
│ └── Extensions/
│
├── Converters/
├── Helpers/
├── Messages/
│
├── Platforms/
│ ├── Android/
│ └── Windows/
│
└── Resources/
└── Styles/
```

### 🖥️ Backend
```
Backend/
├── Program.cs
│
├── Extensions/
├── Middleware/
├── Models/
│
├── Common/
│   ├── Enums/
│   ├── Filters/
│   ├── Helpers/
│   ├── Results/
│   ├── Services/
│   └── Settings/
│
└── Features/
    ├── Auth/
    ├── Cities/
    ├── FunFairs/
    ├── Register/
    ├── Scan/
    ├── Settings/
    ├── SkiResorts/
    ├── Support/
    ├── Tickets/
    └── UserBalance/
```

---

## 🧠 Architecture Overview

### Frontend
- MVVM architecture  
- `IHttpClientFactory` for API calls  
- Centralized DI container via `MauiProgram.cs`  
- **CommunityToolkit.Maui** for popups & notifications  
- Modular View/ViewModel separation per feature  
- Messaging system for cross-component communication  

### Backend
- **CQRS** + **MediatR** for request handling  
- **Feature Pattern** → each feature is self-contained (Commands, Queries, Controllers)  
- **Result Pattern** → unified API responses  
- **FluentValidation** for strong input validation  
- **JWT Authentication**
- Swagger

---
