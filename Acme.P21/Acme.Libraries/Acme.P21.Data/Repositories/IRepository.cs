namespace Acme.P21.Data.Repositories
{
    public interface IRepository
    {
        T Get<T>(int id) where T : class;
    }
}