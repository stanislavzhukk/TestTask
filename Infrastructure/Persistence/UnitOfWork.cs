using Application.Exceptions;
using Application.Interfaces;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using System;
using System.Data;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                // Serializable isolation protects against a race condition when two bookings
                // for the same hall with overlapping time ranges are created concurrently
                // under ReadCommitted (the default), two parallel requests might not see
                // each other's changes and both could pass the overlap check.
                using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
                try
                {
                    await action();
                    await transaction.CommitAsync();
                }
                catch (Exception ex) when (IsSerializationFailure(ex))
                {
                    await TryRollbackAsync(transaction);
                    throw new ConflictException("The booking could not be completed due to a conflicting concurrent request. Please try again.");
                }
                catch
                {
                    await TryRollbackAsync(transaction);
                    throw;
                }
            });
        }


        private static async Task TryRollbackAsync(IDbContextTransaction transaction)
        {
            try
            {
                await transaction.RollbackAsync();
            }
            catch (InvalidOperationException)
            {
                // Postgres already aborted the transaction after a failed commit
                // (e.g. serialization failure) — nothing left to roll back.
            }
        }

        private static bool IsSerializationFailure(Exception ex)
        {
            // DbUpdateException wraps the Npgsql exception; PostgresException may also
            // surface directly depending on where the failure occurs in the transaction.
            return ex is PostgresException { SqlState: "40001" }
                || ex.InnerException is PostgresException { SqlState: "40001" };
        }
    }
}