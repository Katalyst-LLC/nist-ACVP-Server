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

public interface IAlgoExecutionFactory
{
    IGenerator CreateGenerator(AlgoMode mode);
    IValidator CreateValidator(AlgoMode mode);
}

public class AlgoExecutionFactory : IAlgoExecutionFactory
{
    private readonly IAlgoModeContainerRegistry _containerRegistry;

    public AlgoExecutionFactory(IAlgoModeContainerRegistry containerRegistry)
    {
        _containerRegistry = containerRegistry;
    }

    public IGenerator CreateGenerator(AlgoMode mode)
    {
        var scope = _containerRegistry.BeginScope(mode);

        var generator = scope.Resolve<IGenerator>();

        return new ScopedGenerator(generator, scope);
    }

    public IValidator CreateValidator(AlgoMode mode)
    {
        var scope = _containerRegistry.BeginScope(mode);

        var validator = scope.Resolve<IValidator>();

        return new ScopedValidator(validator, scope);
    }
}
