using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Infrastructure.Services
{
    public class ApiRequestService : IApiRequestService
    {
        private readonly ApiRequestRepository _apiRequestRepository;

        public ApiRequestService(ApiRequestRepository apiRequestRepository)
        {
            _apiRequestRepository = apiRequestRepository;
        }

        public async Task<List<ApiRequest>> GetAllAsync()
        {
            return await _apiRequestRepository.GetAllAsync();
        }

        public async Task<ApiRequest?> GetByIdAsync(int id)
        {
            return await _apiRequestRepository.GetByIdAsync(id);
        }

        public async Task<List<ApiRequest>> GetByUserIdAsync(int userId)
        {
            return await _apiRequestRepository.GetByUserIdAsync(userId);
        }

        public async Task<List<ApiRequest>> GetByProjectIdAsync(int projectId)
        {
            return await _apiRequestRepository.GetByProjectIdAsync(projectId);
        }

        public async Task<List<ApiRequest>> GetByMethodAsync(string method)
        {
            return await _apiRequestRepository.GetByMethodAsync(method);
        }

        public async Task<List<ApiRequest>> GetPagedAsync(int pageNumber,int pageSize)
        {
            return await _apiRequestRepository.GetPagedAsync(pageNumber, pageSize);
        }

        public async Task<ApiRequest> CreateAsync(ApiRequest apiRequest)
        {
            await _apiRequestRepository.AddAsync(apiRequest);

            return apiRequest;
        }

        public async Task<bool> UpdateAsync(int id,ApiRequest apiRequest)
        {
            var existingRequest = await _apiRequestRepository.GetByIdAsync(id);

            if (existingRequest == null)
                return false;

            existingRequest.Name = apiRequest.Name;
            existingRequest.Method = apiRequest.Method;
            existingRequest.Url = apiRequest.Url;
            existingRequest.Headers = apiRequest.Headers;
            existingRequest.Body = apiRequest.Body;
            existingRequest.StatusCode = apiRequest.StatusCode;
            existingRequest.ResponseBody = apiRequest.ResponseBody;

            await _apiRequestRepository.UpdateAsync(existingRequest);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var apiRequest = await _apiRequestRepository.GetByIdAsync(id);

            if (apiRequest == null)
                return false;

            await _apiRequestRepository.DeleteAsync(apiRequest);

            return true;
        }
    }
}
