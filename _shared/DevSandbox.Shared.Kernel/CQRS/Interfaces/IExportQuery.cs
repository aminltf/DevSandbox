using DevSandbox.Shared.Kernel.Paging;
using DevSandbox.Shared.Kernel.Search;
using DevSandbox.Shared.Kernel.Sorting;

namespace DevSandbox.Shared.Kernel.CQRS.Interfaces;

public interface IExportQuery
{
    PageRequest Page { get; set; }
    SearchRequest Search { get; set; }
    SortOptions Sort { get; set; }
    List<string> SelectedFields { get; set; }
}
