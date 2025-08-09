// -------------------- Demo (Client) --------------------
using DesignPatterns.Proxy.Sample;

var salesDept = Guid.NewGuid();
var hrDept = Guid.NewGuid();

var admin = new UserContext { UserId = Guid.NewGuid(), Role = Role.Admin };
var manager = new UserContext { UserId = Guid.NewGuid(), Role = Role.Manager };
var employee = new UserContext { UserId = Guid.NewGuid(), Role = Role.Employee };

FakeDirectory.MapManagerToDepartment(manager.UserId, salesDept);

Console.WriteLine("=== Admin Calls ===");
IReportGenerator adminProxy = new ReportGeneratorProxy(admin);
Console.WriteLine(adminProxy.GenerateMonthlyReport(salesDept));
Console.WriteLine(adminProxy.GenerateMonthlyReport(salesDept));

Console.WriteLine("\n=== Manager Calls (own dept) ===");
IReportGenerator managerProxy = new ReportGeneratorProxy(manager);
Console.WriteLine(managerProxy.GenerateMonthlyReport(salesDept));
try
{
    Console.WriteLine(managerProxy.GenerateMonthlyReport(hrDept));
}
catch (Exception ex)
{
    Console.WriteLine($"[Expected Deny] {ex.Message}");
}

Console.WriteLine("\n=== Employee Calls ===");
IReportGenerator empProxy = new ReportGeneratorProxy(employee);
try
{
    Console.WriteLine(empProxy.GenerateMonthlyReport(salesDept));
}
catch (Exception ex)
{
    Console.WriteLine($"[Expected Deny] {ex.Message}");
}

Console.WriteLine("\nDone.");