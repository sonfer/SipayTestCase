using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SipayTestCase.Contract.Commons;
using SipayTestCase.Contract.Dtos;
using SipayTestCase.Shared.Helpers;
using SipayTestCase.Shared.Responses;
using SipayTestCase.Web.Models;

namespace SipayTestCase.Web.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly CommonConfiguration _commonConfiguration;

    public UserController(ILogger<UserController> logger, IOptions<CommonConfiguration> commonConfiguration)
    {
        _logger = logger;
        _commonConfiguration = commonConfiguration.Value;
    }

    public IActionResult Index()
    {
        return View();
    }


    public IActionResult Login()
    {
        var loginViewModel = new LoginViewModel();
        return View(loginViewModel);
    }


    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        var loginResponse = ApiHelper.PostApiResponse<DataResult<UserDto>, object>(new
        {
            Email = viewModel.Email,
            Password = viewModel.Password
        }, new ApiModel() { Url = $"{_commonConfiguration.ApiUrl}/api/User/Login" });

        if (loginResponse.IsSuccessful() && loginResponse.Data.Success)
        {
            //session token
            var userDto = loginResponse.Data.Data;
            HttpContext.Session.SetString("user-token-session", JsonSerializer.Serialize(userDto.AuthToken));

            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    
    public async Task<IActionResult> Register()
    {
        var registerViewModel = new RegisterViewModel();
        return View(registerViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        var registerResponse = ApiHelper.PostApiResponse<DataResult<UserDto>, object>(new
        {
            Email = viewModel.Email,
            FullName= viewModel.FullName,
            Password = viewModel.Password
        }, new ApiModel() { Url = $"{_commonConfiguration.ApiUrl}/User/Register" });

        if (registerResponse.IsSuccessful() && registerResponse.Data.Success)
        {
            //session token
            var userDto = registerResponse.Data.Data;
            HttpContext.Session.SetString("user--token-session", JsonSerializer.Serialize(userDto.AuthToken));

            return RedirectToAction("Index", "Home");
        }

        return View();
    }
}