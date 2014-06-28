namespace RedLions.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IAnnouncementRepository
    {
        Announcement GetByID(int id);
        IEnumerable<Announcement> GetPagedListOrderedByRecent(
            int pageIndex,
            int pageSize,
            out int totalCount);

        void Create(Announcement announcement);
        void Update(Announcement announcement);
        void Delete(Announcement announcement);
    }
}
