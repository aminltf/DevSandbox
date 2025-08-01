﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDemo.Web.Controllers.Base;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseController<TController> : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}
