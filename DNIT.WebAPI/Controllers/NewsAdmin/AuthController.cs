using DNIT.Core.Models;
using DNIT.Core.Service;
using DNIT.Dao.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DNIT.WebAPI.Controllers.NewsAdmin
{
  [ApiController]
  [Route("[controller]")]
  public class AuthController : ControllerBase
  {

    private readonly AuthService _authService;
    private readonly UserRepository _userRepository;
    private readonly JwtService _jwtService;
    private readonly IPasswordHasher<AuthModel> _passwordHasher;

    public AuthController(IPasswordHasher<AuthModel> passwordHasher, AuthService authService, JwtService jwtService, UserRepository userRepository)
    {
      _jwtService = jwtService;
      _passwordHasher = passwordHasher;
      _authService = authService;
      _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthModel model)
    {
      var user = await _userRepository.FindByUsernameAsync(model.Username);
      if (user == null)
        return Unauthorized("Not found Account");

      var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
      if (passwordVerificationResult == PasswordVerificationResult.Failed)
        return Unauthorized("Incorrect Password");

      var accessToken = _jwtService.GenerateAccessToken(user.Id);
      var refreshToken = _jwtService.GenerateRefreshToken();
      await _userRepository.UpdateRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddDays(7));

      return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] AuthModel model)
    {
      var user = await _userRepository.FindByUsernameAsync(model.Username);

      if (user == null || user.RefreshToken != model.RefreshToken)
      {
        return Unauthorized();
      }

      // Làm mới Access Token
      var newAccessToken = _jwtService.GenerateAccessToken(user.Id);
      var newRefreshToken = _jwtService.GenerateRefreshToken();

      await _userRepository.UpdateRefreshTokenAsync(user.Id, newRefreshToken, DateTime.UtcNow.AddDays(7));

      return Ok(new
      {
        AccessToken = newAccessToken,
        RefreshToken = newRefreshToken
      });
    }

    // ✅ GET: api/AuthModels (Lấy danh sách AuthModel)
    [HttpGet]
    public ActionResult<List<AuthModel>> GetAuthModels()
    {
      return _authService.Get();
    }

    // ✅ GET: api/AuthModels/{id} (Lấy AuthModel theo ID)
    [HttpGet("{id}")]
    public ActionResult<AuthModel> GetAuthModel(string id)
    {
      var authModel = _authService.Get(id);
      if (authModel == null)
      {
        return NotFound(new { message = "AuthModel not found" });
      }
      return authModel;
    }

    // ✅ POST: api/AuthModels (Tạo AuthModel mới)
    [HttpPost]
    public ActionResult<AuthModel> CreateAuthModel([FromBody] AuthModel authModel)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      _authService.Create(authModel);
      return CreatedAtAction(nameof(GetAuthModel), new { id = authModel.Id }, authModel);
    }

    // ✅ PUT: api/AuthModels/{id} (Cập nhật AuthModel)
    [HttpPut("{id}")]
    public IActionResult UpdateAuthModel(string id, [FromBody] AuthModel authModelIn)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var authModel = _authService.Get(id);
      if (authModel == null)
      {
        return NotFound(new { message = "AuthModel not found" });
      }

      _authService.Update(id, authModelIn);
      return NoContent();
    }

    // ✅ DELETE: api/AuthModels/{id} (Xóa AuthModel)
    [HttpDelete("{id}")]
    public IActionResult DeleteAuthModel(string id)
    {
      var authModel = _authService.Get(id);
      if (authModel == null)
      {
        return NotFound(new { message = "AuthModel not found" });
      }

      _authService.Remove(id);
      return NoContent();
    }

  }
}
