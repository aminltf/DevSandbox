namespace DesignPatterns.Proxy.Sample;

// -------------------- Domain Contracts --------------------
public interface IReportGenerator
{
    string GenerateMonthlyReport(Guid departmentId);
}