using Datalayer;
using Datalayer.Interfaces;
using Datalayer.Models;
using Datalayer.Models.Users;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class FurnitureService : IFurnitureService
    {
        private readonly UnitOfWork _unitOfWork;
        public FurnitureService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Furniture> GetItems() => _unitOfWork.FurnitureTransports;
        public void AddFurnitureTransport(Furniture furniture)=> _unitOfWork.FurnitureTransports.Add(furniture);
        public void RemoveFurnitureTransport(Furniture furniture)=> _unitOfWork.FurnitureTransports.Remove(furniture);
        public List<string> GetCompanyNames()
        {
            List<IndustryUser> result = new();
            foreach(var iu in _unitOfWork.IndustryUsers) 
            {
                foreach(var fur in _unitOfWork.FurnitureTransports )
                {
                    if(iu.Id==fur.CreatorId) 
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
