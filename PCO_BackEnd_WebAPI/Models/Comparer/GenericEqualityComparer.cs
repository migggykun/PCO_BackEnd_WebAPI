using PCO_BackEnd_WebAPI.Models.Conferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Comparer
{
    public class GenericEqualityComparer<TEntity> : IEqualityComparer<TEntity> where TEntity : class
    {

        public GenericEqualityComparer()
        {

        }

        public bool Equals(TEntity object1, TEntity object2)
        {

           if (ReferenceEquals (object1, object2)) return true;
           if ((object1 == null) || (object2 == null)) return false;

           // Comparing class of 2 object1ects, if different, then fail
           if (object1.GetType()!= object2.GetType()) return false;
 
           var result = true;
           // Get all properties of object1
           // then compare the value of each property
           
           foreach (var property in object1.GetType (). GetProperties ())
           {
                var object1Value = property.GetValue (object1);
                var object2Value = property.GetValue (object2);

                if (object1Value == null && object2Value == null)
                {
                    result = true;
                }

                else if ((object1Value == null && object2Value != null) || (object1Value != null && object2Value == null))
                {
                    result = false;
                }

                else if(! object1Value.Equals (object2Value)) result = false;
          }
           return result;
        }

        public int GetHashCode(TEntity obj)
        {
            int result = 1;


            //Check whether the object is null
            if (Object.ReferenceEquals(obj, null)) return 0;

            foreach (var property in obj.GetType().GetProperties())
            {
                var propertyValue = property.GetValue(obj);
                if (propertyValue == null)
                {
                    result = result ^ 0;
                }
                else
                {
                    result = result ^ propertyValue.GetHashCode();
                }
            }

            return result;
        }
    }
}