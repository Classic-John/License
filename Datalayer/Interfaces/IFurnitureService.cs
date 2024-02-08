using Datalayer.Models;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Interfaces
{
    public interface IFurnitureService : IService
    {
        public new List<Furniture> GetItems();
        public void AddFurnitureTransport(Furniture furniture);
        public void RemoveFurnitureTransport(Furniture furniture);
    }
}
