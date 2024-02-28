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
            => _unitOfWork.Transports;
        public void AddTransport(Transport transport)
        {
            try { transport.Id = GetItems().Last().Id + 1; }
            catch(Exception) { transport.Id = 1; }
            _unitOfWork.Transports.Add(transport);
        }
        public void RemoveTransport(Transport transport) => _unitOfWork.Transports.Remove(transport);
        public List<string> GetCompanyNames()
        {
            List<IndustryUser> result = new();
            foreach (var iu in _unitOfWork.IndustryUsers)
            {
                foreach (var trans in _unitOfWork.Transports)
                {
                    if (iu.Id == trans.CreatorId)
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
