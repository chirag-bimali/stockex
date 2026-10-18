namespace SE.Application.Common.Response;

public abstract record Result<T>
{
  private Result() { }

  public sealed record Ok(T Value) : Result<T>;
  public sealed record Err(Error Error) : Result<T>;

  public bool IsSuccess => this is Ok;

  public TR Match<TR>(Func<T, TR> ok, Func<Err, TR> err) => this switch
  {
    Ok o => ok(o.Value),
    Err e => err(e),
    _ => throw new InvalidOperationException()
  };
  public void Switch(Action<T> ok, Action<Err> err)
  {
    switch (this)
    {
      case Ok o: ok(o.Value); break;
      case Err e: err(e); break;
    }
  }

  public static implicit operator Result<T>(T value) => new Ok(value);
  public static implicit operator Result<T>(Error error) => new Err(error);
}