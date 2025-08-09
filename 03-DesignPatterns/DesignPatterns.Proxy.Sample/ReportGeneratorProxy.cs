using System.Collections.Concurrent;
using System.Diagnostics;

namespace DesignPatterns.Proxy.Sample;

// -------------------- Proxy (Virtual + Protection + Caching + Logging) --------------------
public sealed class ReportGeneratorProxy : IReportGenerator
{
    private readonly UserContext _userContext;

    private readonly Lazy<HeavyReportGenerator> _real =
        new(() => new HeavyReportGenerator(), isThreadSafe: true);

    private static readonly ConcurrentDictionary<string, (string Value, DateTime CachedAt)> _cache
        = new();

    private static readonly TimeSpan _minRealGenerationGap = TimeSpan.FromSeconds(10);

    public ReportGeneratorProxy(UserContext userContext)
    {
        _userContext = userContext;
    }

    public string GenerateMonthlyReport(Guid departmentId)
    {
        EnsureAccess(departmentId);

        var key = $"monthly:{departmentId:D}";

        // --- Cache check ---
        if (_cache.TryGetValue(key, out var entry))
        {
            Console.WriteLine("[Proxy] Cache HIT");
            return entry.Value + " (from cache)";
        }

        // --- Rate limiting ---
        if (entry.Value is not null && DateTime.UtcNow - entry.CachedAt < _minRealGenerationGap)
        {
            Console.WriteLine("[Proxy] Rate limited; serving last cached result");
            return entry.Value + " (rate-limited)";
        }

        // --- Logging ---
        var sw = Stopwatch.StartNew();
        Console.WriteLine("[Proxy] Cache MISS → delegating to RealSubject");

        // --- Delegation to RealSubject ---
        var report = _real.Value.GenerateMonthlyReport(departmentId);
        sw.Stop();

        Console.WriteLine($"[Proxy] Real generation took {sw.ElapsedMilliseconds} ms");

        // --- Store in cache ---
        _cache[key] = (report, DateTime.UtcNow);

        return report;
    }

    private void EnsureAccess(Guid departmentId)
    {
        switch (_userContext.Role)
        {
            case Role.Employee:
                throw new UnauthorizedAccessException("Employees are not allowed to generate this report.");

            case Role.Manager:
                var managerDept = FakeDirectory.GetDepartmentForManager(_userContext.UserId);
                if (managerDept != departmentId)
                    throw new UnauthorizedAccessException("Managers can only access their own department reports.");
                break;

            case Role.Admin:
                // full access
                break;
        }
    }
}