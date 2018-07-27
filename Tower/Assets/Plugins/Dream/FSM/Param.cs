using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Plugins.Dream.FSM
{
	/// <summary>
	/// 状态机中，当有参数发生变化时的参数类声明
	/// </summary>
	public class ParamsChangedEvent : EventArgs
	{
		private Param lastValue;
		public Param LastValue
		{
			get { return lastValue; }
		}

		private Param curValue;
		public Param CurValue
		{
			get { return curValue; }
		}

		public ParamsChangedEvent(Param lastValue, Param curValue)
		{
			this.lastValue = lastValue;
			this.curValue = curValue;
		}
	}

	/// <summary>
	/// 过度参数为函数委托的声明
	/// </summary>
	public delegate bool FuncParamHandler(params object[] objs);

	/// <summary>
	/// 过度参数类型
	/// </summary>
	public enum ParamType
	{
		Int,
		Float,
		Double,
		Boolean,
		Func
	}

	public abstract class Param
	{
		private static Dictionary<ParamType, ConditionType> dic = new Dictionary<ParamType, ConditionType>();
		public static ConditionType GetConditionTypeByParamType(ParamType type)
		{
			return dic[type];
		}

		static Param()
		{
			dic[ParamType.Int] =
			dic[ParamType.Float] =
			dic[ParamType.Double] = ConditionType.Less | ConditionType.LessEquals | ConditionType.Equals |
									ConditionType.Greater | ConditionType.GreaterEquals | ConditionType.NotEquals;
			dic[ParamType.Func] = ConditionType.Func;
			dic[ParamType.Boolean] = ConditionType.Equals | ConditionType.NotEquals;
		}

		private string name;
		public string Name
		{
			get { return name; }
		}

		public ParamType Type;
		public object Value; // object Value 可以考虑使用FSMParam<T> => Generic Type

		public Param(string name, ParamType type)
		{
			this.name = name;
			this.Type = type;
		}

		public T GetValue<T>() where T : struct
		{
			return (T)Value;
		}

		public T GetClassValue<T>() where T : class
		{
			return Value as T;
		}

		public bool CheckValue() // self
		{
			return CheckValue(Value);
		}

		public bool CheckValue(object value) // sepecial
		{
			bool result = false;
			switch (Type)
			{
				case ParamType.Int:
					result = value is int;
					break;
				case ParamType.Float:
					result = value is float;
					break;
				case ParamType.Double:
					result = value is double;
					break;
				case ParamType.Boolean:
					result = value is bool;
					break;
				case ParamType.Func:
					result = true; // func args, it can spectial anything..., always return true
					break;
			}
			return result;
		}

		public Param Clone()
		{
			return MemberwiseClone() as Param;
		}

		public override string ToString()
		{
			return string.Format("name : {0}, type : {1}, value : {2}", name, Type, Value);
		}
	}

	/// <summary>
	/// 过度参数是函数类型
	/// </summary>
	public class ParamFunc : Param
	{
		public ParamFunc(string name, Delegate func)
			: base(name, ParamType.Func)
		{
			Value = func;
		}
	}

	/// <summary>
	/// 过度参数是布尔类型
	/// </summary>
	public class ParamBoolean : Param
	{
		public ParamBoolean(string name)
			: base(name, ParamType.Boolean)
		{
			Value = false;
		}
	}

	/// <summary>
	/// 过度参数是值类型
	/// </summary>
	public class ParamValue : Param
	{
		public ParamValue(string name)
			: this(name, ParamType.Int)
		{

		}

		public ParamValue(string name, ParamType type)
			: base(name, type)
		{
			if (type == ParamType.Func)
				throw new Exception(string.Format("FSMParamValue type invalidated, type : {0}", type));
			switch (type)
			{
				case ParamType.Int:
					Value = 0;
					break;
				case ParamType.Float:
					Value = 0f;
					break;
				case ParamType.Double:
					Value = 0d;
					break;
				default:
					throw new Exception(string.Format("FSMParamValue new type : {0} unhandled", type));
			}
		}
	}
}
