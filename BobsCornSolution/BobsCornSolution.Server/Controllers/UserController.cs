using System.Net;
using BobsCorn.Application.DTOs;
using BobsCorn.Application.Interfaces;
using BobsCorn.Domain.Common;
using BobsCorn.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BobCornSolution.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        public UserController(IProductService productService, IUserRepository userRepository, ITokenService tokenService)
        {
            _productService = productService;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            if (userLoginDto == null)
            {
                return BadRequest("User login data cannot be null.");
            }

            try
            {
                var result = await _userRepository.LoginAsync(userLoginDto);

                if (result.Item1 == HttpStatusCode.BadRequest)
                {
                    return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, HttpStatusCode.BadRequest.ToString(), result.Item2));
                }
                else if (result.Item1 == HttpStatusCode.NotFound)
                {
                    return NotFound(new ApiResponse<string>((int)HttpStatusCode.NotFound, HttpStatusCode.NotFound.ToString(), result.Item2));
                }

                var response = new ApiResponse<string>(200, "Logged successfully.", _tokenService.GenerateToken(userLoginDto.Email));
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<string>(500, "An internal server error occurred.", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            if (userRegisterDto == null)
            {
                return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, "User registration data cannot be null.", ""));
            }

            try
            {
                var result = await _userRepository.AddUserAsync(userRegisterDto);

                if (result.Item1 == HttpStatusCode.BadRequest)
                {
                    return BadRequest(new ApiResponse<string>((int)HttpStatusCode.BadRequest, HttpStatusCode.BadRequest.ToString(), result.Item2));
                }

                return Ok(result.Item2);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<string>(500, "An internal server error occurred.", ex.Message);
                return StatusCode(500, errorResponse);
            }

        }

        [HttpPost("buy")]
        [Authorize]
        public async Task<IActionResult> Buy([FromBody] ProductBuyDto request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be empty.");
            }
            try
            {
                var result = await _productService.BuyCornAsync(request);

                if (result.Item1 == HttpStatusCode.TooManyRequests)
                {
                    var errorResponse = new ApiResponse<string>((int)HttpStatusCode.TooManyRequests, result.Item2, "");
                    return StatusCode((int)HttpStatusCode.TooManyRequests, errorResponse);
                }

                return Ok(result.Item2);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<string>(500, "An internal server error occurred.", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet("report/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetReport(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }
            var report = await _productService.GetPurchaseReportAsync(userId);
            if (report == null || !report.Any())
            {
                return NotFound("No purchase records found for this user.");
            }
            return Ok(report);
        }
    }
}
