using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extended
{
    public static class IEnumerableExtended
    {
        /// <summary>
        /// 比较两个集合中的数值相等
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean ValueEquest(this IEnumerable obj, IEnumerable value)
        {
            if (obj == null && value == null) return true;
            if (obj == null || value == null) return false;

            IEnumerator objIE = obj.GetEnumerator();
            IEnumerator valIE = value.GetEnumerator();
            while (true)
            {
                var b1 = objIE.MoveNext();
                var b2 = valIE.MoveNext();
                if ((!b1 && b2)||(b1 && !b2)) return false;
                if (!b1 && !b2) return true;

                if (!objIE.Current.ValueEquest(valIE.Current)) return false;
            }
        }
    }
}
