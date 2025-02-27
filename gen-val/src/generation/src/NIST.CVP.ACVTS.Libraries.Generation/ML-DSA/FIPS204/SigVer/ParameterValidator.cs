﻿using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.PqcHelpers;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigVer;

public class ParameterValidator : PqcParameterValidator, IParameterValidator<Parameters>
{
    public static readonly DilithiumParameterSet[] ValidParameterSets = EnumHelpers.GetEnums<DilithiumParameterSet>().Except(new [] { DilithiumParameterSet.None }).ToArray();
    
    public ParameterValidateResponse Validate(Parameters parameters)
    {
        var errors = new List<string>();

        ValidateAlgoMode(parameters, new[] { AlgoMode.ML_DSA_SigVer_FIPS204 }, errors);
        ValidateSignatureInterfacesAndPreHash(parameters, errors);
        ValidateCapabilities(parameters, errors);
        ValidateExternalMu(parameters, errors);

        return errors.Any() ? new ParameterValidateResponse(errors) : new ParameterValidateResponse();
    }

    private void ValidateCapabilities(Parameters parameters, List<string> errors)
    {
        // 1) was Capabilities included, but empty?
        if (!parameters.Capabilities.Distinct().Any())
        {
            errors.Add($"Expected {nameof(parameters.Capabilities)} to not be empty");
            return;
        }
        
        // 2) examine each Capability that was provided
        foreach (var capability in parameters.Capabilities)
        {
            // i) is ParameterSets non-empty?
            if (!capability.ParameterSets.Distinct().Any())
            {
                errors.Add($"Expected {nameof(capability.ParameterSets)} to contain at least one valid ML-DSA parameter set");
                return;
            }

            // ii) check no duplicates are provided
            if (capability.ParameterSets.Length != capability.ParameterSets.Distinct().Count())
            {
                errors.Add($"{nameof(capability.ParameterSets)} must not contain the same ML-DSA parameter set more than once");
            }
            
            // iii) run the base validator on each capability
            ValidateCapability(capability, parameters, errors);
        }
    }

    private void ValidateExternalMu(Parameters parameters, List<string> errors)
    {
        if (parameters.ExternalMu.Distinct().Count() != parameters.ExternalMu.Length)
        {
            errors.Add("Expected no duplicates in external mu");
        }

        if (parameters.ExternalMu.Contains(true) && !parameters.SignatureInterfaces.Contains(SignatureInterface.Internal))
        {
            errors.Add("Expected external mu must be tested with internal signature interface");
        }
    }
}
