using System.Data;
using DevSandbox.Shared.Kernel.Results;

namespace DevSandbox.Shared.Kernel.Interfaces.Services;

public interface IExcelService
{
    Task<byte[]> CreateTemplateAsync(IEnumerable<string> fields, string sheetName = "Sheet1");

    Task<byte[]> ExportAsync<T>(
        IEnumerable<T> data,
        Dictionary<string, Func<T, object?>> mappers,
        string sheetName = "Sheet1");

    Task<IResult<IEnumerable<T>>> ImportAsync<T>(
        byte[] fileBytes,
        Dictionary<string, Func<DataRow, T, object?>> mappers,
        string sheetName = "Sheet1");
}
