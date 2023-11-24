namespace Granitos.Common.Errors.Core;

/// <summary>
///     A machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807.
/// </summary>
public class ProblemDetails
{
    /// <summary>
    ///     A URI reference [RFC3986] that identifies the problem type.<br />
    ///     This specification encourages that, when dereferenced, it provides human-readable
    ///     documentation for the problem type (e.g., using HTML [W3C.REC-html5-20141028]).<br />
    ///     When this member is not present, its value is assumed to be "about:blank".
    /// </summary>
    public string Type { get; set; } = default!;

    /// <summary>
    ///     The HTTP status code ([RFC7231], Section 6) generated by the origin server for
    ///     this occurrence of the problem.
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    ///     A short, human-readable summary of the problem type.<br />
    ///     It SHOULD NOT change from occurrence to occurrence of the problem, except for purposes
    ///     of localization (e.g., using proactive content negotiation; see[RFC7231], Section 3.4).
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    ///     A human-readable explanation specific to this occurrence of the problem.
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    ///     A URI reference that identifies the specific occurrence of the problem.<br />
    ///     It may or may not yield further information if dereferenced.
    /// </summary>
    public string? Instance { get; set; }

    /// <summary>
    ///     Problem type definitions MAY extend the problem details object with additional members.<br />
    ///     Clients consuming problem details MUST ignore any such extensions that they don't recognize;
    ///     this allows problem types to evolve and include additional information in the future.
    /// </summary>
    public Dictionary<string, object?> Extensions { get; } = new();
}