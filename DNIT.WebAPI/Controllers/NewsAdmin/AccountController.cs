using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DNIT.WebAPI.Models;
using DNIT.WebAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace DNIT.WebAPI.Controllers.NewsAdmin
{
  [ApiController]
  [Route("[controller]")]
  public class AccountController : ControllerBase
  {
    
    private readonly AuthService _accountService;
    private readonly IConfiguration _configuration;
    private readonly IMongoDatabase _database;
    private readonly PasswordHasher<AccountModel> _passwordHasher;

    public AccountController(IConfiguration configuration, IMongoDatabase database, 
      PasswordHasher<AccountModel> passwordHasher,AuthService accountService)
    {
      _configuration = configuration;
      _database = database;
      _passwordHasher = passwordHasher;
      _accountService = accountService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AccountModel model)
    {
      var user = await _accountService.FindByUsernameAsync(model.Username);
      if (user == null)
      {
        return Unauthorized("Not found Account");
      }

      // So sánh mật khẩu người dùng nhập với mật khẩu đã mã hóa trong DB
      var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

      if (passwordVerificationResult == PasswordVerificationResult.Failed)
      {
        return Unauthorized("Incorrect Password");
      }


      // Tạo Access Token và Refresh Token
      var accessToken = GenerateAccessToken(user);
      var refreshToken = GenerateRefreshToken();

      // Lưu refresh token vào cơ sở dữ liệu của người dùng (tùy chọn)
      user.RefreshToken = refreshToken;
      

      return Ok(new
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken
      });
    }

    private string GenerateAccessToken(AccountModel user)
    {
      var claims = new[]
      {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            // Các claim khác tùy thuộc vào nhu cầu
        };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer: _configuration["Jwt:Issuer"],
          audience: _configuration["Jwt:Audience"],
          claims: claims,
          expires: DateTime.Now.AddMinutes(30),  // Access token hết hạn sau 30 phút
          signingCredentials: credentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
      var randomBytes = new byte[64];
      using (var rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(randomBytes);
      }

      var refreshToken = Convert.ToBase64String(randomBytes);  // Chuyển byte array thành Base64 string

      // Mã hóa refresh token bằng SHA256
      using (var sha256 = SHA256.Create())
      {
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));  // Mã hóa refresh token
        return Convert.ToBase64String(hashBytes);  // Trả về refresh token đã mã hóa
      }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] AccountModel model)
    {
      var user = await _accountService.FindByUsernameAsync(model.Username);

      if (user == null || user.RefreshToken != model.RefreshToken)
      {
        return Unauthorized();
      }

      // Làm mới Access Token
      var newAccessToken = GenerateAccessToken(user);

      var newRefreshToken = GenerateRefreshToken();
      user.RefreshToken = newRefreshToken;
     
      return Ok(new
      {
        AccessToken = newAccessToken,
        RefreshToken = newRefreshToken
      });
    }

    // GET: api/AccountModels
    [HttpGet]
    public ActionResult<List<AccountModel>> GetAccountModels() =>
        _accountService.Get();

    // GET: api/AccountModels/{id}
    [HttpGet("{id}")]
    public ActionResult<AccountModel> GetAccountModel(string id)
    {
      var AccountModel = _accountService.Get(id);
      if (AccountModel == null)
      {
        return NotFound();
      }
      return AccountModel;
    }

    // POST: api/AccountModels
    [HttpPost]
    public ActionResult<AccountModel> CreateAccountModel(AccountModel AccountModel)
    {
      _accountService.Create(AccountModel);
      return CreatedAtAction(nameof(GetAccountModel), new { id = AccountModel.Id }, AccountModel);
    }

    // PUT: api/AccountModels/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateAccountModel(string id, AccountModel AccountModelIn)
    {
      var AccountModel = _accountService.Get(id);
      if (AccountModel == null)
      {
        return NotFound();
      }
      _accountService.Update(id, AccountModelIn);
      return NoContent();
    }

    // DELETE: api/AccountModels/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteAccountModel(string id)
    {
      var AccountModel = _accountService.Get(id);
      if (AccountModel == null)
      {
        return NotFound();
      }
      _accountService.Remove(id);
      return NoContent();
    }

  }
}
