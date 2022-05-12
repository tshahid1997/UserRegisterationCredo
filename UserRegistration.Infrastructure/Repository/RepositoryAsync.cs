using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserRegistration.Application.Interfaces;
using UserRegistration.Application.Interfaces.Marker;
using UserRegistration.Application.Wrapper;
using UserRegistration.Domain.Common;
using UserRegistration.Infrastructure.Persistance;

public class RepositoryAsync : IRepositoryAsync
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RepositoryAsync(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    #region Get
    //Get by Id, return Domain Entity
    public async Task<T> GetByIdAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : BaseEntity
    {
        IQueryable<T> query = _context.Set<T>();

        var entity = await query.Where(e => e.Id == id).FirstOrDefaultAsync(cancellationToken: cancellationToken);


        if (entity != null)
        {
            return entity;
        }
        else
        {
            throw new Exception("Not Found");
        }

    }

    //Get by Id, return mapped Dtos
    public async Task<TDto> GetByIdAsync<T, TDto>(Guid id, CancellationToken cancellationToken = default)
        where T : BaseEntity
        where TDto : IDto
    {
        IQueryable<T> query = _context.Set<T>();

        var entity = await query.Where(a => a.Id == id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (entity != null)
        {
            var dto = _mapper.Map<TDto>(entity);

            return dto;
        }
        else
        {
            throw new Exception("Not Found");
        }



    }

    //Get all/condition, return non-paginated list of Domain Entities
    public async Task<IEnumerable<T>> GetListAsync<T>(Expression<Func<T, bool>> expression = null) where T : BaseEntity
    {
        var query = _context.Set<T>().AsQueryable();

        if (expression != null)
            query = query.Where(expression);
        return await query.ToListAsync();
    }

    //Get all/condition, return non-paginated list of mapped Dtos
    public async Task<IEnumerable<TDto>> GetListAsync<T, TDto>(Expression<Func<T, bool>> expression = null) where T : BaseEntity
        where TDto : IDto
    {
        var query = _context.Set<T>().AsQueryable();

        if (expression != null)
            query = query.Where(expression);
        var list = await query.ToListAsync();

        var result = _mapper.Map<List<TDto>>(list);

        return result;
    }

    //Check if exists, return true/false
    public async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> expression)
    where T : BaseEntity
    {
        IQueryable<T> query = _context.Set<T>();
        if (expression != null) return await query.AnyAsync(expression);
        return await query.AnyAsync();
    }

    #endregion Get

    #region Create

    //Create, retun Guid
    public async Task<Guid> CreateAsync<T>(T entity)
    where T : BaseEntity
    {
        await _context.Set<T>().AddAsync(entity);
        return entity.Id;
    }

    //Create range, retun List of Guid
    public async Task<IList<Guid>> CreateRangeAsync<T>(IEnumerable<T> entity)
    where T : BaseEntity
    {
        await _context.Set<T>().AddRangeAsync(entity);
        return entity.Select(x => x.Id).ToList();
    }
    #endregion Create

    #region Update

    //Update
    public Task UpdateAsync<T>(T entity) where T : BaseEntity
    {
        if (_context.Entry(entity).State == EntityState.Unchanged)
        {
            throw new Exception("Nothing to update");
        }

        T exist = _context.Set<T>().Find(entity.Id);
        if (exist != null)
        {
            _context.Entry(exist).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }
        else
        {
            throw new Exception("Not Found");
        }

    }
    #endregion Update

    #region Remove
    //Remove
    public Task RemoveAsync<T>(T entity) where T : BaseEntity
    {
        _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    //Remove by Id
    public async Task<T> RemoveByIdAsync<T>(Guid entityId)
    where T : BaseEntity
    {
        var entity = await _context.Set<T>().FindAsync(entityId);
        if (entity == null)
            throw new Exception("Not Found");

        _context.Set<T>().Remove(entity);

        return entity;
    }
    #endregion Remove

    #region Pagination
    //Get all/condition, return paginated list of Domain Entities
    public async Task<PaginatedResponse<T>> GetPaginatedResultsAsync<T>(int pageNumber, int pageSize = int.MaxValue, Expression<Func<T, bool>> expression = null, string[] orderBy = null) where T : BaseEntity
    {
        IQueryable<T> query = _context.Set<T>();
        if (expression != null)
            query = query.Where(expression);


        var filteredList = await query.ToListAsync();
        var totalCount = filteredList.Count();

        var pagedData = filteredList
         .Skip(((pageNumber - 1) * pageSize))
          .Take(pageSize).ToList();


        var result = pagedData;

        return PaginatedResponse<T>.Success(result, totalCount, pageNumber, pageSize);
    }

    //Get all/condition, return paginated list of mapped Dtos
    public async Task<PaginatedResponse<TDto>> GetPaginatedResultsAsync<T, TDto>(int pageNumber, int pageSize = int.MaxValue, Expression<Func<T, bool>> expression = null, string[] orderBy = null)
        where T : BaseEntity
        where TDto : IDto
    {
        IQueryable<T> query = _context.Set<T>();
        if (expression != null)
            query = query.Where(expression);


        var filteredList = await query.ToListAsync();
        var totalCount = filteredList.Count();

        var pagedData = filteredList
         .Skip(((pageNumber - 1) * pageSize))
          .Take(pageSize).ToList();


        var result = _mapper.Map<List<TDto>>(pagedData);

        return PaginatedResponse<TDto>.Success(result, totalCount, pageNumber, pageSize);
    }

    #endregion Pagination

    #region Save

    //Save the Changes to Database!
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    #endregion Save
}