using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Registrations;
using PCO_BackEnd_WebAPI.Models.Registrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Helpers;
namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Registrations
{
	public class PaymentRepository : Repository<Payment> , IPaymentRepository
	{
		public PaymentRepository (ApplicationDbContext context) : base(context)
		{

		}

		public PageResult<Payment> GetPagedPayments(string filter,
                                                    int page, 
													int size,
													DateTime? aPaymentSubmissionDateFrom = null,
													DateTime? aPaymentSubmissionDateTo = null,
													DateTime? aConfirmationDateFrom = null,
													DateTime? aConfirmationDateTo = null)
		{
            PageResult<Payment> pageResult;
            int amount;
            bool IsAmountValid = DataConverter.ConvertToInt(filter, out amount);
	        IQueryable<Payment> queryResult = appDbContext.Payments.OrderBy(p => p.RegistrationId)
                                                                   .Where(p => string.IsNullOrEmpty(filter) ? true : p.Registration.Conference.Title.Contains(filter) ||
                                                                   !IsAmountValid ? true : p.AmountPaid == amount)  
					                                               .Where(
							                                              x =>
								                                           (aPaymentSubmissionDateFrom != null && aPaymentSubmissionDateTo != null) ?
										                                          (x.PaymentSubmissionDate >= aPaymentSubmissionDateFrom &&
										                                          x.PaymentSubmissionDate <= aPaymentSubmissionDateTo) 
										                                          : 
										                                          true
						                                           )
					                                               .Where
						                                            (
							                                           x =>
								                                           (aConfirmationDateFrom != null && aConfirmationDateTo != null) ?
										                                          (x.ConfirmationDate >= aConfirmationDateFrom &&
										                                          x.ConfirmationDate <= aConfirmationDateTo)
										                                          :
										                                          true
						                                            );



            pageResult = PaginationManager<Payment>.GetPagedResult(queryResult, page, size);
            return pageResult;
		}

		public override void Add(Payment payment)
		{
			//Set details
            SetPaymentDetails(payment);
			appDbContext.Payments.Add(payment);
		}

		public void UpdatePayment(int id, Payment payment, byte[] imageSize)
		{
			payment.RegistrationId = id;
            payment.ProofOfPayment = imageSize;
            SetPaymentDetails(payment);
			appDbContext.UpdateGraph<Payment>(payment);
		}

		public void SetPaymentConfirmationDate(int id)
		{
			var obj = appDbContext.Payments.Find(id);
			obj.ConfirmationDate = DateTime.Now;
			string x = obj.ToString();
		}

        private void SetPaymentDetails(Payment payment)
        {
            payment.PaymentSubmissionDate = DateTime.Now;
            payment.TransactionNumber = Guid.NewGuid().ToString(); ;
        }
		private ApplicationDbContext appDbContext
		{
			get
			{
				return _context as ApplicationDbContext;
			}
		}
	}
}