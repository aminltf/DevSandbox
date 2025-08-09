namespace DesignPatterns.Proxy.Sample;

// -------------------- Real Subject --------------------
public sealed class HeavyReportGenerator : IReportGenerator
{
    public string GenerateMonthlyReport(Guid departmentId)
    {
        Console.WriteLine("[RealSubject] Doing heavy work (db/compute) ...");
        Thread.Sleep(1200);
        return $"[REPORT] Dept={departmentId}, Period=CurrentMonth, GeneratedAt={DateTime.UtcNow:O}";
    }
}