using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Plugins.Dream.FSM
{
	/// <summary>
	/// 过度参数的成立条件类型
	/// </summary>
	[Flags]
	public enum ConditionType
	{
		None = 0,
		Less = 1,
		LessEquals = 2,
		Equals = 4,
		Greater = 8,
		GreaterEquals = 16,
		NotEquals = 32,
		Func = 64
	}

	public class Condition
	{
		private string paramName;

		public string ParamName { get { return paramName; } }

		private ConditionType conditionType;

		public ConditionType ConditionType { get { return conditionType; } }

		private object args;
		public object Args
		{
			get { return args; }
		}

		public Condition(string paramName, object args, ConditionType conditionType)
		{
			this.paramName = paramName;
			this.args = args;
			this.conditionType = conditionType;
		}

		public bool Check(FiniteStateMachine fsm)
		{
			if (string.IsNullOrEmpty(paramName))
				return false;

			var param = fsm.GetParam<Param>(paramName);
			if (param == null)
				return false;

			bool result = false;
			switch (param.Type)
			{
				case ParamType.Int:
				case ParamType.Float:
				case ParamType.Double:
					var valueParam = fsm.GetParam<ParamValue>(paramName);
					if (valueParam != null)
					{
						result = CheckDouble(Convert.ToDouble(valueParam.Value), Convert.ToDouble(args), this.conditionType);
					}
					break;
				case ParamType.Boolean:
					var boolParam = fsm.GetParam<ParamBoolean>(paramName);
					if (boolParam != null)
					{
						result = CheckBool(Convert.ToBoolean(boolParam.Value), Convert.ToBoolean(args), this.conditionType);
					}
					break;
				case ParamType.Func:
					var funcParam = fsm.GetParam<ParamFunc>(paramName);
					if (funcParam != null)
					{
						FuncParamHandler action = (FuncParamHandler)funcParam.Value;
						object[] funcArgs = args == null ? null : (args is object[] ? args as object[] : new object[] { args });
						result = action != null ? action(funcArgs) : false;
					}
					break;
				default:
					throw new Exception(string.Format("PipelineCondition.Check(FSM fsm) paramName : {0}, paramType : {1} unhandler!", paramName, param.Type));
			}

			return result;
		}

		private static bool CheckBool(bool a, bool b, ConditionType type)
		{
			if (type == ConditionType.Equals)
				return a == b;
			else if (type == ConditionType.NotEquals)
				return a != b;
			return false;
		}

		private static bool CheckInt(int a, int b, ConditionType type)
		{
			return CheckValueAndTarget(a, b, type);
		}

		private static bool CheckFloat(float a, float b, ConditionType type)
		{
			return CheckValueAndTarget(a, b, type);
		}

		private static bool CheckDouble(double a, double b, ConditionType type)
		{
			return CheckValueAndTarget(a, b, type);
		}

		private static bool CheckValueAndTarget(double value, double target, ConditionType type)
		{
			bool result = false;
			if (ContainType(type, ConditionType.Less))
			{
				result = value < target;
			}
			else if (ContainType(type, ConditionType.LessEquals))
			{
				result = value <= target;
			}
			else if (ContainType(type, ConditionType.Equals))
			{
				result = value == target;
			}
			else if (ContainType(type, ConditionType.Greater))
			{
				result = value > target;
			}
			else if (ContainType(type, ConditionType.GreaterEquals))
			{
				result = value >= target;
			}
			else if (ContainType(type, ConditionType.NotEquals))
			{
				result = value != target;
			}
			return result;
		}

		public static bool ContainType(ConditionType type, ConditionType beContainedType)
		{
			return (type & beContainedType) == beContainedType;
		}
	}

	public class ConditionFunc : Condition
	{
		public ConditionFunc(string paramName, object args) : base(paramName, args, ConditionType.Func)
		{

		}
	}
}
