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
using Newtonsoft.Json;
using GenValAppRunner.DTO;
using Autofac;

public interface IAlgorithmInfoService
{
    List<AlgoModeInfo> GetSupportedAlgorithms();
}
public class AlgorithmInfoService : IAlgorithmInfoService
{
    public List<AlgoModeInfo> GetSupportedAlgorithms()
    {
        return AutofacConfig.GetSupportedAlgoModeInfos();
    }
}
