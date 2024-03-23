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
        public List<Furniture> GetItems()
            => _unitOfWork.Furnitures.GetItems();
        public async Task<Furniture> AddFurnitureTransport(Furniture furniture)
          => await _unitOfWork.Furnitures.Add(furniture);

        public async Task<bool> RemoveFurnitureTransport(Furniture furniture)
            => await _unitOfWork.Furnitures.Delete(furniture);
        public async Task<Furniture> UpdateFurniture(Furniture furniture)
            => await _unitOfWork.Furnitures.Update(furniture);
        public List<string> GetCompanyNames()
        {
            List<IndustryUser> result = new();
            foreach (var iu in _unitOfWork.IndustryUsers.GetItems())
            {
                foreach (var fur in GetItems())
                {
                    if (iu.Id == fur.CreatorId)
                    {
                        result.Add(iu);
                    }
                }
            }
            return result.Select(iu => iu.CompanyName).ToList();
        }

        List<AbstractModel> IService.GetItems() =>
            GetItems().Select(item => (AbstractModel)item).ToList();
    }
}
