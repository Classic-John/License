using Datalayer.Models.SchoolItem;
using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Interfaces
{
    public interface ISchoolService : IWrapper
    {
        public SchoolUser? FindSchoolUser(int id);
        public List<School>? GetSchoolServices(int id);
        public Task<SchoolUser> AddSchoolUser(SchoolUser user);
        public Task<bool> RemoveSchoolUser(SchoolUser user);
        public List<SchoolUser?> GetSchoolUsers();
        public Task<School> AddSchoolItem(School item);
        public School? FindSchoolService(int id, int schoolId);
        public Task<bool> RemoveSchoolService(School item);
        public List<School> GetSchools();
        public bool IsEmpty();
        public Task<School> UpdateSchool(School school);
        public Task<SchoolUser> UpdateSchoolUser(SchoolUser user);
        public Task<bool> DeleteSchoolUser(int id);
    }
}
