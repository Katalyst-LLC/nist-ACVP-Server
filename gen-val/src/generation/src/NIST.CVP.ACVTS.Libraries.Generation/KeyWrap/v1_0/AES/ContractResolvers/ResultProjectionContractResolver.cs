﻿using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.AES.ContractResolvers
{
    public class ResultProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.Tests)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional properties
            if (jsonProperty.UnderlyingName == nameof(TestCase.TestPassed))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if ((testCase.TestPassed != null && !testCase.TestPassed.Value))
                        {
                            return true;
                        }

                        return false;
                    };
            }

            if (jsonProperty.UnderlyingName == nameof(TestCase.PlainText))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if ((testCase.TestPassed != null && testCase.TestPassed.Value) &&
                            testGroup.Direction.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }

                        return false;
                    };
            }

            if (jsonProperty.UnderlyingName.Equals(nameof(TestCase.CipherText), StringComparison.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize = instance =>
                {
                    GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                    if (testGroup.Direction.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return false;
                };
            }
            #endregion Conditional properties

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}
