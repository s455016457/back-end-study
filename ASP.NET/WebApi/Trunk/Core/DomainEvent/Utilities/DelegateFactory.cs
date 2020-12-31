using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Event.Utilities
{/// <summary>
 /// 动态委托工厂
 /// </summary>
    public class DelegateFactory
    {
        /// <summary>
        /// 从给定的methodInfo和parameterTypes创建一个委托。
        /// </summary>
        /// <typeparam name="TDelegate">委托</typeparam>
        /// <param name="methodInfo">方法信息</param>
        /// <param name="parameterTypes">参数类型</param>
        /// <returns></returns>
        public static TDelegate CreateDelegate<TDelegate>(MethodInfo methodInfo, Type[] parameterTypes) where TDelegate : class
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }
            if (parameterTypes == null)
            {
                throw new ArgumentNullException("parameterTypes");
            }
            var parameters = methodInfo.GetParameters();
            var dynamicMethod = new DynamicMethod(
                methodInfo.Name,
                MethodAttributes.Static | MethodAttributes.Public,
                CallingConventions.Standard,
                methodInfo.ReturnType,
                parameterTypes,
                typeof(object),
                true)
            {
                InitLocals = false
            };
            var dynamicEmit = new DynamicEmit(dynamicMethod);
            if (!methodInfo.IsStatic)
            {
                dynamicEmit.LoadArgument(0);
                dynamicEmit.CastTo(typeof(object), methodInfo.DeclaringType);
            }
            for (int index = 0; index < parameters.Length; index++)
            {
                dynamicEmit.LoadArgument(index);
                dynamicEmit.CastTo(parameterTypes[index], parameters[index].ParameterType);
            }
            dynamicEmit.Call(methodInfo);
            dynamicEmit.Return();

            return dynamicMethod.CreateDelegate(typeof(TDelegate)) as TDelegate;
        }

        /// <summary>
        /// 动态发行
        /// </summary>
        class DynamicEmit
        {
            /// <summary>
            /// 生成 Microsoft 中间语言 (MSIL) 指令。
            /// </summary>
            private ILGenerator _ilGenerator;
            private static readonly Dictionary<Type, OpCode> _converts = new Dictionary<Type, OpCode>();

            /// <summary>
            /// 静态构造函数
            /// </summary>
            static DynamicEmit()
            {
                _converts.Add(typeof(sbyte), OpCodes.Conv_I1);
                _converts.Add(typeof(short), OpCodes.Conv_I2);
                _converts.Add(typeof(int), OpCodes.Conv_I4);
                _converts.Add(typeof(long), OpCodes.Conv_I8);
                _converts.Add(typeof(byte), OpCodes.Conv_U1);
                _converts.Add(typeof(ushort), OpCodes.Conv_U2);
                _converts.Add(typeof(uint), OpCodes.Conv_U4);
                _converts.Add(typeof(ulong), OpCodes.Conv_U8);
                _converts.Add(typeof(float), OpCodes.Conv_R4);
                _converts.Add(typeof(double), OpCodes.Conv_R8);
                _converts.Add(typeof(bool), OpCodes.Conv_I1);
                _converts.Add(typeof(char), OpCodes.Conv_U2);
            }
            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="dynamicMethod"> 定义并表示一种可编译、执行和丢弃的动态方法。 丢弃的方法可用于垃圾回收。</param>
            public DynamicEmit(DynamicMethod dynamicMethod)
            {
                this._ilGenerator = dynamicMethod.GetILGenerator();
            }
            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="ilGen">生成 Microsoft 中间语言 (MSIL) 指令。</param>
            public DynamicEmit(ILGenerator ilGen)
            {
                this._ilGenerator = ilGen;
            }
            /// <summary>
            /// 加载参数
            /// </summary>
            /// <param name="argumentIndex">参数下标</param>
            public void LoadArgument(int argumentIndex)
            {
                switch (argumentIndex)
                {
                    case 0:
                        this._ilGenerator.Emit(OpCodes.Ldarg_0);
                        break;
                    case 1:
                        this._ilGenerator.Emit(OpCodes.Ldarg_1);
                        break;
                    case 2:
                        this._ilGenerator.Emit(OpCodes.Ldarg_2);
                        break;
                    case 3:
                        this._ilGenerator.Emit(OpCodes.Ldarg_3);
                        break;
                    default:
                        if (argumentIndex < 0x100)
                        {
                            this._ilGenerator.Emit(OpCodes.Ldarg_S, (byte)argumentIndex);
                        }
                        else
                        {
                            this._ilGenerator.Emit(OpCodes.Ldarg, argumentIndex);
                        }
                        break;
                }
            }
            public void CastTo(Type fromType, Type toType)
            {
                if (fromType != toType)
                {
                    if (toType == typeof(void))
                    {
                        if (!(fromType == typeof(void)))
                        {
                            this.Pop();
                        }
                    }
                    else
                    {
                        if (fromType.IsValueType)
                        {
                            if (toType.IsValueType)
                            {
                                this.Convert(toType);
                                return;
                            }
                            this._ilGenerator.Emit(OpCodes.Box, fromType);
                        }
                        this.CastTo(toType);
                    }
                }
            }
            public void CastTo(Type toType)
            {
                if (toType.IsValueType)
                {
                    this._ilGenerator.Emit(OpCodes.Unbox_Any, toType);
                }
                else
                {
                    this._ilGenerator.Emit(OpCodes.Castclass, toType);
                }
            }
            public void Pop()
            {
                this._ilGenerator.Emit(OpCodes.Pop);
            }
            /// <summary>
            /// 从当前方法返回，并将返回值（如果存在）从调用方的计算堆栈推送到被调用方的计算堆栈上。
            /// </summary>
            /// <param name="toType"></param>
            public void Convert(Type toType)
            {
                this._ilGenerator.Emit(_converts[toType]);
            }
            /// <summary>
            /// 返回
            /// </summary>
            public void Return()
            {
                this._ilGenerator.Emit(OpCodes.Ret);
            }
            /// <summary>
            /// 调用方法
            /// </summary>
            /// <param name="method"></param>
            public void Call(MethodInfo method)
            {
                if (method.IsFinal || !method.IsVirtual)
                {
                    this._ilGenerator.EmitCall(OpCodes.Call, method, null);
                }
                else
                {
                    this._ilGenerator.EmitCall(OpCodes.Callvirt, method, null);
                }
            }
        }
    }
}
