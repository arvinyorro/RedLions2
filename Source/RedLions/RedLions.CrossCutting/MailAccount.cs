namespace RedLions.CrossCutting
{
    public interface IMailAccount
    {
        string DisplayName { get; }
        string Address { get; }
    }

    public class MailAccount : IMailAccount
    {
        public MailAccount(string displayName, string address)
        {
            this.DisplayName = displayName;
            this.Address = address;
        }

        public string DisplayName { get; private set; }
        public string Address { get; private set; }
    }
}
