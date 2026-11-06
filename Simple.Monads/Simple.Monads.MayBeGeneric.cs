using System.Runtime.ExceptionServices;

namespace Simple.Monads;

/// <summary>
/// Represents a result of an execution that may fail with an exception
/// </summary>
/// <typeparam name="T">Type of the result value</typeparam> 
public sealed class MayBe<T> {
  /// <summary>
  /// Dispatch info to preserve original stack trace when throwing exception
  /// </summary>
  private readonly ExceptionDispatchInfo? _dispatchInfo;

  /// <summary>
  /// Gets the value of the result if the execution was successful, otherwise throws the exception
  /// </summary>
  public T? Value {
    get {
      if (IsSuccessful)
        return field;
      else {
        _dispatchInfo?.Throw();

        return default;
      }
    }
  }

  /// <summary>
  /// Gets the exception if the execution failed, otherwise returns null
  /// </summary>
  public Exception? Exception { get; }

  /// <summary>
  /// True if instance stands for a successful execution
  /// </summary>
  public bool IsSuccessful => Exception is null;

  /// <summary>
  /// Creates a new instance of MayBe with a value
  /// </summary>
  /// <param name="value">The value of the result</param>
  public MayBe(T? value) {
    Value = value;
  }

  /// <summary>
  /// Creates a new instance of MayBe with an exception
  /// </summary>
  /// <param name="exception">The exception that occurred</param>
  public MayBe(Exception exception) {
    ArgumentNullException.ThrowIfNull(exception);

    Exception = exception;

    _dispatchInfo = ExceptionDispatchInfo.Capture(exception);
  }

  /// <summary>
  /// Creates a new instance of MayBe from an exception
  /// </summary>
  /// <param name="exception">The exception that occurred</param>
  /// <returns>A new instance of MayBe representing the failure</returns>
  public static MayBe<T> FromException(Exception exception) => new(exception);

  /// <summary>
  /// Creates a new instance of MayBe from a value
  /// </summary>
  /// <param name="value">The value of the result</param>
  /// <returns>A new instance of MayBe representing the success</returns>
  public static MayBe<T> FromValue(T? value) => new(value);

  /// <summary>
  /// Throws the exception if instance stands for a failed execution, otherwise does nothing
  /// </summary>
  public void ThrowIfFailed() => _dispatchInfo?.Throw();

  /// <summary>
  /// Converts a MayBe instance to its underlying value. If the instance is null or represents a failure, returns the default value of T.
  /// </summary>
  /// <param name="instance">The MayBe instance to convert</param>
  /// <returns>The underlying value, or the default value of T if the instance is null or represents a failure</returns>
  public static implicit operator T?(MayBe<T>? instance) {
    if (instance is null)
      return default;

    return instance.Value;
  }

  /// <summary>
  /// Converts a value of type T to a MayBe instance. If the value is null, returns a MayBe instance representing a failure with a NullReferenceException.
  /// </summary>
  /// <param name="value">The value to convert</param>
  /// <returns>A new instance of MayBe representing the success or failure</returns>
  public static implicit operator MayBe<T>(T? value) => new(value);
}
