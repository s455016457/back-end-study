using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extended
{
    public static class ObjectExtended
    {
        /// <summary>
        /// 比较两个对象的值相等
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean ValueEquest(this object obj, object value)
        {
            var objType = obj.GetType();
            var valType = value.GetType();
            if (objType.IsGenericType && valType.IsGenericType)//如果对象为泛型
            {
                Type[] objGenericTypes = objType.GenericTypeArguments;
                Type[] varGenericTypes = objType.GenericTypeArguments;
                if (objGenericTypes.Count() != varGenericTypes.Count()) return false;

                for (int i = 0; i < objGenericTypes.Count(); i++)
                {
                    if (!objGenericTypes[i].Equals(varGenericTypes[i])) return false;
                }
            }
            else
            {
                if (!objType.Equals(valType)) return false;
            }

            if (objType.IsValueType || objType.Equals(typeof(String))) return obj.Equals(value);

            if (objType.GetInterface(typeof(IEnumerable).Name) != null)
            {
                var ICObj = obj as IEnumerable;
                var ICVal = value as IEnumerable;
                return ICObj.ValueEquest(ICVal);
            }

            var objProperties = objType.GetProperties();
            var valProperties = valType.GetProperties();

            if (objProperties == null && valProperties == null)
                return true;
            else if (objProperties != null && valProperties != null)
            {
                foreach (var objPItem in objProperties)
                {
                    var valPItem = valProperties.FirstOrDefault(p => p.Name.Equals(objPItem.Name));
                    if (valPItem == null) return false;

                    var objValue = objPItem.GetValue(obj);
                    var valValue = objPItem.GetValue(value);

                    if (!objValue.ValueEquest(valValue)) return false;

                }
            }
            else
                return false;


            return true;
         }
    }
}
