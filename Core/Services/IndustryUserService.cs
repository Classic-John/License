using Datalayer;
using Datalayer.Interfaces;
using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class IndustryUserService : IIndustryUserService
    {
        private readonly UnitOfWork _unitOfWork;
        public IndustryUserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<IndustryUser> GetIndustryUsers() => _unitOfWork.IndustryUsers;
        public void AddIndustryUser(IndustryUser industryUser) => _unitOfWork.IndustryUsers.Add(industryUser);
        public void RemoveIndustryUser(IndustryUser industryUser) => _unitOfWork.IndustryUsers.Remove(industryUser);
    }
}
