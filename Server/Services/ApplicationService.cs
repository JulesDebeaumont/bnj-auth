namespace Server.Services;

public abstract class ApplicationService
{
  public class ResponseService
  {
    public bool IsSuccess {get; set;} = false;
    public List<string> Errors { get; set; } = [];

    public void AddError(string error)
    {
      this.Errors.Add(error);
    }

    public void SetSuccessTrue()
    {
      this.IsSuccess = true;
    }

  }
}
