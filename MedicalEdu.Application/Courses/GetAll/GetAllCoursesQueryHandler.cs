using MedicalEdu.Application.Models.Courses;
using MedicalEdu.Domain.DataAccess;
using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicalEdu.Application.Courses.GetAll;

internal sealed class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, GetAllCoursesResponse>
{
    private readonly IMedicalEduDbContext _dbContext;

    public GetAllCoursesQueryHandler(IMedicalEduDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetAllCoursesResponse> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Course> coursesQuery = _dbContext.Courses.AsNoTracking();

        // Apply filtering
        coursesQuery = ApplyFilters(coursesQuery, request);

        // Get total count before pagination
        var totalCount = await coursesQuery.CountAsync(cancellationToken);

        // Apply sorting
        coursesQuery = ApplySorting(coursesQuery, request);

        // Apply pagination
        coursesQuery = coursesQuery
            .Skip(request.Page * request.PageSize)
            .Take(request.PageSize);

        // Project to DTOs
        var courses = await coursesQuery
            .Select(course => new CourseResponse
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                ShortDescription = course.ShortDescription,
                Price = course.Price != null ? course.Price.Amount : 0,
                Currency = course.Price != null ? course.Price.Currency.Code : "USD",
                ThumbnailUrl = course.ThumbnailUrl != null ? course.ThumbnailUrl.Value : null,
                VideoIntroUrl = course.VideoIntroUrl != null ? course.VideoIntroUrl.Value : null,
                DurationMinutes = course.DurationMinutes,
                MaxStudents = course.MaxStudents,
                Category = course.Category,
                Tags = course.Tags,
                IsPublished = course.IsPublished,
                IsActive = course.IsActive,
                InstructorId = course.InstructorId,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt,
                PublishedAt = course.PublishedAt
            })
            .ToArrayAsync(cancellationToken);

        return new GetAllCoursesResponse
        {
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
            Courses = courses
        };
    }

    private static IQueryable<Course> ApplyFilters(IQueryable<Course> query, GetAllCoursesQuery request)
    {
        if (request.IsPublished.HasValue)
            query = query.Where(c => c.IsPublished == request.IsPublished.Value);

        if (request.IsActive.HasValue)
            query = query.Where(c => c.IsActive == request.IsActive.Value);

        if (request.InstructorId.HasValue)
            query = query.Where(c => c.InstructorId == request.InstructorId.Value);

        if (!string.IsNullOrEmpty(request.Title))
            query = query.Where(c => c.Title.Contains(request.Title));

        if (!string.IsNullOrEmpty(request.Description))
            query = query.Where(c => c.Description.Contains(request.Description));

        if (!string.IsNullOrEmpty(request.Category))
            query = query.Where(c => c.Category == request.Category);

        if (!string.IsNullOrEmpty(request.Tags))
            query = query.Where(c => c.Tags.Any(tag => tag.Contains(request.Tags)));

        if (request.MinPrice.HasValue)
            query = query.Where(c => c.Price != null && c.Price.Amount >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            query = query.Where(c => c.Price != null && c.Price.Amount <= request.MaxPrice.Value);

        if (!string.IsNullOrEmpty(request.Currency))
            query = query.Where(c => c.Price != null && c.Price.Currency.Code == request.Currency);

        if (request.CreatedFrom.HasValue)
            query = query.Where(c => c.CreatedAt >= request.CreatedFrom.Value);

        if (request.CreatedTo.HasValue)
            query = query.Where(c => c.CreatedAt <= request.CreatedTo.Value);

        if (request.PublishedFrom.HasValue)
            query = query.Where(c => c.PublishedAt >= request.PublishedFrom.Value);

        if (request.PublishedTo.HasValue)
            query = query.Where(c => c.PublishedAt <= request.PublishedTo.Value);

        if (request.UpdatedFrom.HasValue)
            query = query.Where(c => c.UpdatedAt >= request.UpdatedFrom.Value);

        if (request.UpdatedTo.HasValue)
            query = query.Where(c => c.UpdatedAt <= request.UpdatedTo.Value);

        if (request.MinDurationMinutes.HasValue)
            query = query.Where(c => c.DurationMinutes >= request.MinDurationMinutes.Value);

        if (request.MaxDurationMinutes.HasValue)
            query = query.Where(c => c.DurationMinutes <= request.MaxDurationMinutes.Value);

        if (request.MinMaxStudents.HasValue)
            query = query.Where(c => c.MaxStudents >= request.MinMaxStudents.Value);

        if (request.MaxMaxStudents.HasValue)
            query = query.Where(c => c.MaxStudents <= request.MaxMaxStudents.Value);

        return query;
    }

    private static IQueryable<Course> ApplySorting(IQueryable<Course> query, GetAllCoursesQuery request)
    {
        // Apply sorting based on request parameters
        if (request.TitleSortDirection.HasValue)
        {
            query = request.TitleSortDirection == SortDirection.Ascending
                ? query.OrderBy(c => c.Title)
                : query.OrderByDescending(c => c.Title);
        }

        if (request.PriceSortDirection.HasValue)
        {
            query = request.PriceSortDirection == SortDirection.Ascending
                ? query.OrderBy(c => c.Price.Amount)
                : query.OrderByDescending(c => c.Price.Amount);
        }

        if (request.CreatedAtSortDirection.HasValue)
        {
            query = request.CreatedAtSortDirection == SortDirection.Ascending
                ? query.OrderBy(c => c.CreatedAt)
                : query.OrderByDescending(c => c.CreatedAt);
        }

        if (request.PublishedAtSortDirection.HasValue)
        {
            query = request.PublishedAtSortDirection == SortDirection.Ascending
                ? query.OrderBy(c => c.PublishedAt)
                : query.OrderByDescending(c => c.PublishedAt);
        }

        if (request.UpdatedAtSortDirection.HasValue)
        {
            query = request.UpdatedAtSortDirection == SortDirection.Ascending
                ? query.OrderBy(c => c.UpdatedAt)
                : query.OrderByDescending(c => c.UpdatedAt);
        }

        if (request.DurationMinutesSortDirection.HasValue)
        {
            query = request.DurationMinutesSortDirection == SortDirection.Ascending
                ? query.OrderBy(c => c.DurationMinutes)
                : query.OrderByDescending(c => c.DurationMinutes);
        }

        // Default sorting if no specific sorting is requested
        if (!request.TitleSortDirection.HasValue && 
            !request.PriceSortDirection.HasValue && 
            !request.CreatedAtSortDirection.HasValue && 
            !request.PublishedAtSortDirection.HasValue && 
            !request.UpdatedAtSortDirection.HasValue && 
            !request.DurationMinutesSortDirection.HasValue)
        {
            query = query.OrderByDescending(c => c.CreatedAt);
        }

        return query;
    }
} 