using AuthAuthApplicationServices;
using AuthAuthDomaineService;
using AuthAuthMicroService.SwaggerTemplateData;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthAuthMicroService.Controllers;
[Route("api/[controller]")]
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

    
    [HttpPost(Name ="creation")]
    public IActionResult Create(User user)
    {
        var status = accountManager.Create(user.Login, user.Password);
        switch (status)
        {
            case AccountCreationStatus.Created: return Created("account","SUCCES");
            case AccountCreationStatus.AlreadyExisting: return Conflict();
            case AccountCreationStatus.Error: return BadRequest();
            default: throw new Exception();
        }
    }
    /*
    // GET api/<UserManagementController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<UserManagementController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<UserManagementController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<UserManagementController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
    */
}
