﻿using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.FFC;
using NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.FFC
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private TestGroupGenerator _subject;
        private Mock<IOracle> _oracle;

        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();
            _oracle
                .Setup(s => s.GetDsaDomainParametersAsync(It.IsAny<DsaDomainParametersParameters>()))
                .Returns(() => Task.FromResult(new DsaDomainParametersResult()
                {
                    P = 1,
                    Q = 2,
                    G = 3
                }));

            _subject = new TestGroupGenerator(_oracle.Object);
        }

        private static object[] _testShouldReturnCorrectNumberOfGroups = new object[]
        {
            new object[]
            {
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder()
                            .WithDhEphem(
                                new SchemeBuilder()
                                    .WithRole(new string[] { "initiator" }) // 1
                                    .WithNoKdfNoKc(
                                        new NoKdfNoKcBuilder()
                                            .WithParameterSets(
                                                new ParameterSetBuilder(false)
                                                    .WithFb(
                                                        new ParameterSetBaseBuilder(false)
                                                            .WithHashAlg(new string[] {"SHA2-256"}) // 1
                                                            .BuildParameterSetBaseFb()
                                                    )
                                                    .WithFc(null)
                                                    .BuildParameterSets()
                                            )
                                            .BuildNoKdfNoKc()
                                    )
                                    .WithKdfNoKc(null)
                                    .Build<FfcDhEphem>()
                            )
                            .BuildSchemes()
                    )
                    .BuildParameters(),
                2 // 1 * 2 (for test types)
            },
            new object[]
            {
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder()
                            .WithDhEphem(
                                new SchemeBuilder()
                                    .WithRole(new string[] { "initiator" }) // 1
                                    .WithNoKdfNoKc(
                                        new NoKdfNoKcBuilder()
                                            .WithParameterSets(
                                                new ParameterSetBuilder(false)
                                                    .WithFb(
                                                        new ParameterSetBaseBuilder(false)
                                                            .WithHashAlg(new string[] {"SHA2-256"}) // 1
                                                            .BuildParameterSetBaseFb()
                                                    )
                                                    .WithFc(new ParameterSetBaseBuilder(false)
                                                        .WithHashAlg(new string[] {"SHA2-256"}) // 2
                                                        .BuildParameterSetBaseFc())
                                                    .BuildParameterSets()
                                            )
                                            .BuildNoKdfNoKc()
                                    )
                                    .WithKdfNoKc(null)
                                    .Build<FfcDhEphem>()
                            )
                            .BuildSchemes()
                    )
                    .BuildParameters(),
                4 // 2 * 2 (for test types)
            },
            new object[]
            {
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder()
                            .WithDhEphem(
                                new SchemeBuilder()
                                    .WithRole(new string[] { "responder" }) // 1
                                    .WithNoKdfNoKc(
                                        new NoKdfNoKcBuilder()
                                            .WithParameterSets(
                                                new ParameterSetBuilder(false)
                                                    .WithFb(
                                                        new ParameterSetBaseBuilder(false)
                                                            .WithHashAlg(new string[] {"SHA2-256"}) // 1
                                                            .BuildParameterSetBaseFb()
                                                    )
                                                    .WithFc(new ParameterSetBaseBuilder(false)
                                                        .WithHashAlg(new string[] {"SHA2-256", "SHA2-512"}) // 2 - only use one sha per parameter set for NoKdfNoKc
                                                        .BuildParameterSetBaseFc())
                                                    .BuildParameterSets()
                                            )
                                            .BuildNoKdfNoKc()
                                    )
                                    .WithKdfNoKc(null)
                                    .Build<FfcDhEphem>()
                            )
                            .BuildSchemes()
                    )
                    .BuildParameters(),
                4 // 2 * 2 (for test types)
            },
            new object[]
            {
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder()
                            .WithDhEphem(
                                new SchemeBuilder()
                                    .WithRole(new string[] { "initiator", "responder" }) // 1,2,3,4
                                    .WithNoKdfNoKc(
                                        new NoKdfNoKcBuilder()
                                            .WithParameterSets(
                                                new ParameterSetBuilder(false)
                                                    .WithFb(
                                                        new ParameterSetBaseBuilder(false)
                                                            .WithHashAlg(new string[] {"SHA2-256"}) // 1, 2
                                                            .BuildParameterSetBaseFb()
                                                    )
                                                    .WithFc(new ParameterSetBaseBuilder(false)
                                                        .WithHashAlg(new string[] {"SHA2-256"}) // 3, 4
                                                        .BuildParameterSetBaseFc())
                                                    .BuildParameterSets()
                                            )
                                            .BuildNoKdfNoKc()
                                    )
                                    .WithKdfNoKc(null)
                                    .Build<FfcDhEphem>()
                            )
                            .BuildSchemes()
                    )
                    .BuildParameters(),
                8 // 4 * 2 (for test types)
            },
            new object[]
            {
                new ParameterBuilder()
                    .WithSchemes(
                        new SchemesBuilder()
                            .WithDhEphem(
                                new SchemeBuilder()
                                    .WithRole(new string[] { "initiator", "responder" }) // 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
                                    .WithNoKdfNoKc(
                                        new NoKdfNoKcBuilder()
                                            .WithParameterSets(
                                                new ParameterSetBuilder(false)
                                                    .WithFb(
                                                        new ParameterSetBaseBuilder(false)
                                                            .WithHashAlg(new string[] {"SHA2-256"}) // 1, 2
                                                            .BuildParameterSetBaseFb()
                                                    )
                                                    .WithFc(new ParameterSetBaseBuilder(false)
                                                        .WithHashAlg(new string[] {"SHA2-256"}) // 3, 4
                                                        .BuildParameterSetBaseFc())
                                                    .BuildParameterSets()
                                            )
                                            .BuildNoKdfNoKc()
                                    )
                                    .WithKdfNoKc(
                                        new KdfNoKcBuilder()
                                            .WithKdfOptions(
                                                new KdfOptionsBuilder()
                                                    .WithAsn1("uPartyInfo||vPartyInfo")  // 5, 6, 7, 8
                                                    .WithConcatenation("uPartyInfo||vPartyInfo") // 9, 10, 11, 12
                                                    .BuildKdfOptions()
                                            )
                                            .WithParameterSets(
                                                new ParameterSetBuilder(true)
                                                    .WithFb(
                                                        new ParameterSetBaseBuilder(true)
                                                            .WithHashAlg(new string[] {"SHA2-256"}) // 5, 6, 9, 10
                                                            .WithMacOptions(
                                                                new MacOptionsBuilder()
                                                                    .WithHmac2_224(
                                                                        new MacOptionsBaseBuilder(true)
                                                                            .BuildHmac2_224() // 5, 6, 9, 10
                                                                    )
                                                                    .WithAesCcm(null)
                                                                    .WithCmac(null)
                                                                    .WithHmac2_256(null)
                                                                    .WithHmac2_384(null)
                                                                    .WithHmac2_512(null)
                                                                    .BuildMacOptions()
                                                            )
                                                            .BuildParameterSetBaseFb()
                                                    )
                                                    .WithFc(new ParameterSetBaseBuilder(true)
                                                        .WithHashAlg(new string[] {"SHA2-256"}) // 7, 8, 11, 12
                                                        .WithMacOptions(
                                                            new MacOptionsBuilder()
                                                                .WithHmac2_224(
                                                                    new MacOptionsBaseBuilder(true)
                                                                        .BuildHmac2_224() // 7, 8, 11, 12
                                                                )
                                                                .WithAesCcm(null)
                                                                .WithCmac(null)
                                                                .WithHmac2_256(null)
                                                                .WithHmac2_384(null)
                                                                .WithHmac2_512(null)
                                                                .BuildMacOptions()
                                                        )
                                                        .BuildParameterSetBaseFc())
                                                    .BuildParameterSets()
                                            )
                                            .BuildKdfNoKc()
                                    )
                                    .Build<FfcDhEphem>()
                            )
                            .BuildSchemes()
                    )
                    .BuildParameters(),
                24 // 12 * 2 (for test types)
            },
        };

        [Test]
        [TestCaseSource(nameof(_testShouldReturnCorrectNumberOfGroups))]
        public async Task ShouldReturnCorrectNumberOfGroups(Parameters parm, int expectedNumberOfGroups)
        {
            var result = await _subject.BuildTestGroupsAsync(parm);

            Assert.That(result.Count(), Is.EqualTo(expectedNumberOfGroups));
        }
    }
}
