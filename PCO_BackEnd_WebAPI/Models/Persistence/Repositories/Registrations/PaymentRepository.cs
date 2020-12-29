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
using PCO_BackEnd_WebAPI.Models.Images;

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
			int statusId = string.IsNullOrEmpty(filter) ? -1 : getRegistrationStatusId(filter);

			IQueryable<Payment> queryResult = appDbContext.Payments
				.Where(u=>(filter == null)?true:
					(u.MemberRegistration.User.UserInfo.FirstName.Contains(filter)) ||
					(u.MemberRegistration.User.UserInfo.LastName.Contains(filter)) ||
					(u.MemberRegistration.User.UserInfo.MiddleName.Contains(filter)) ||
					(u.Registration.User.UserInfo.FirstName.Contains(filter)) ||
					(u.Registration.User.UserInfo.LastName.Contains(filter)) ||
					(u.Registration.User.UserInfo.MiddleName.Contains(filter)) ||
					(u.Registration.Conference.Title.Contains(filter)) ||
					(u.AmountPaid.ToString().Contains(filter)) ||
					((statusId != -1) && (u.Registration.RegistrationStatusId == statusId) || (u.MemberRegistration.MemberRegistrationStatusId == statusId)));


			//apply Date Filters
			queryResult = queryResult.Where(x =>
				(aPaymentSubmissionDateFrom != null && aPaymentSubmissionDateTo != null) ?
				(x.PaymentSubmissionDate >= aPaymentSubmissionDateFrom &&
				x.PaymentSubmissionDate <= aPaymentSubmissionDateTo) 
				: 
				true)
			.Where(x =>
				(aConfirmationDateFrom != null && aConfirmationDateTo != null) ?
				(x.ConfirmationDate >= aConfirmationDateFrom &&
				x.ConfirmationDate <= aConfirmationDateTo)
				:
				true);

			pageResult = PaginationManager<Payment>.GetPagedResult(queryResult, page, size);
            return pageResult;
		}

		public int getRegistrationStatusId(string registrationStatus)
        {
			if(appDbContext.RegistrationStatus.ToList().Find(x=>x.StatusLabel == registrationStatus)!=null)
            {
				return appDbContext.RegistrationStatus.ToList().Find(x => x.StatusLabel == registrationStatus).Id;
			}
			return -1;
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
				var receipt = appDbContext.Receipts.ToList().Find(x => x.Id == oldPayment.Id);
				if(receipt!=null)
                {
					receipt.Image = new ImageManager(base64Image).GetAdjustedSizeInBytes();
				}
				else
                {
					ImageManager receiptManager = new ImageManager(base64Image);
					oldPayment.Receipt = new Receipt();
					oldPayment.Receipt.Id = newPayment.Id;
					oldPayment.Receipt.Image = receiptManager.GetAdjustedSizeInBytes();
				}
                
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