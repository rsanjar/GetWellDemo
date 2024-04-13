using System;
using System.Collections.Generic;
using GetWell.Data.Model;
using GetWell.Service.Services;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace GetWell.Service.Tests
{
    public class UnitTest1
    {
        [Fact]  
        public async void GetByIdAsync_Returns_Product()  
        {  
            //Setup DbContext and DbSet mock  
            var dbContextMock = new Mock<GetWellContext>();  
            var dbSetMock = new Mock<DbSet<Clinic>>();  
            dbSetMock.Setup(s => s.FindAsync(It.IsAny<int>())).ReturnsAsync(new Clinic(){ ID = 1, Name = "My clinic 1"});  
            dbContextMock.Setup(s => s.Set<Clinic>()).Returns(dbSetMock.Object);  
            
            //Execute method of SUT (ProductsRepository)  
            var productRepository = new ClinicService(new RepositoryTest<Clinic>());  
            DTO.Clinic product = Task.Run(() => productRepository.GetAsync(1)).Result;

            var list = Task.Run(() => productRepository.GetAllAsync(new PaginatedList<DTO.Clinic>())).Result;

            //Assert  
            Assert.NotNull(product);  
            //Assert.IsAssignableFrom<DTO.Clinic>(product);  
        }

        [Fact]
        public async Task Test1()
        {
            var repo = new RepositoryTest<City>();
            
            var citydto = new DTO.City() {Name = "Test City1", CountryID = 1};
            var service = new CityService(repo);

            var test = await service.GetAsync(3);
            var anothertest = await service.SaveAsync(citydto);

            var list = await service.GetAllAsync(1, 5, "ID", true);

            //var update = await service.UpdateAsync(test);
        }
    }
}
