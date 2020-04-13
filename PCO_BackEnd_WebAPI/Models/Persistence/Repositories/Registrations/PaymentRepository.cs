using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Registrations;
using PCO_BackEnd_WebAPI.Models.Registrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;
using PCO_BackEnd_WebAPI.Models.Pagination;
namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Registrations
{
	public class PaymentRepository : Repository<Payment> , IPaymentRepository
	{
		public PaymentRepository (ApplicationDbContext context) : base(context)
		{

		}

		public PageResult<Payment> GetPagedPayments(int page, 
													int size,
													DateTime? aPaymentSubmissionDateFrom = null,
													DateTime? aPaymentSubmissionDateTo = null,
													DateTime? aConfirmationDateFrom = null,
													DateTime? aConfirmationDateTo = null)
		{
			PageResult<Payment> pageResult = new PageResult<Payment>();
			int recordCount = appDbContext.Payments.Count();
			int mod;
			int totalPageCount;
			int offset;
			int recordToReturn;
			if (size == 0)
			{
				mod = 0;
				totalPageCount = 1;
				offset = 0;
				recordToReturn = recordCount;
			}
			else
			{
				mod = recordCount % size;
				totalPageCount = (recordCount / size) + (mod == 0 ? 0 : 1);
				offset = size * (page - 1);
				recordToReturn = size;
			}

				pageResult.Results = appDbContext.Payments.OrderBy(p => p.RegistrationId)
					.Where
						  (
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
						   )
												  .Skip(offset)
												  .Take(recordToReturn)
												  .ToList();


			pageResult.PageCount = totalPageCount;
			pageResult.RecordCount = recordCount;
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