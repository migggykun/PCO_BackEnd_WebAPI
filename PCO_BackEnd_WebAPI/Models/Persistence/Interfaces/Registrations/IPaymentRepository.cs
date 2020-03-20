using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using PCO_BackEnd_WebAPI.Models.Registrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Registrations
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        void UpdatePayment(int id, Payment payment);
        void SetPaymentConfirmationDate(int id);
    }
}
