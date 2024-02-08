using Datalayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Interfaces
{
    public interface IJobService : IService
    {
        public new List<Job> GetItems();
        public void AddJob(Job job);
        public void RemoveJob(Job job);
    }
}
