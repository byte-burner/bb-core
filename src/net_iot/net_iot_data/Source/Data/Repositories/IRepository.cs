namespace net_iot_data.Data.Repositories
{
    public interface IRepository<T>
    {
        Task<T> Create(T command);

        Task<T> Delete(T command);

        Task<List<T>> GetAll();

        Task<T?> Get(T command);

        Task<T?> Update(T command);
    } 
}