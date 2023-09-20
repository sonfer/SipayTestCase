using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SipayTestCase.Contract.Commons;
using SipayTestCase.Contract.Dtos;
using SipayTestCase.Shared.Helpers;
using SipayTestCase.Shared.Responses;
using SipayTestCase.Web.Models;

namespace SipayTestCase.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CommonConfiguration _commonConfiguration;

    public HomeController(ILogger<HomeController> logger, IOptions<CommonConfiguration> commonConfigurationOption)
    {
        _logger = logger;
        _commonConfiguration = commonConfigurationOption.Value;
    }

    public IActionResult Index()
    {
        return View();
    }
    
}