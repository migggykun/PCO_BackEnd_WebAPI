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
using PCO_BackEnd_WebAPI.Models.Images.Manager;
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

		public PageResult<Payment> GetPagedPayments(int userId)
        {
			PageResult<Payment> pageResult;
			IQueryable<Payment> queryResult = appDbContext.Payments.OrderBy(p => p.Id).Where(p => p.MemberRegistration.UserId == userId);

			pageResult = PaginationManager<Payment>.GetPagedResult(queryResult, 0, 0);
			return pageResult;
		}

		public override void Add(Payment payment)
		{
			//Set details       
            SetPaymentDetails(payment);
			appDbContext.Payments.Add(payment);
		}

		public void UpdatePayment(Payment oldPayment, Payment newPayment, string base64Image)
		{
            if (!string.IsNullOrEmpty(base64Image))
            {
                var receipt = appDbContext.Receipts.Find(oldPayment.RegistrationId);
                receipt.Image = new ImageManager(base64Image).GetAdjustedSizeInBytes();
            }

            oldPayment.PaymentSubmissionDate = DateTime.Now;
            oldPayment.TransactionNumber = Guid.NewGuid().ToString();
            oldPayment.AmountPaid = newPayment.AmountPaid;
            oldPayment.Remarks = newPayment.Remarks;
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