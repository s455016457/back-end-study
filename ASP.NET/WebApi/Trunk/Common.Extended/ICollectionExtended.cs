using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extended
{
    public static class ICollectionExtended
    {
        /// <summary>
        /// 比较两个集合中的数值相等
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean ValueEquest(this ICollection obj, ICollection value)
        {
            if (obj == null && value == null) return true;
            if (obj == null || value == null) return false;

            if (obj.Count != value.Count) return false;

            object[] objArray = new object[obj.Count];
            object[] valArray = new object[value.Count];

            obj.CopyTo(objArray, 0);
            value.CopyTo(valArray, 0);

            for (int i = 0; i < objArray.Count(); i++)
            {
                var objValue = objArray[i];
                var valValue = valArray[i];

                if (!objValue.ValueEquest(valValue)) return false;
            }

            return true;
        }

        public static Boolean ValueEquest<T>(this ICollection<T> obj, ICollection<T> value)
        {
            if (obj == null && value == null) return true;
            if (obj == null || value == null) return false;

            if (obj.Count != value.Count) return false;

            T[] objArray = new T[obj.Count];
            T[] valArray = new T[value.Count];

            obj.CopyTo(objArray, 0);
            value.CopyTo(valArray, 0);

            for (int i = 0; i < objArray.Count(); i++)
            {
                var objValue = objArray[i];
                var valValue = valArray[i];

                if (!objValue.ValueEquest(valValue)) return false;
            }

            return true;
        }
    }
}
