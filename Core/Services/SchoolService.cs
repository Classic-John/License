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
            => GetSchoolUsers().FirstOrDefault(user => user.UserId == id);
        public List<School>? GetSchoolServices(int id)
            => GetSchools().FindAll(item => item.CreatorId == id);
        public School? FindSchoolService(int id, int schoolId)
            => GetSchoolServices(id).FirstOrDefault(item => item.Id == schoolId);
        public async Task<SchoolUser> AddSchoolUser(SchoolUser user)
            => await _unitOfWork.SchoolUsers.Add(user);
        public async Task<School> AddSchoolItem(School item)
            => await _unitOfWork.Schools.Add(item);
        public async Task<bool> RemoveSchoolUser(SchoolUser user)
            => await _unitOfWork.SchoolUsers.Delete(user);
        public async Task<bool> RemoveSchoolService(School item)
            => await _unitOfWork.Schools.Delete(item);
        public List<SchoolUser?> GetSchoolUsers()
            => _unitOfWork.SchoolUsers.GetItems();
        public bool IsEmpty()
            => _unitOfWork.SchoolUsers == null;
        public List<School> GetSchools()
            => _unitOfWork.Schools.GetItems();
        public async Task<School> UpdateSchool(School school)
            => await _unitOfWork.Schools.Update(school);
        public async Task<SchoolUser> UpdateSchoolUser(SchoolUser user)
            => await _unitOfWork.SchoolUsers.Update(user);

        public async Task<bool> DeleteSchoolUser(int id)
        {
            SchoolUser user= FindSchoolUser(id);
            List<School> services = GetSchoolServices(id);
            await _unitOfWork.Schools.DeleteAll(services);
            await _unitOfWork.SchoolUsers.Delete(user);
            return true;
        }
    }
}
