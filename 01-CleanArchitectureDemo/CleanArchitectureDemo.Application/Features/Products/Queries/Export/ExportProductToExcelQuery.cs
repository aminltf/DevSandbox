using AutoMapper;
using CleanArchitectureDemo.Application.Common.Interfaces.Repositories;
using CleanArchitectureDemo.Application.Features.Products.Dtos;
using DevSandbox.Shared.Kernel.CQRS.Queries;
using DevSandbox.Shared.Kernel.Interfaces.Services;
using MediatR;

namespace CleanArchitectureDemo.Application.Features.Products.Queries.Export;

public class ExportProductToExcelQuery : ExportQuery, IRequest<byte[]>;

public class ExportProductToExcelQueryHandler : IRequestHandler<ExportProductToExcelQuery, byte[]>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IExcelService _service;

    public ExportProductToExcelQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IExcelService service)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _service = service;
    }

    public async Task<byte[]> Handle(ExportProductToExcelQuery request, CancellationToken cancellationToken)
    {
        var entityPage = await _unitOfWork.Product.GetReportAsync(
            request.Page,
            request.Search,
            request.Sort,
            request.SelectedFields,
            cancellationToken
        );

        var reportData = _mapper.Map<List<ProductReportDto>>(entityPage.Items);

        var allFieldMappers = new Dictionary<string, Func<ProductReportDto, object?>>
        {
            ["Name"] = x => x.Name,
            ["PriceAmount"] = x => x.PriceAmount,
            ["PriceCurrency"] = x => x.PriceCurrency,
            ["StatusTitle"] = x => x.StatusTitle,
            ["Stock"] = x => x.Stock
        };

        Dictionary<string, Func<ProductReportDto, object?>> selectedFieldMappers;
        if (request.SelectedFields != null && request.SelectedFields.Any())
        {
            selectedFieldMappers = request.SelectedFields
                .Where(allFieldMappers.ContainsKey)
                .ToDictionary(f => f, f => allFieldMappers[f]);
            if (selectedFieldMappers.Count == 0)
                selectedFieldMappers = allFieldMappers;
        }
        else
            selectedFieldMappers = allFieldMappers;

        return await _service.ExportAsync(
            reportData,
            selectedFieldMappers,
            "Products"
        );
    }
}
