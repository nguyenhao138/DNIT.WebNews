namespace DNIT.WebNews.Admin.Models
{
  public class ErrorViewModel
  {
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
  }
}
