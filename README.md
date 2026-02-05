# Hi-Tech Order Management System (HiTech OMS)

Windows Forms multi-tier application for **Hi-Tech Distribution Inc.**  
Built with **C#**, **SQL Server 2022**, **ADO.NET (connected + disconnected)**, and **Entity Framework**.

## Features
- ✅ Secure login (username/password) + change password
- ✅ Users & Employees (MIS Manager)
- ✅ Customers (Sales Manager)
- ✅ Books, Authors, Publishers, Categories (Inventory Controller)
- ✅ Orders (Order Clerks)
- ✅ Order confirmation email to customers
- ✅ Validation & error handling across all forms

## Tech Stack
- **.NET**: .NET 6/7 (or .NET Framework if required by course)
- **UI**: WinForms
- **DB**: SQL Server 2022
- **Data Access**:
  - ADO.NET Connected Mode (Users/Employees)
  - ADO.NET Disconnected Mode (Customers)
  - Entity Framework (Orders)
- **Architecture**: 3-Tier / Multi-layer (UI, BLL, DAL, Entities)

## Screenshots
> Add screenshots in `/docs/assets/` and link them here.

## Database
- ERD: `/database/diagrams/`
- Schema: `/database/schema.sql`
- Seed data: `/database/seed.sql`

## Getting Started
### Prerequisites
- Visual Studio 2022
- SQL Server 2022 + SSMS
- .NET SDK (if using .NET 6/7)

### Setup
1. Clone the repo
2. Run `/database/schema.sql`
3. Run `/database/seed.sql`
4. Update connection string in `appsettings.json` (or App.config)
5. Run the solution

## Project Documentation
All documents are in `/docs/`:
- Project Description
- Database Design
- Class Diagram
- GUI Design
- Data Access Design
- Testing
- Deployment

## Team
- **Krunal Joshi** – (roles + contribution)
- (Partner name) – (roles + contribution)

## License
MIT
