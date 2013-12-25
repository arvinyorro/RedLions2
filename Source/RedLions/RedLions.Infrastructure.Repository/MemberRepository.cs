namespace RedLions.Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;
    using RedLions.Business;

    /// <summary>
    /// This class implements the <see cref="RedLions.Business.IMemberRepository"/> interface.
    /// </summary>
    public class MemberRepository : IMemberRepository
    {
        private RedLionsContext context;

        public MemberRepository(RedLionsContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("The parameter 'context' must not be null");
            }

            this.context = context;
        }

        public void RegisterMember(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException("The parameter 'member' must not be null");
            }

            this.context.Users.Add(member);
            this.Save();            
        }

        public void Update(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException("The parameter 'member' must not be null");
            }

            this.context.Entry<Member>(member).State = System.Data.EntityState.Modified;
            this.Save();
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return this.context.Users.OfType<Member>().ToList();
        }

        public Member GetMemberByID(int userID)
        {
            return this.context.Users.OfType<Member>()
                .FirstOrDefault(x => x.ID == userID);
        }

        public Member GetMemberByUsername(string username)
        {
            return this.context.Users.OfType<Member>()
                .FirstOrDefault(x => x.Username == username);
        }

        public Member GetMemberByReferralCode(string referralCode)
        {
            return this.context.Users.OfType<Member>()
                .FirstOrDefault(x => x.ReferralCode == referralCode);
        }

        public Member GetRandomMember()
        {
            int maxUserCount = this.context.Users.OfType<Member>().Count();
            int randomCount = new Random().Next(maxUserCount);

            return this.context.Users.OfType<Member>()
                .OrderBy(x => x.ID).Skip(randomCount).First();
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
