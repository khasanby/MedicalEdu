using MediatR;
using MedicalEdu.Application.Common.Interfaces;
using MedicalEdu.Domain.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MedicalEdu.Application.Common.Behaviors;

public sealed class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IMedicalEduDbContext _dbContext;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(
        IMedicalEduDbContext dbContext,
        ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Only apply transactions to commands (write operations)
        if (request is not ICommand<TResponse>)
        {
            _logger.LogDebug("Request {RequestType} is not a command, skipping transaction", typeof(TRequest).Name);
            return await next();
        }

        _logger.LogInformation("⏱ Starting transaction for {RequestType}", typeof(TRequest).Name);

        // Use EF Core's execution strategy for resiliency against transient faults
        if (_dbContext is DbContext dbContext)
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                return await ExecuteTransactionAsync(next, cancellationToken);
            });
        }
        else
        {
            // Fallback for non-EF Core implementations
            return await ExecuteTransactionAsync(next, cancellationToken);
        }
    }

    private async Task<TResponse> ExecuteTransactionAsync(RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Use interface methods for transaction management
        await _dbContext.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var response = await next();

            // Single SaveChanges call for the entire transaction
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _dbContext.CommitTransactionAsync(cancellationToken);

            _logger.LogInformation("✅ Committed transaction for {RequestType}", typeof(TRequest).Name);

            return response;
        }
        catch (Exception ex)
        {
            await _dbContext.RollbackTransactionAsync(cancellationToken);

            _logger.LogError(ex, "❌ Rolled back transaction for {RequestType}", typeof(TRequest).Name);

            throw;
        }
    }
}