﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProgrammerGrammar.Example.Core.Abstractions
{
    public interface IExampleDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        IDbContextTransaction BeginTransaction();

        Task LockAsync<T>(CancellationToken cancellationToken = default) where T : class;

        Task LockAsync(Type type, CancellationToken cancellationToken = default);
    }
}