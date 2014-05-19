namespace RedLions.CrossCutting
{
    using System;

    public interface IUnitOfWork
    {
        void Commit();
    }
}
