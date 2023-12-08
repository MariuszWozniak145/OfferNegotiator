namespace OfferNegotiatorDal.Repositories.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}

