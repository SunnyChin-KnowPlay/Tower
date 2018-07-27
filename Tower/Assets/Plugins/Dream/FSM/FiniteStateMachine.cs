using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.Plugins.Dream.FSM
{
	/// <summary>
	/// 状态机中，当前状态发生改变的事件委托声明
	/// </summary>
	public delegate void CurStateChangedEventHandler(FiniteStateMachine sender);

	/// <summary>
	/// 状态机中，当有参数发生变化时的事件委托声明
	/// </summary>
	public delegate void ParamsChangedEventHandler(FiniteStateMachine sender, ParamsChangedEvent args);

	/// <summary>
	/// 状态参数检验异常类
	/// </summary>
	public class FSMParamInvalidatedException : System.Exception
	{
		public FSMParamInvalidatedException(string msg)
			: base(msg)
		{

		}
	}

	public class FiniteStateMachine
	{
		/// <summary>
		/// throw an FSMParamInvalidatedException
		/// </summary>
		public static void CheckParamValidated(FiniteStateMachine fsm, string name, object args, ConditionType type)
		{
			if ((type != ConditionType.Func || type == ConditionType.None) && args == null)
				throw new FSMParamInvalidatedException(string.Format("ChangStatePipeline add condition args can not be null"));
			var srcParam = fsm.GetParam(name);
			if (srcParam == null)
				throw new FSMParamInvalidatedException(string.Format("fsm not contains param : {0}", name));
			if (!srcParam.CheckValue(args))
				throw new FSMParamInvalidatedException(string.Format("ChangStatePipeline add condition args ValidatedSelf invalidated : {0}", args));
		}

		public void TransitionTo(string srcStateName, string targetStateName)
		{
			TransitionTo(this, srcStateName, targetStateName);
		}

		public static void TransitionTo(FiniteStateMachine fsm, string srcStateName, string targetStateName)
		{
			TransitionTo(fsm, srcStateName, targetStateName, null, true);
		}

		public void TransitionTo(string srcStateName, string targetStateName, Condition[] conditions)
		{
			TransitionTo(this, srcStateName, targetStateName, new List<Condition>(conditions));
		}

		public static void TransitionTo(FiniteStateMachine fsm, string srcStateName, string targetStateName, Condition[] conditions)
		{
			TransitionTo(fsm, srcStateName, targetStateName, new List<Condition>(conditions), true);
		}

		public void TransitionTo(string srcStateName, string targetStateName, List<Condition> conditions)
		{
			TransitionTo(this, srcStateName, targetStateName, conditions);
		}

		public static void TransitionTo(FiniteStateMachine fsm, string srcStateName, string targetStateName, List<Condition> conditions)
		{
			TransitionTo(fsm, srcStateName, targetStateName, conditions, true);
		}

		public void TransitionTo(string srcStateName, string targetStateName, List<Condition> conditions, bool addInSameStateNameTransition)
		{
			TransitionTo(this, srcStateName, targetStateName, conditions, addInSameStateNameTransition);
		}

		public static void TransitionTo(FiniteStateMachine fsm, string srcStateName, string targetStateName, List<Condition> conditions, bool addInSameStateNameTransition)
		{
			Transition transition = null;
			Pipeline pipeline = null;
			GetTransitionAndPipeline(fsm, srcStateName, targetStateName, addInSameStateNameTransition, out transition, out pipeline);
			pipeline.AddConditions(conditions);
		}

		public static void GetTransitionAndPipeline(FiniteStateMachine fsm, string srcStateName, string targetName, bool addInSameStateNameTransition, out Transition out1, out Pipeline out2)
		{
			out1 = null;
			out2 = null;

			var srcState = fsm.GetState(srcStateName);
			if (srcState == null)
				return;
			if (addInSameStateNameTransition)
			{
				var ts = srcState.GetTransition(srcStateName, targetName);
				if (ts.Count > 0)
					out1 = ts[0];
				if (out1 != null)
				{
					var ps = out1.GetPipeline(srcStateName, targetName);
					if (ps.Count > 0)
						out2 = ps[0];
				}
			}
			if (out1 == null)
			{
				out1 = new Transition(srcStateName, targetName);
				srcState.AddTransition(out1);
			}
			if (out2 == null)
			{
				out2 = new Pipeline(srcStateName, targetName);
				out1.AddPipeline(out2);
			}
		}

		public event CurStateChangedEventHandler CurStateChangedEvent;
		public event ParamsChangedEventHandler ParamsChangedEvent;

		private readonly Dictionary<string, Param> fsmParams = new Dictionary<string, Param>();
		private readonly Blackboard blackboard = new Blackboard();

		public string name;

		private State lastState;

		public Blackboard Blackboard
		{
			get { return blackboard; }
		}

		public State LastState
		{
			get { return lastState; }
		}

		private State curState;

		public State CurState
		{
			get { return curState; }
			set { curState = value; }
		}

		private State lastAnyState;
		public State LastAnyState
		{
			get { return lastAnyState; }
		}

		private State anyState;
		public State AnyState
		{
			get { return anyState; }
			set { anyState = value; }
		}

		private Dictionary<string, State> states = new Dictionary<string, State>();

		public FiniteStateMachine()
			: this("FSM")
		{

		}

		public FiniteStateMachine(string name)
		{
			this.name = name;
		}

		public Param AddParam(string name, ParamType type)
		{
			Param result = fsmParams.ContainsKey(name) ? fsmParams[name] : null;

			if (result != null)
			{
				if (result.Type != type)
					throw new Exception(string.Format("FSM AddParam name : {0}, is already def, but still add param same name, but type different, src type : {1}, new type : {2}", result.Type, type));
			}

			if (result == null)
			{
				result = new ParamValue(name) { Type = type };
				switch (type)
				{
					case ParamType.Int:
						result.Value = 0;
						break;
					case ParamType.Float:
						result.Value = 0f;
						break;
					case ParamType.Double:
						result.Value = 0D;
						break;
					case ParamType.Boolean:
						result.Value = false;
						break;
					case ParamType.Func:
						result.Value = null;
						break;
				}
				AddParam(result);
			}

			return result;
		}

		public void AddParam(Param param)
		{
			if (param == null || fsmParams.ContainsKey(param.Name))
				return;
			if (!param.CheckValue())
				throw new Exception(string.Format("fsm add param ValidatedSelf invalidated : {0}", param));
			fsmParams.Add(param.Name, param);
		}

		public void RemoveParam(Param param)
		{
			if (param == null || !fsmParams.ContainsKey(param.Name))
				return;
			fsmParams.Remove(param.Name);
		}

		public T GetParam<T>(string name) where T : Param
		{
			return GetParam(name) as T;
		}

		public T GetParamValue<T>(string name) where T : struct
		{
			return GetParam(name).GetValue<T>();
		}

		public T GetParamClassValue<T>(string name) where T : class
		{
			return GetParam(name).GetClassValue<T>();
		}

		public Param GetParam(string name)
		{
			if (!fsmParams.ContainsKey(name))
				return null;
			return fsmParams[name];
		}

		public void SetParamValue(string name, object value)
		{
			SetParamValue(name, value, true);
		}

		public void SetParamValue(string name, object value, bool autoAsValue)
		{
			var srcParam = GetParam(name);
			if (srcParam == null)
				throw new Exception(string.Format("FSM.SetParamValue param : name : {0} is not contains", name));

			if (autoAsValue && value.GetType() != srcParam.Value.GetType())
			{
				try
				{
					switch (srcParam.Type)
					{
						case ParamType.Int:
							value = Convert.ToInt32(value);
							break;
						case ParamType.Float:
							value = Convert.ToSingle(value);
							break;
						case ParamType.Double:
							value = Convert.ToDouble(value);
							break;
						case ParamType.Func:
							value = (Delegate)(value);
							break;
						default:
							throw new Exception(string.Format("FSM.SetParamValue new param type unhandled, type : {0}", srcParam.Type));
					}
				}
				catch (Exception er)
				{
					throw new Exception(string.Format("FSM.SetParamValue param : name : {0}, value is invalidated : {1}\nException:{2}", name, value, er.ToString()));
				}
			}

			if (!srcParam.CheckValue(value))
				throw new Exception(string.Format("FSM.SetParamValue param : name : {0}, value is invalidated : {1}", name, value));

			var lastValue = srcParam.Clone();
			srcParam.Value = value;
			OnParamsChangedEvent(new ParamsChangedEvent(lastValue, srcParam));

			if (anyState != null)
				anyState.DirtyState();
			if (curState != null)
				curState.DirtyState();
		}

		public void DirtyState()
		{
			if (anyState != null)
				anyState.DirtyState();
			if (curState != null)
				curState.DirtyState();
		}

		public void AddState(State state)
		{
			if (states.ContainsKey(state.Name)) return;
			states.Add(state.Name, state);
		}

		public void RemoveState(State state)
		{
			if (state == null || !states.ContainsKey(state.Name)) return;
			states.Remove(state.Name);
		}

		public State GetState(string name)
		{
			if (!states.ContainsKey(name))
				return null;
			return states[name];
		}

		public void Update()
		{
			if (anyState != null)
			{
				anyState.Excute(this);
				anyState.Update(this);
			}

			if (curState != lastState)
			{
				if (lastState != null)
				{
					lastState.LeaveState(this);
				}

				if (anyState != null)
				{
					anyState.LeaveState(this);
				}

				lastState = curState;
				if (curState != null)
				{
					curState.EnterState(this);
				}

				if (anyState != null)
				{
					anyState.EnterState(this);
				}
			}
			if (curState != null && curState != anyState)
			{
				curState.Excute(this);
				curState.Update(this);
			}
		}

		public void LateUpdate()
		{
			if (anyState != null)
			{
				anyState.LateUpdate(this);
			}

			if (curState != null && curState != anyState)
			{
				curState.LateUpdate(this);
			}
		}

		public void FixedUpdate()
		{
			if (anyState != null)
			{
				anyState.FixedUpdate(this);
			}

			if (curState != null && curState != anyState)
			{
				curState.FixedUpdate(this);
			}
		}

		public void SetCurState(string stateName)
		{
			curState = GetState(stateName);
		}

		private void OnCurStateChangedEvent()
		{
			if (CurStateChangedEvent != null)
			{
				CurStateChangedEvent(this);
			}
		}

		private void OnParamsChangedEvent(ParamsChangedEvent args)
		{
			if (ParamsChangedEvent != null)
			{
				ParamsChangedEvent(this, args);
			}
		}

		public override string ToString()
		{
			return string.Format("fsm : {0}", name);
		}
	}
}
