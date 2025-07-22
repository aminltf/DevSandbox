using DevSandbox.Shared.Kernel.CQRS.Interfaces;
using DevSandbox.Shared.Kernel.Paging;
using DevSandbox.Shared.Kernel.Search;
using DevSandbox.Shared.Kernel.Sorting;

namespace DevSandbox.Shared.Kernel.CQRS.Queries;

public class ExportQuery : IExportQuery
{
    public PageRequest Page { get; set; } = new();
    public SearchRequest Search { get; set; } = new();
    public SortOptions Sort { get; set; } = new();
    public List<string> SelectedFields { get; set; } = new();
}
