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
        public List<School>? GetSchoolServices(int id)
            => _unitOfWork.Schools.FindAll(item => item.CreatorId == id);
        public School? FindSchoolService(int id,int schoolId)
            =>GetSchoolServices(id).FirstOrDefault(item => item.Id == schoolId);
        public void AddSchoolUser(SchoolUser user)
        {
            try { user.Id = _unitOfWork.SchoolUsers.Last().Id + 1; }
            catch(Exception) { user.Id = 1; }
            _unitOfWork.SchoolUsers.Add(user); 
        }
        public void AddSchoolItem(School item)
        {
            try { item.Id = _unitOfWork.Schools.Last().Id + 1; }
            catch (Exception) { item.Id = 1; }
            _unitOfWork.Schools.Add(item);
        }
        public void RemoveSchoolUser(SchoolUser user)
        => _unitOfWork.SchoolUsers.Remove(user);
        public void RemoveSchoolService(School item)
            =>_unitOfWork.Schools.Remove(item);
        public List<SchoolUser?> GetSchoolUsers()
            => _unitOfWork.SchoolUsers;
        public bool IsEmpty()
            => _unitOfWork.SchoolUsers == null;
        public List<School>GetSchools()
            => _unitOfWork.Schools;
    }
}
