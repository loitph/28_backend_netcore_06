public class ResonseTypeDTO<T>
{
  public int StatusCode { get; set; }
  public string? Message { get; set; }
  public T? Content { get; set; }

  public DateTime DateTime { get; set; }
}