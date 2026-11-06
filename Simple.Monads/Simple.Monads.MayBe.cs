using System.Runtime.ExceptionServices;

namespace Simple.Monads;

/// <summary>
/// Represents a result of an execution that may fail with an exception 
/// </summary>
public sealed class MayBe {
  /// <summary>
  /// Dispatch info to preserve original stack trace when throwing exception
  /// </summary>
  private readonly ExceptionDispatchInfo? _dispatchInfo;

  /// <summary>
  /// Successful execution instance
  /// </summary>
  public static MayBe Success { get; } = new(null);

  /// <summary>
  /// Failed execution instance
  /// </summary>
  public Exception? Exception { get; }

  /// <summary>
  /// True if instance stands for a successful execution
  /// </summary>
  public bool IsSuccessful => Exception is null;

  /// <summary>
  /// Create a new instance of MayBe
  /// </summary>
  /// <param name="exception">Exception, reason for failure</param>
  public MayBe(Exception? exception) {
    Exception = exception;

    if (Exception is not null)
      _dispatchInfo = ExceptionDispatchInfo.Capture(Exception);
  }

  /// <summary>
  /// Creates a new instance of MayBe from an exception
  /// </summary>
  /// <param name="exception">Exception, reason for failure</param>
  /// <returns>Failed instance</returns>
  public static MayBe FromException(Exception exception) => new(exception);

  /// <summary>
  /// Throws the exception if instance stands for a failed execution, otherwise does nothing
  /// </summary>
  public void ThrowIfFailed() => _dispatchInfo?.Throw();

  /// <summary>
  /// Converts a MayBe instance to a boolean value. Returns true if the instance is not null and does not contain an exception, otherwise returns false.
  /// </summary>
  /// <param name="instance">The MayBe instance to convert to a boolean value.</param>
  /// <returns>true if the instance is not null and does not contain an exception, otherwise false</returns>
  public static implicit operator bool(MayBe? instance) => instance is not null && instance.Exception is null;
}

