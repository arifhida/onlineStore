using OnlineStore.Model;
using OnlineStore.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineStore.Data.Abstract
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        Task<IEnumerable<T>> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetAll();
        Task<int> Count();
        T GetSingle(long id);
        T GetSingle(Expression<Func<T, bool>> predicate);
        T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetSingleAsync(long id);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindQueryBy(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindByAsyncIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        void Add(T entity);
        void Update(T entity);
        void Update(T entity, string excludeProperties = "");
        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
        Task Commit();
        void Save();
    }

    public interface IUserRepository : IEntityBaseRepository<User> { }
    public interface IRoleRepository : IEntityBaseRepository<Role> { }
    public interface IUserInRoleRepository : IEntityBaseRepository<UserInRole> { }    
    public interface ICategoryRepository : IEntityBaseRepository<Category> { }
    public interface IProductRepository : IEntityBaseRepository<Product> { }
    public interface IProductImageRepository : IEntityBaseRepository<ProductImage> { }
    public interface IBrandRepository : IEntityBaseRepository<Brand> { }
    public interface ICustomerRepository: IEntityBaseRepository<Customer> { }
    public interface IStoreRepository: IEntityBaseRepository<Store> { }
    public interface IOrderRepository: IEntityBaseRepository<Order> { }
    public interface IOrderDetailRepository : IEntityBaseRepository<OrderDetail> { }

}
