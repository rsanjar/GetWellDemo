using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Enums;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.DTO;
using GetWell.Service.Interface;
using GetWell.Service.Services;
using Xunit;

namespace GetWell.Service.Tests.Services
{
    public class ClinicServiceTest
    {
        private readonly IClinicService _service;

        public ClinicServiceTest()
        {
            _service = new ClinicService(new RepositoryTest<Data.Model.Clinic>());
        }

        public async Task<PaginatedList<Clinic>> GetAllAsync(PaginatedList<Clinic> pagination)
        {
            return await _service.GetAllAsync(pagination);
        }

        public async Task<PaginatedList<Clinic>> GetAllAsync(int pageNumber, int pageSize, string orderByString, bool isAsc = true)
        {
            return await _service.GetAllAsync(pageNumber, pageSize, orderByString, isAsc);
        }
        
        [Theory]
        [InlineData(1)]
        public async Task GetClinicById_CheckIfNotNull(int id)
        {
            var value = await _service.GetAsync(id);

            Assert.NotNull(value);
        }

        [Fact]
        public void SaveNewClinic()
        {
            var newItem = new Clinic
            {
                ID = 20, Name = "Clinic 20", Description = "Description 20", Website = "website20.com"
            };

            var result = Task.Run(() => _service.SaveAsync(newItem)).Result;

            Assert.Equal(Crud.Success, result.MessageKey);
        }

        // [Fact]
        // public async Task UpdateClinic()
        // {
        //     var newItem = new Clinic
        //     {
        //         ID = 2, Name = "Clinic 2 Update", Description = "Description 2 Update", Website = "website2update.com"
        //     };
        //
        //     var result = await _service.UpdateAsync(newItem);
        //
        //     Assert.Equal(Crud.Success, result.MessageKey);
        // }
        
        [Theory]
        [InlineData("bb2e4f81-1db8-45c6-892b-77a0aada1818")]
        public async Task<Clinic> GetClinicByUniqueKey(Guid uniqueKey)
        {
            return await _service.GetAsync(uniqueKey);
        }
    }

    public class ClinicData
    {
        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] {20, "Clinic 20 Update", "Description 20 Update", "website20update.com"}
            };
    }
}