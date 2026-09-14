using DevToolsHub.Core.Interfaces;
using DevToolsHub.Core.Models;
using DevToolsHub.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ProjectRepository _projectRepository;

        public ProjectService(ProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<List<Project>> GetAllAsync()
        {
            return await _projectRepository.GetAllAsync();
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _projectRepository.GetByIdAsync(id);
        }

        public async Task<List<Project>> GetByUserIdAsync(int userId)
        {
            return await _projectRepository.GetByUserIdAsync(userId);
        }

        public async Task<List<Project>> SearchAsync(string name)
        {
            return await _projectRepository.SearchAsync(name);
        }

        public async Task<List<Project>> GetPagedAsync(int pageNumber,int pageSize)
        {
            return await _projectRepository.GetPagedAsync(pageNumber, pageSize);
        }

        public async Task<Project> CreateAsync(Project project)
        {
            await _projectRepository.AddAsync(project);

            return project;
        }

        public async Task<bool> UpdateAsync(int id,Project project)
        {
            var existingProject =await _projectRepository.GetByIdAsync(id);

            if (existingProject == null)
                return false;

            existingProject.Name = project.Name;
            existingProject.Description = project.Description;

            await _projectRepository.UpdateAsync(existingProject);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);

            if (project == null)
                return false;

            await _projectRepository.DeleteAsync(project);

            return true;
        }
    }
}
