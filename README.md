# 🏛️ Prison Management System

A desktop enterprise application for managing a correctional facility, built with **WPF (.NET 5)** and the **MVVM pattern**. The client communicates with a remote REST API hosted on Azure, implements role-based access control across 6 staff roles, and provides full CRUD operations across 20 database entities with soft-delete and CSV export.

## ✨ Features

- 🔐 **Role-based login** — 6 staff roles, each with a dynamically configured set of accessible modules
- 📋 **20 data modules** — prisoners, workers, prosecutions, warehouses, dishes, sales, journals, and more
- ✏️ **Full CRUD** — create, read, update, and soft-delete records via REST API calls
- ♻️ **Soft delete & recover** — deleted records are moved to a separate list and can be restored
- 📤 **CSV export** — any table can be exported to a `.csv` file via a folder picker dialog
- 🔃 **Column sorting** — generic reflection-based sorter works across all `ObservableCollection<T>` tables
- ⚡ **Fully async** — all API calls use `async/await` with `HttpClient`, keeping the UI responsive

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Language | C# |
| UI Framework | WPF (.NET 5) |
| Architecture | MVVM (custom `ObservableObject`, `RelayCommand`) |
| ORM | Entity Framework Core 5 (SQL Server) |
| Networking | `HttpClient` — GET / POST / PUT / DELETE |
| Serialization | Newtonsoft.Json |
| CSV Export | CsvHelper |
| API Backend | REST API hosted on Azure (`prisonapi2.azurewebsites.net`) |

## 🏗️ Architecture

The project follows a strict **MVVM** structure with reusable base classes and interfaces to keep all 20 modules consistent:

```
Prison/
├── Core/
│   ├── ObservableObject.cs       # INotifyPropertyChanged base class
│   └── RelayCommand.cs           # ICommand implementation with CanExecute
│
├── Data/
│   ├── DataSender.cs             # HttpClient wrapper: GET/POST/PUT/DELETE
│   ├── ApiConnector.cs           # Generic JSON deserializer on top of DataSender
│   ├── TableHelper.cs            # Generic sort & CSV export (reflection-based)
│   ├── ICRUD.cs                  # Interface: Create/Read/Update/Delete/Clear
│   └── ITableModel.cs            # Interface: CanRecover, Recover, Drop, Export
│
└── MVVM/
    ├── Model/
    │   ├── PrisonContext.cs       # EF Core DbContext (20 DbSets, Fluent API config)
    │   ├── Prisoner.cs            # Model with ICloneable (safe edit copy pattern)
    │   ├── Worker.cs              # Staff model with login/password/role
    │   └── ... (18 more models)
    ├── ViewModel/
    │   ├── MainViewModel.cs       # Navigation host: manages current view & prev/next
    │   ├── TableViewModel.cs      # Base VM: CRUD RelayCommands shared across modules
    │   ├── PrisonerViewModel.cs   # Full CRUD + soft-delete + validation + export
    │   └── ... (19 more ViewModels)
    └── View/
        ├── AuthorizationWindow.xaml.cs  # Login: fetches worker, routes by post name
        └── ... (20 XAML views)
```

## 🔑 Key Design Decisions

**Generic API layer** — `ApiConnector.GetAll<T>(tableName)` deserializes any endpoint into `ObservableCollection<T>` in one line. All 20 ViewModels reuse the same `GetAll`, `PostRequest`, `PutRequest`, and `DeleteRequest` methods without duplication.

**Safe edit pattern** — when a row is selected, the ViewModel clones it via `ICloneable` into a separate `ForEdit` object. The user edits the clone; the original is only overwritten on confirmed save, preventing accidental mutations to bound UI data.

**Soft delete** — records are flagged `IsDeleted = true` via PUT rather than hard-deleted. The ViewModel splits the API response into two `ObservableCollection`s (active / deleted) and exposes a Recover command to restore them.

**Role-based view routing** — after login, `AuthorizationWindow` maps the worker's post name to a specific array of ViewModels and injects it into `MainViewModel.views`, so each role only sees their permitted modules — no extra access control logic needed in the views.

**Reflection-based generic sort** — `TableHelper.Sort<T>()` uses `GetProperty(propertyName).GetValue()` to sort any `ObservableCollection<T>` by any column without writing per-type sorting code.

## 👥 Access Roles

| Role | Accessible Modules |
|---|---|
| Администратор | All 20 modules |
| Бухгалтер | Sales, workers, prisoners, journals, accounting |
| Работник кухни | Dishes, dining visits, meal sets |
| Продавец | Sales accounting, products |
| Сотрудник склада | Warehouse, products, product types |
| Член комиссии УДО | Dining visits, prisoner accounting, rehabilitation, sales |

## 🚀 Getting Started

### Prerequisites

- Windows OS
- Visual Studio 2019+ with .NET desktop development workload
- .NET 5 SDK

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/NackBard/Prison.git
   ```

2. Open `Prison.sln` in Visual Studio.

3. The app connects to the remote API at `https://prisonapi2.azurewebsites.net/api` — no local database setup needed to run the client.

4. Run the project (F5) and log in with your credentials.

---

> Built with ❤️ using C#, WPF, MVVM, Entity Framework Core and Azure REST API
