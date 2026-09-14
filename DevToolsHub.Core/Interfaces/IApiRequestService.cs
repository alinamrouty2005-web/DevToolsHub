using DevToolsHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Interfaces
{
    public interface IApiRequestService
    {
        Task<List<ApiRequest>> GetAllAsync();

        Task<ApiRequest?> GetByIdAsync(int id);

        Task<List<ApiRequest>> GetByUserIdAsync(int userId);

        Task<List<ApiRequest>> GetByProjectIdAsync(int projectId);

        Task<List<ApiRequest>> GetByMethodAsync(string method);

        Task<List<ApiRequest>> GetPagedAsync(int pageNumber,int pageSize);

        Task<ApiRequest> CreateAsync(ApiRequest apiRequest);

        Task<bool> UpdateAsync(int id, ApiRequest apiRequest);

        Task<bool> DeleteAsync(int id);
    }
}
