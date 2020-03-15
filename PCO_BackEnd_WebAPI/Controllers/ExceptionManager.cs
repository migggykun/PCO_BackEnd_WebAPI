using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Controllers
{
    public static class ExceptionManager
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
            }
            while (exception != null);

            return message;
        }
    }
}