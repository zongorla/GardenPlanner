using GardenPlannerApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenPlannerApp
{
    public static class QueryExtentions
    {

        public static IQueryable<T> OwnedOrPublic<T> (this IQueryable<T> query, string userId) where T : BaseEntity
        {
            return query.Where(e => e.Public || e.Owner.Id == userId).Include(x => x.Owner);
        }
    }
}
