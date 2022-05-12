using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserRegistration.Application.Interfaces.Marker;
using UserRegistration.Application.Wrapper;
using UserRegistration.Domain;
using UserRegistration.Domain.Common;

namespace UserRegistration.Application.Interfaces
{
    public interface IRepositoryAsync : ITransientService
    {
        Task<T> GetByIdAsync<T>(Guid id, CancellationToken cancellationToken = default)
        where T : BaseEntity;

        Task<TDto> GetByIdAsync<T, TDto>(Guid id, CancellationToken cancellationToken = default)
        where T : BaseEntity
        where TDto : IDto;


        Task<IEnumerable<T>> GetListAsync<T>(Expression<Func<T, bool>> expression = null)
        where T : BaseEntity;

        Task<IEnumerable<TDto>> GetListAsync<T, TDto>(Expression<Func<T, bool>> expression = null)
            where T : BaseEntity
            where TDto : IDto;

        Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> expression)
        where T : BaseEntity;


        Task<Guid> CreateAsync<T>(T entity)
        where T : BaseEntity;

        Task<IList<Guid>> CreateRangeAsync<T>(IEnumerable<T> entity)
        where T : BaseEntity;

        Task UpdateAsync<T>(T entity)
        where T : BaseEntity;

        Task RemoveAsync<T>(T entity)
        where T : BaseEntity;

        Task<T> RemoveByIdAsync<T>(Guid entityId)
        where T : BaseEntity;

        Task<PaginatedResponse<T>> GetPaginatedResultsAsync<T>(int pageNumber, int pageSize = int.MaxValue, Expression<Func<T, bool>> expression = null, string[] orderBy = null)
        where T : BaseEntity;

        Task<PaginatedResponse<TDto>> GetPaginatedResultsAsync<T, TDto>(int pageNumber, int pageSize = int.MaxValue, Expression<Func<T, bool>> expression = null, string[] orderBy = null)
        where T : BaseEntity
        where TDto : IDto;


        Task<int> SaveChangesAsync();

    }
}
