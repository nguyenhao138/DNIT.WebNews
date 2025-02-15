using DNIT.Core.Models;
using DNIT.Core.Interface;
//using DNIT.Core.Service;
using DNIT.Dao.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DNIT.WebAPI.Controllers.NewsAdmin
{
  [ApiController]
  [Route("[controller]")]
  public class AuthController : ControllerBase
  {

    private readonly IAuthService _authService;
    private readonly UserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher<AccountModel> _passwordHasher;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IPasswordHasher<AccountModel> passwordHasher, IAuthService authService, IJwtService jwtService, UserRepository userRepository, ILogger<AuthController> logger)
    {
      _jwtService = jwtService;
      _passwordHasher = passwordHasher;
      _authService = authService;
      _userRepository = userRepository;
      _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AccountModel model) 
    {

      var user = await _userRepository.GetByUsername(model.Username);
      if (user == null)
        return Unauthorized("Account not found");

      var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
      if (passwordVerificationResult == PasswordVerificationResult.Failed)
        return Unauthorized("Incorrect Password");

      var accessToken = _jwtService.GenerateAccessToken(user.Id);
      var refreshToken = _jwtService.GenerateRefreshToken();
      await _userRepository.UpdateRefreshToken(user.Id, refreshToken, DateTime.UtcNow.AddDays(7));

      return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });

      
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] AccountModel model)
    {
      var user = await _userRepository.GetByUsername(model.Username);

      if (user == null || user.RefreshToken != model.RefreshToken)
      {
        return Unauthorized();
      }

      // Làm mới Access Token
      var newAccessToken = _jwtService.GenerateAccessToken(user.Id);
      var newRefreshToken = _jwtService.GenerateRefreshToken();

      await _userRepository.UpdateRefreshToken(user.Id, newRefreshToken, DateTime.UtcNow.AddDays(7));

      return Ok(new
      {
        AccessToken = newAccessToken,
        RefreshToken = newRefreshToken
      });
    }

    // ✅ GET: api/AuthModels (Lấy danh sách AuthModel)
    [HttpGet]
    public async Task<List<AccountModel>> Get() =>
        await _authService.ListAccount();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<AccountModel>> Get(string id)
    {
      var book = await _authService.GetAccount(id);

      if (book is null)
      {
        return NotFound();
      }

      return book;
    }

    [HttpPost]
    public async Task<IActionResult> Post(AccountModel newBook)
    {
      await _authService.CreateAccount(newBook);

      return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, AccountModel updatedBook)
    {
      var book = await _authService.GetAccount(id);

      if (book is null)
      {
        return NotFound();
      }

      updatedBook.Id = book.Id;

      await _authService.UpdateAccount(id, updatedBook);

      return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
      var book = await _authService.GetAccount(id);

      if (book is null)
      {
        return NotFound();
      }

      await _authService.RemoveAccount(id);

      return NoContent();
    }

  } 
}
