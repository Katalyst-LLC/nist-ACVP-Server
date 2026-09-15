using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GenValApp.Controllers;
using GenValAppRunner.DTO;
using NIST.CVP.ACVTS.Generation.GenValApp.Helpers;
using NIST.CVP.ACVTS.Generation.GenValApp.Models;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Common;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using Autofac;

namespace NIST.CVP.ACVTS.Libraries.Generation.GenValApp.Tests.Controllers
{
public class VectorSetsControllerTests
  {
    private Mock<IVectorSetService> _mockService;
    private VectorSetsController _controller;

    [SetUp]
    public void Setup()
    {
        _mockService = new Mock<IVectorSetService>();
        _controller = new VectorSetsController(_mockService.Object);
    }

    // =========================
    // GENERATE TESTS
    // =========================

    [Test]
    public async Task Generate_ReturnsOk_WithExpectedResponse()
    {
        // Arrange
        var expectedResponse = new VectorSetResponse
        {
            StatusCode = StatusCode.Success,
            ErrorMessage = null,
            Result = new TestVectorSet
            {
                VsId = 1,
                Algorithm = "ACVP-AES-CBC"
            }
        };

        _mockService
            .Setup(s => s.GenerateAsync(It.IsAny<Registration>()))
            .ReturnsAsync(expectedResponse);

        var registration = new Registration
        {
            VsId = 1,
            Algorithm = "ACVP-AES-CBC",
            Revision = "1.0"
        };

        // Act
        var result = await _controller.Generate(registration);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as VectorSetResponse;

        Assert.That(response, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Success));
        Assert.That(response.ErrorMessage, Is.Null);
        Assert.That(response.Result.VsId, Is.EqualTo(1));
        Assert.That(response.Result.Algorithm, Is.EqualTo("ACVP-AES-CBC"));
    }

    [Test]
    public async Task Generate_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Algorithm", "Required");

        var registration = new Registration
        {
            Revision = "1.0"
        };

        // Act
        var result = await _controller.Generate(registration);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());

        var badRequest = result.Result as BadRequestObjectResult;
        Assert.That(badRequest.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public async Task Generate_Returns500_WhenServiceThrowsException()
    {
        // Arrange
        _mockService
            .Setup(s => s.GenerateAsync(It.IsAny<Registration>()))
            .ThrowsAsync(new Exception("Something went wrong"));

        var registration = new Registration
        {
            Algorithm = "ACVP-AES-CBC",
            Revision = "1.0"
        };

        // Act
        var result = await _controller.Generate(registration);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<ObjectResult>());

        var objectResult = result.Result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(500));
        Assert.That(objectResult.Value.ToString(), Does.Contain("Something went wrong"));
    }

    // =========================
    // VALIDATE TESTS
    // =========================

    [Test]
    public async Task Validate_ReturnsOk_WithExpectedValidationResponse()
    {
        // Arrange
        var expectedResponse = new ValidationResponse
        {
            StatusCode = StatusCode.Success,
            ErrorMessage = null,
            Result = new VectorSetValidationResults
            {
                VsId = 0,
                Disposition = "passed"
            }
        };

        _mockService
            .Setup(s => s.ValidateAsync(It.IsAny<ValidationRequest>()))
            .ReturnsAsync(expectedResponse);

        var request = new ValidationRequest
        {
            Answer = new TestVectorSet
            {
                Algorithm = "ACVP-AES-CBC",
                Revision = "1.0"
            },
            Expected = new TestVectorSet
            {
                Algorithm = "ACVP-AES-CBC",
                Revision = "1.0"
            }
        };

        // Act
        var result = await _controller.Validate(request);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as ValidationResponse;

        Assert.That(response, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(StatusCode.Success));
        Assert.That(response.ErrorMessage, Is.Null);
        Assert.That(response.Result.Disposition, Is.EqualTo("passed"));
    }

    [Test]
    public async Task Validate_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Answer", "Answer is required");

        var request = new ValidationRequest
        {
            Answer = null,
            Expected = new TestVectorSet()
        };

        // Act
        var result = await _controller.Validate(request);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());

        var badRequest = result.Result as BadRequestObjectResult;
        Assert.That(badRequest.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public async Task Validate_Returns500_WhenServiceThrowsException()
    {
        // Arrange
        _mockService
            .Setup(s => s.ValidateAsync(It.IsAny<ValidationRequest>()))
            .ThrowsAsync(new Exception("Validation failed"));

        var request = new ValidationRequest
        {
            Answer = new TestVectorSet
            {
                Algorithm = "ACVP-AES-CBC",
                Revision = "1.0"
            },
            Expected = new TestVectorSet
            {
                Algorithm = "ACVP-AES-CBC",
                Revision = "1.0"
            }
        };

        // Act
        var result = await _controller.Validate(request);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<ObjectResult>());

        var objectResult = result.Result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(500));
        Assert.That(objectResult.Value.ToString(), Does.Contain("Validation failed"));
    }
  }

}