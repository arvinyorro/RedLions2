namespace RedLions.Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using RedLions.Business;
    using RedLions.CrossCutting;

    public class AnnouncementRepository : GenericRepository, IAnnouncementRepository
    {
        public AnnouncementRepository(IDbContext dbContext)
            : base(dbContext) { }
                
        public Announcement GetByID(int id)
        {
            return base.GetById<Announcement>(id);
        }

        public IEnumerable<Announcement> GetPagedListOrderedByRecent(
            int pageIndex, 
            int pageSize, 
            out int totalCount)
        {
            var query = from x in base.GetQueryableAll<Announcement>()
                        select x;

            totalCount = query.Count();

            var orderedQuery = query.OrderByDescending(x => x.PostedDateTime);

            IEnumerable<Announcement> announcements = base.GetPagedList<Announcement>(orderedQuery, pageIndex, pageSize);

            return announcements;
        }

        public void Create(Announcement announcement)
        {
            base.Create<Announcement>(announcement);
        }

        public void Update(Announcement announcement)
        {
            base.Update<Announcement>(announcement);
        }

        public void Delete(Announcement announcement)
        {
            base.Delete<Announcement>(announcement);
        }
    }
}
