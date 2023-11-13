using System.Text;

namespace Granitos.Common.Errors.Core.Extensions;

public static class ExceptionExtensions
{
    /// <summary>
    /// Recursively gets the message of an exception and its inner exceptions, up to a specified number of inner exceptions.
    /// </summary>
    /// <param name="ex">The exception to get the message for.</param>
    /// <param name="maxInnerExceptions">The maximum number of inner exceptions to traverse. The default is 3.</param>
    /// <returns>A concatenated message string containing the messages of the exception and its inner exceptions.</returns>
    public static string GetMessages(this Exception? ex, int maxInnerExceptions = 3)
    {
        var sb = new StringBuilder();
        var innerExceptionCount = 0;

        while (ex is not null && innerExceptionCount < maxInnerExceptions)
        {
            sb.AppendLine(ex.Message);
            ex = ex.InnerException!;
            innerExceptionCount++;
        }

        return sb.ToString();
    }
}