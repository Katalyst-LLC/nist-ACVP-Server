using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Generation.GenValApp.Helpers;
using NIST.CVP.ACVTS.Libraries.Common;
using System.Threading;
using System.Collections.Concurrent;
using GenValAppRunner.DTO;
using Autofac;
public class ScopedValidator : IValidator, IDisposable
{
    private readonly IValidator _inner;
    private readonly ILifetimeScope _scope;

    public ScopedValidator(IValidator inner, ILifetimeScope scope)
    {
        _inner = inner;
        _scope = scope;
    }

    public Task<ValidateResponse> ValidateAsync(ValidateRequest request)
        => _inner.ValidateAsync(request);

    public void Dispose()
    {
        _scope.Dispose();
    }
}
