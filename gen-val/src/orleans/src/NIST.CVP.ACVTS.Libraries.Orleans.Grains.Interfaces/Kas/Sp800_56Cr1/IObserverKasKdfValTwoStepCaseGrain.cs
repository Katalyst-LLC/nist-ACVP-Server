﻿using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Cr1;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Cr1
{
    public interface IObserverKdaValTwoStepCaseGrain : IGrainWithGuidKey, IGrainObservable<KdaValTwoStepResult>
    {
        Task<bool> BeginWorkAsync(KdaValTwoStepParameters param);
    }
}
