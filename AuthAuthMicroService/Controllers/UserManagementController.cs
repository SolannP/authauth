using AuthAuthApplicationServices;
using AuthAuthDomaineService;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthAuthMicroService.Controllers;
[Route("api/user_management")]
[ApiController]
public class UserManagementController : ControllerBase
{
    AccountManager accountManager;
    public UserManagementController(IConfiguration configurationJsonFile)
    {
        var redisData = configurationJsonFile.GetSection("RedisDataConnection");
        BuilderApplicationService builder = new BuilderApplicationService();

        builder.BuildRedisDataBaseConnection(
            redisData.GetSection("User").Value, 
            redisData.GetSection("Password").Value, 
            redisData.GetSection("Endpoint").Value
        );
        accountManager = builder.Make();
    }

    /// <summary>
    /// API dediacted to the user creation
    /// </summary>
    /// <param name="contact">Way to contact the user or to adress him. If empty take the value of the login</param>
    /// <param name="login"> Login for the user</param>
    /// <param name="password">Password for the user</param>
    /// <response code="201">Succefull request creation</response>
    /// <response code="409">Cannot make a request : already existing user</response>
    /// <response code="500">Something went wrong on the server side</response>
    /// <remarks>Use redis database for performance, please do not create to much user</remarks>
    /// <exception cref="ArgumentOutOfRangeException">Rare case when result is in unknow state, i.e. not among Created|AlreadyExisting|Error </exception>
    [HttpPost("create")]
    public IActionResult Create(string? contact,[Required] string login , [Required] string password)
    {
        AccountCreationStatus status;
        status = contact is null ? accountManager.Create(login, password) : accountManager.Create(contact, login, password);

        switch (status)
        {
            case AccountCreationStatus.Created: return Created("account","SUCCES");
            case AccountCreationStatus.AlreadyExisting: return Conflict("Already existing user");
            case AccountCreationStatus.Error: return StatusCode(500);
            default: throw new ArgumentOutOfRangeException("AccountCreationStatus not in possible value");
        }
    }

    [HttpGet("correct")]
    public IActionResult Correct(string? contact, [Required] string login, [Required] string password)
    {
        AccountAccesStatus status;
        status = contact is null ? accountManager.IsCorrectPassword(login, password) : accountManager.IsCorrectPassword(contact, login, password);
        
        switch (status)
        {
            case AccountAccesStatus.CorrectCredential:return Ok("SUCCES");
            case AccountAccesStatus.IncorectCredential: return BadRequest();
            case AccountAccesStatus.Error: return StatusCode(500);
            default:throw new ArgumentOutOfRangeException("AccountAccesStatus not in possible value");
        }
    }
}
