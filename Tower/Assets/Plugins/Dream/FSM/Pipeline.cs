using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Plugins.Dream.FSM
{
	/// <summary>
	/// 状态之间的过渡管道(可包含多个过渡条件)
	/// </summary>
	public class Pipeline
	{
		private string srcStateName;
		public string SrcStateName
		{
			get { return srcStateName; }
		}

		private string targetStateName;
		public string TargetStateName
		{
			get { return targetStateName; }
			set { targetStateName = value; }
		}

		private List<Condition> conditions = new List<Condition>();

		public Pipeline(string srcStateName, string targetStateName)
		{
			this.srcStateName = srcStateName;
			this.targetStateName = targetStateName;
		}

		/// <summary>
		/// 调用该方法前，最好先调用CheckAndThrowException
		/// </summary>
		public Condition AddCondition(string name, object args, ConditionType type)
		{
			var result = new Condition(name, args, type);
			conditions.Add(result);
			return result;
		}

		public void AddConditions(List<Condition> conditions)
		{
			if (conditions != null)
			{
				foreach (var c in conditions)
					AddCondition(c);
			}
		}
		/// <summary>
		/// 调用该方法前，最好先调用CheckAndThrowException
		/// </summary>
		public void AddCondition(Condition condition)
		{
			if (conditions.Contains(condition))
				return;
			conditions.Add(condition);
		}

		public void RemoveCondition(int idx)
		{
			conditions.RemoveAt(idx);
		}

		public void RemoveCondition(Condition condition)
		{
			if (!conditions.Contains(condition))
				return;
			conditions.Remove(condition);
		}

		public bool Check(FiniteStateMachine fsm)
		{
			var result = true;
			foreach (var condition in conditions)
			{
				if (!condition.Check(fsm)) // 只要有一个不成立，则不通过
				{
					result = false;
					break;
				}
			}
			return result;
		}
	}
}
