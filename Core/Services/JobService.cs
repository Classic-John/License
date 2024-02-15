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
            => _unitOfWork.Jobs;
        public void AddJob(Job job)
        {
            job.Id=GetItems().Last().Id+1;
            _unitOfWork.Jobs.Add(job);
        }
        public void RemoveJob(Job job) 
            => _unitOfWork.Jobs.Remove(job);

        public List<string> GetCompanyNames()
        {
            List<IndustryUser> result = new();
            foreach (var iu in _unitOfWork.IndustryUsers)
            {
                foreach (var job in _unitOfWork.Jobs)
                {
                    if (iu.Id == job.CreatorId)
                    {
                        result.Add(iu);
                    }
                }
            }
            return result.Select(iu => iu.CompanyName).ToList();
        }

        List<AbstractModel> IService.GetItems() => GetItems().Select(item => (AbstractModel)item).ToList();
    }
}
