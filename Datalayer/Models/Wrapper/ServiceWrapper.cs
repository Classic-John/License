using Datalayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Models.Wrapper
{
    public class ServiceWrapper
    {
        public IBookingService _bookingService;
        public IFurnitureService _furnitureService;
        public IJobService _jobService;
        public IRentingService _rentingService;
        public ITransportService _transportService;
        public IUserService _userService;
        public IIndustryUserService _industryUserService;
        public ISchoolService _schoolService;

        public ServiceWrapper(IBookingService bookingService, IFurnitureService furnitureService, IJobService jobService, IRentingService rentingService, ITransportService transportService, 
            IUserService userService, IIndustryUserService industryUserService, ISchoolService schoolService)
        {
            _bookingService = bookingService;
            _furnitureService = furnitureService;
            _jobService = jobService;
            _rentingService = rentingService;
            _transportService = transportService;
            _userService = userService;
            _industryUserService = industryUserService;
            _schoolService = schoolService;
        }
    }
}
