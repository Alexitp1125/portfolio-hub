namespace PortfolioHub.Components.Features.Catalog;

/// <summary>
/// Deployment state of a project, shown as a badge on its card.
/// Kept honest: nothing is "Live" unless it is actually deployed.
/// </summary>
public enum ProjectStatus
{
    /// <summary>Deployed and reachable.</summary>
    Live,

    /// <summary>Planned or in progress, not yet deployed.</summary>
    Soon
}

/// <summary>
/// A single catalog entry rendered by <c>ProjectCard</c>. Data lives in memory
/// (hardcoded) until the hub database exists. <see cref="ImageUrl"/> is optional:
/// when null the card renders a compact text-only layout.
/// </summary>
public sealed class Project
{
    public required string Title { get; init; }

    public required string Tagline { get; init; }

    public string[] Tags { get; init; } = [];

    public ProjectStatus Status { get; init; } = ProjectStatus.Soon;

    /// <summary>Optional screenshot/thumbnail. Null = text-only card.</summary>
    public string? ImageUrl { get; init; }

    public string? DemoUrl { get; init; }

    public string? GitHubUrl { get; init; }

    /// <summary>Link to the project's detail page (e.g. /apps/{slug}).</summary>
    public string? DetailUrl { get; init; }
}
