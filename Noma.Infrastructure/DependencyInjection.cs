using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Noma.ApplicationL.Common.Repositories;
using Noma.Infrastructure.Persistence;
using Noma.Infrastructure.Persistence.Repositories.CategoryRepositories;
using Noma.Infrastructure.Persistence.Repositories.NoteRepositories;
using Noma.ApplicationL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Noma.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("DataSource=NomaDB.db"));
            services.AddSingleton<INoteRepository, SQLiteNoteRepository>();
            services.AddSingleton<ICategoryRepository, SQLiteCategoryRepository>();
            services.AddSingleton<IRequestsReceiver, LocalHttpRequestsReceiver>();

            return services;
        }
    }
}
