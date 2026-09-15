using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Generation.GenValApp.Helpers;

namespace GenValApp.Controllers
{
    [ApiController]
    [Route("api/v1/algorithms")]
    public class AlgorithmsController : ControllerBase
    {
     private readonly IAlgorithmInfoService _algorithmInfoService;

     public AlgorithmsController(IAlgorithmInfoService algorithmInfoService)
      {
        _algorithmInfoService = algorithmInfoService;
      }

        [HttpGet()]
        public IActionResult GetSupportedAlgorithms()
        {
            try
            {
                var algoModes = _algorithmInfoService.GetSupportedAlgorithms();

                return Ok(algoModes);
            }
            catch (Exception ex)
            {
                  return StatusCode(500, new
                     {
                          message = "Failed to retrieve supported algorithms.",
                          error = ex.Message
                     });
            }
        }
    }
}