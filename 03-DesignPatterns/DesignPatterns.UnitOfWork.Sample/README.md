# Unit of Work Pattern Sample – Customer Entity

A minimal, clean and educational example demonstrating the **Unit of Work** design pattern in C# with an in-memory data context, using a `Customer` entity.

---

## 🟧 What is the Unit of Work Pattern?

The **Unit of Work (UoW)** pattern is a software design pattern used to group one or more operations (usually database operations) into a single "unit" or transaction.  
This means that either all the operations succeed (are committed), or all fail (are rolled back). The pattern helps maintain data integrity and makes the business logic clearer.

**Key Benefits:**
- Groups multiple operations into a single transaction boundary.
- Prevents partial/unfinished data updates.
- Manages commits and rollbacks centrally.
- Makes complex business operations easier to maintain and test.

---

## 🚀 How to Run

1. Clone or download this repository.
2. Open the solution/folder in Visual Studio, Rider, or VS Code.
3. Restore NuGet packages (if needed).
4. Run the project:
   ```bash
   dotnet run
