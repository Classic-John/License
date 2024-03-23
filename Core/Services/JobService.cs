using Datalayer;
using Datalayer.Interfaces;
using Datalayer.Models;
using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class JobService : IJobService
    {
        private readonly UnitOfWork _unitOfWork;
        public JobService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Job> GetItems()
            => _unitOfWork.Jobs.GetItems();
        public async Task<Job> AddJob(Job job)
            => await _unitOfWork.Jobs.Add(job);

        public async Task<bool> RemoveJob(Job job)
            =>  await _unitOfWork.Jobs.Delete(job);

        public List<string> GetCompanyNames()
        {
            List<IndustryUser> result = new();
            foreach (var iu in _unitOfWork.IndustryUsers.GetItems())
            {
                foreach (var job in GetItems())
                {
                    if (iu.Id == job.CreatorId)
                    {
                        result.Add(iu);
                    }
                }
            }
            return result.Select(iu => iu.CompanyName).ToList();
        }

        List<AbstractModel> IService.GetItems()
            => GetItems().Select(item => (AbstractModel)item).ToList();
        public async Task<Job> UpdateJob(Job job)
            => await _unitOfWork.Jobs.Update(job);
    }
}
