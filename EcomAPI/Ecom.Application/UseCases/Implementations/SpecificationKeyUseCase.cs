using Ecom.Application.Abstractions.Persistence;
using Ecom.Application.Models.Requests.SpecificationKey;
using Ecom.Application.Models.Responses.SpecificationKey;
using Ecom.Application.UseCases.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Shared.Common;

namespace Ecom.Application.UseCases.Implementations
{
    public class SpecificationKeyUseCase : ISpecificationKeyUseCase
    {
        private readonly ISpecificationKeyRepository _specificationKeyRepo;

        public SpecificationKeyUseCase(ISpecificationKeyRepository specificationKeyRepo)
        {
            _specificationKeyRepo = specificationKeyRepo;
        }

        public async Task<ApiResponse<SpecKeyResponse>> CreateAsync(CreateSpecKeyRequest request )
        {
            if (request is null)
                return ApiResponse<SpecKeyResponse>.Fail("Request cannot be null.");
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrEmpty(request.Unit))
                return ApiResponse<SpecKeyResponse>.Fail("Invalid Data.");
            var item = SpecificationKey.Create(request.Name, request.Unit);
            await _specificationKeyRepo.AddAsync(item);
            await _specificationKeyRepo.SaveChangesAsync();

            return ApiResponse<SpecKeyResponse>.Ok(new SpecKeyResponse
            {
                Id = item.Id,
                Name = item.Name,
                Unit = item.Unit
            }, "Specification Key created successfully.");

        }

        public async Task<ApiResponse<string>> DeleteAsync(int id )
        {
            var item = await _specificationKeyRepo.GetByIdAsync(id);
            if (item is null)
                return ApiResponse<string>.Fail("Specification Key not found.");
            if (item.IsDeleted)
                return ApiResponse<string>.Fail("Specification Key already deleted.");
            await _specificationKeyRepo.SoftDeleteAsync(item);
            await _specificationKeyRepo.SaveChangesAsync();
            return ApiResponse<string>.Ok(null, "Specification Key deleted successfully.");
        }

        public async Task<ApiResponse<IEnumerable<SpecKeyResponse>>> GetAllAsync()
        {
            var items = await _specificationKeyRepo.GetAllAsync();
            var response = items.Where(i => !i.IsDeleted).Select(item => new SpecKeyResponse
            {
                Id = item.Id,
                Name = item.Name,
                Unit = item.Unit
            }).ToList();

            return ApiResponse<IEnumerable<SpecKeyResponse>>.Ok(response, "Specification Keys retrieved successfully.");
        }

        public async Task<ApiResponse<SpecKeyResponse>> GetByIdAsync(int id)
        {
            var item = await _specificationKeyRepo.GetByIdAsync(id);
            if (item is null || item.IsDeleted)
                return ApiResponse<SpecKeyResponse>.Fail("Specification Key not found.");
            return ApiResponse<SpecKeyResponse>.Ok(new SpecKeyResponse
            {
                Id = item.Id,
                Name = item.Name,
                Unit = item.Unit
            }, "Specification Key retrieved successfully.");
        }

        public async Task<ApiResponse<SpecKeyResponse>> UpdateAsync(int id, UpdateSpecKeyRequest request )
        {
            var item = await _specificationKeyRepo.GetByIdAsync(id);
            if (item is null || item.IsDeleted == true)
                return ApiResponse<SpecKeyResponse>.Fail("Specification Key not found.");
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrEmpty(request.Unit))
                return ApiResponse<SpecKeyResponse>.Fail("Invalid Data.");
            item.Update(request.Name, request.Unit);
            await _specificationKeyRepo.UpdateAsync(item);
            await _specificationKeyRepo.SaveChangesAsync();

            return ApiResponse<SpecKeyResponse>.Ok(new SpecKeyResponse
            {
                Id = item.Id,
                Name = item.Name,
                Unit = item.Unit
            }, "Specification Key updated successfully.");
        }
    }
}
