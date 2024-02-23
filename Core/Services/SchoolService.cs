using Datalayer;
using Datalayer.Interfaces;
using Datalayer.Models.SchoolItem;
using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly UnitOfWork _unitOfWork;
        public SchoolService(UnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;
        public SchoolUser? FindSchoolUser(int id)
            => _unitOfWork.SchoolUsers.FirstOrDefault(user => user.UserId == id);
        public List<School>?GetSchoolServices(int id)
            =>_unitOfWork.Schools.FindAll(item => item.CreatorId == id);
        public void AddSchoolUser(SchoolUser user)
            =>_unitOfWork.SchoolUsers.Add(user);
        public void RemoveSchoolUser(SchoolUser user)
        => _unitOfWork.SchoolUsers.Remove(user);
        public List<SchoolUser?> GetSchoolUsers()
            => _unitOfWork.SchoolUsers;
    }
}
