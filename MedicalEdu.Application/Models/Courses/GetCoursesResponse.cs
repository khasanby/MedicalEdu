namespace MedicalEdu.Application.Models.Courses;


public sealed class GetCoursesResponse<T>
{
    /// <summary>
    /// Gets the total number of courses matching the filter criteria.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets the current page number (0-based).
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets the page size used for pagination.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Gets whether there are more pages available.
    /// </summary>
    public bool HasNextPage => Page < TotalPages - 1;

    /// <summary>
    /// Gets whether there are previous pages available.
    /// </summary>
    public bool HasPreviousPage => Page > 0;

    /// <summary>
    /// Gets the courses for the current page.
    /// </summary>
    public T[] Courses { get; set; } = Array.Empty<T>();

    public GetCoursesResponse(int totalCount, int page, int pageSize, T[] courses)
    {
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
        Courses = courses;
    }
}