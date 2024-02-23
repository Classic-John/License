﻿using Datalayer.Models.SchoolItem;
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
        public void AddSchoolUser(SchoolUser user);
        public void RemoveSchoolUser(SchoolUser user);
        public List<SchoolUser?> GetSchoolUsers();
        public bool IsEmpty();
    }
}
