using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.Data.Model;
using GetWell.Service.Tests.Extensions.Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;

namespace GetWell.Service.Tests
{
    public class RepositoryTest<T> : IRepository<T> where T : class, IBaseModel, new()
    {
        public readonly GetWellContext Context;
        private const string ID = nameof(IBaseModel.ID);

        public RepositoryTest(IEnumerable<T> fakeData = null)
        {
            FakeData = fakeData ?? GetFakeData();
            Context = CreateDbContext().Object;
        }

        public DbSet<T> Entity => Context.Set<T>();

        public IEnumerable<T> FakeData { get; private set; }

        GetWellContext IRepository<T>.Context => Context; 

        #region Not Used

        public List<T> GetAll(int pageNumber, int pageSize, string orderByString, out int count, Expression<Func<T, bool>> predicate = null,
            string[] includeTables = null)
        {
            var result =  FakeData.ToList();
            count = FakeData.Count();

            return result;
        }

        public async Task<List<T>> GetAllAsync(Pagination<T> pagination, Expression<Func<T, bool>> predicate = null, string[] includeTables = null)
        {
            var result =  FakeData.ToList();

            return result;
        }

        public Task<Tuple<List<T>, int>> GetAllAsync(int pageNumber, int pageSize, string orderByString, Expression<Func<T, bool>> predicate = null,
            string[] includeTables = null)
        {
            var result =  FakeData.ToList();
            int count = FakeData.Count();

            return Task.FromResult(new Tuple<List<T>, int>(result, count));
        }
        
        public Task<T> GetSingleByPredicateAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<CrudResponse> SaveAsync(T item, Expression<Func<T, bool>> findSinglePredicate)
        {
            throw new NotImplementedException();
        }

        #endregion

        public async Task<T> GetAsync(int id)
        {
            return await Entity.SingleOrDefaultAsync(c => c.ID == id);
        }

        public async Task<CrudResponse> SaveAsync(T item)
        {
            var list = FakeData.ToList();
            list.Add(item);
            FakeData = list;
            var result = FakeData.Any(c => c == item) ? Crud.Success : Crud.Error;

            return await Task.FromResult(Result(result));
        }

        public async Task<CrudResponse> SaveAllAsync(List<T> list)
        {
            throw new NotImplementedException();
        }

        public async Task<CrudResponse> UpdateAsync(T item)
        {
            if (item.ID <= 0)
                return Result(Crud.Error);

            var t = Entity;

            var query = await t.SingleOrDefaultAsync(c => c.ID == item.ID);

            if (query != null)
            {
                Context.Entry(query).CurrentValues.SetValues(item);
                await Context.SaveChangesAsync();

                item.ID = query.ID;

                return Result(Crud.Success);
            }

            return Result(Crud.ItemNotFoundError);
        }

        public Task<CrudResponse> SaveOrUpdateAsync(T item)
        {
            throw new NotImplementedException();
        }

        public Task<CrudResponse> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        private Mock<GetWellContext> CreateDbContext()
        {
            var items = FakeData.AsQueryable().BuildMock();
            var dbSet = FakeData.AsQueryable().BuildMockDbSet();
            //dbSet.Object.AddRange(FakeData);
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(items.Object.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(items.Object.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(items.Object.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(items.Object.GetEnumerator());
            //dbSet.As<IQueryable<T>>().Setup(m => m.First()).Returns(items.Object.First);
            //dbSet.As<IQueryable<T>>().Setup(m => m.FirstOrDefault()).Returns(items.Object.FirstOrDefault);
            //dbSet.As<IQueryable<T>>().Setup(m => m.SingleOrDefault()).Returns(items.Object.SingleOrDefault);
            
            

            var mockContext = new Mock<GetWellContext>();
            //mockContext.Setup(c => c.SetModified(It.IsAny<IBaseModel>()));
            mockContext.Setup(c => c.Set<T>()).Returns(dbSet.Object);


            return mockContext;
        }
        
        private static CrudResponse Result(Crud crud)
        {
            return new(crud);
        }

        public static IEnumerable<T> GetFakeData()
        {
            var type = typeof(T);
            var result = new List<T>();
            
            if (type == typeof(City))
            {
                result.AddRange((IEnumerable<T>)(new List<City>
                {
                    new() {CountryID = 1, Name = "Samarkand"},
                    new() {CountryID = 1, Name = "Tashkent"},
                    new() {CountryID = 1, Name = "Navoi"}
                }));
            }
            else if (type == typeof(Clinic))
            {
                result.AddRange((IEnumerable<T>)(new List<Clinic>()
                {
                    new() {ID = 1, Name = "Clinic 1", Description = "Description 1", Website = "website1.com", UniqueKey = Guid.Parse("bb2e4f81-1db8-45c6-892b-77a0aada1818")},
                    new() {ID = 1, Name = "Clinic 2", Description = "Description 2", Website = "website2.com"},
                    new() {ID = 1, Name = "Clinic 3", Description = "Description 3", Website = "website3.com"},
                    new() {ID = 1, Name = "Clinic 4", Description = "Description 4", Website = "website4.com"},
                    new() {ID = 1, Name = "Clinic 5", Description = "Description 5", Website = "website5.com"},
                    new() {ID = 1, Name = "Clinic 6", Description = "Description 6", Website = "website6.com"},
                    new() {ID = 1, Name = "Clinic 7", Description = "Description 7", Website = "website7.com"},
                    new() {ID = 1, Name = "Clinic 8", Description = "Description 8", Website = "website8.com"},
                    new() {ID = 1, Name = "Clinic 9", Description = "Description 9", Website = "website9.com"},
                    new() {ID = 1, Name = "Clinic 10", Description = "Description 10", Website = "website10.com"},
                    new() {ID = 1, Name = "Clinic 11", Description = "Description 11", Website = "website11.com"}
                }));
            }
            else
            {
                result.AddRange(new List<T>());
            }

            var idCounter = 1;
            result.ForEach(x => x.ID = idCounter++);
            
            return result.Select(_ => _);
        }
    }

    public static class MockDbSetExtensions
    {
        public static Mock<DbSet<T>> AsDbSetMock<T>(this IEnumerable<T> list) where T : class
        {
            IQueryable<T> queryableList = list.AsQueryable();
            Mock<DbSet<T>> dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Provider).Returns(queryableList.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Expression).Returns(queryableList.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(queryableList.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(() => queryableList.GetEnumerator());
            return dbSetMock;
        }
    }
}