using DevSandbox.Shared.Kernel.Enums;

namespace DevSandbox.Shared.Kernel.Sorting;

public class SortOptions
{
    public List<string> Fields { get; init; } = new();
    public SortDirection Direction { get; init; } = SortDirection.Asc;
}
