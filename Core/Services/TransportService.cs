using Datalayer;
using Datalayer.Interfaces;
using Datalayer.Models;
using Datalayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class TransportService : ITransportService
    {
        private readonly UnitOfWork _unitOfWork;
        public TransportService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Transport> GetItems()
            => _unitOfWork.Transports.GetItems();
        public async void AddTransport(Transport transport)
           => await _unitOfWork.Transports.Add(transport);

        public async void RemoveTransport(Transport transport) 
            => await _unitOfWork.Transports.Delete(transport);
        public List<string> GetCompanyNames()
        {
            List<IndustryUser> result = new();
            foreach (var iu in _unitOfWork.IndustryUsers.GetItems())
            {
                foreach (var trans in GetItems())
                {
                    if (iu.Id == trans.CreatorId)
                    {
                        result.Add(iu);
                    }
                }
            }
            return result.Select(iu => iu.CompanyName).ToList();
        }
        List<AbstractModel> IService.GetItems()
            => GetItems().Select(item => (AbstractModel)item).ToList();
    }
}
