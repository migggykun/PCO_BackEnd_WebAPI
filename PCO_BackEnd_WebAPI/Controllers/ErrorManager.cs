using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace PCO_BackEnd_WebAPI.Controllers
{
    public static class ErrorManager
    {
        /// <summary>
        /// Method that returns all exceptions and inner exceptions
        /// </summary>
        /// <param name="aException"></param>
        /// <returns></returns>
        public static string GetAllExceptionMessages(Exception aException)
        {
            string message = String.Empty;
            Exception exception = aException;

            do
            {
                message += exception.GetType().ToString() + ": " + exception.Message + "\n";
                exception = exception.InnerException;
            }
            while (exception != null);

            return message;
        }

        /// <summary>
        /// Method that returns innest exception message 
        /// </summary>
        /// <param name="aException"></param>
        /// <returns></returns>
        public static string GetInnerExceptionMessage(Exception aException)
        {
            string message = String.Empty;
            Exception exception = aException;

            do
            {
                message = exception.GetType().ToString() + ": " + exception.Message;
                exception = exception.InnerException;

                if(exception is SqlException)
                {
                    message = GetSqlExceptions(exception as SqlException);
                    break;
                }
            }
            while (exception != null);

            return message;
        }

        private static string GetSqlExceptions(SqlException aException)
        {
            string message = "";
            switch(aException.Number)
            {
                case 547:
                    message = "Selected item does not exist!";
                    break;
                default:
                    break;
            }
            return message;   
        }

        public static string GetModelStateErrors(ModelStateDictionary modelState)
        {
            string errorMessages = string.Join("\n", modelState.Values
                                                       .SelectMany(v => v.Errors)
                                                       .Select(e => e.ErrorMessage));
            return errorMessages;
        }
    }
}