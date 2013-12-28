namespace RedLions.Infrastructure.Repository
{
    using System;
    using System.Data.Entity.Validation;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using RedLions.Business;

    public class InquiryRepository : IInquiryRepository
    {
        private RedLionsContext context;

        public InquiryRepository(RedLionsContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("The parameter 'context' must not be null.");
            }

            this.context = context;
        }

        public void Inquire(Inquiry inquiry)
        {
            if (inquiry == null)
            {
                throw new ArgumentNullException("The parameter 'inquiry' must not be null.");
            }

            this.context.Inquiries.Add(inquiry);
            this.Save();
        }

        public Inquiry GetById(int id)
        {
            return this.context.Inquiries.Find(id);
        }

        public IEnumerable<Inquiry> GetPagedList<TKey>(
            int pageIndex,
            int pageSize,
            out int totalCount,
            Expression<Func<Inquiry, TKey>> order,
            Expression<Func<Inquiry, bool>> filter = null)
        {
            var query = from x in this.context.Inquiries
                        select x;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            totalCount = query.Count();

            // Warning: OrderBy() must be used before Skip();
            query = query.OrderBy(order);

            query = query.Skip(pageSize * pageIndex).Take(pageSize);

            return query.ToList();
        }

        private void Save()
        {
            try
            {
                this.context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }
    }
}
