using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Plugins.Dream.FSM
{
	public abstract class State
	{
		public delegate void OnUpdateHandle(FiniteStateMachine fsm);

		public event OnUpdateHandle EnterEvent;
		public event OnUpdateHandle ExitEvent;
		public event OnUpdateHandle UpdateEvent;
		public event OnUpdateHandle LateUpdateEvent;
		public event OnUpdateHandle FixedUpdateEvent;

		private List<Transition> transitions = new List<Transition>();
		public List<Transition> Transitions
		{
			get { return transitions; }
		}

		private bool needUpdate = true;

		private string name;
		public string Name
		{
			get { return name; }
		}

		protected bool isInited = false;
		public bool IsInited
		{
			get { return isInited; }
		}

		public State(string name)
		{
			this.name = name;
		}

		public List<Transition> GetTransition(string srcStateName, string targetStateName)
		{
			var result = new List<Transition>();
			for (int i = 0, len = transitions.Count; i < len; i++)
			{
				var t = transitions[i];
				if (t.SrcStateName == srcStateName && t.TargetStateName == targetStateName)
					result.Add(t);
			}
			return result;
		}

		public void AddTransition(Transition value)
		{
			if (transitions.Contains(value))
				return;
			transitions.Add(value);
		}

		public void RemoveTransition(Transition value)
		{
			if (!transitions.Contains(value))
				return;
			transitions.Remove(value);
		}

		public override string ToString()
		{
			return string.Format("state name : {0}", name);
		}

		/// <summary>
		/// 如果调用之后，就会对Update执行一次
		/// </summary>
		public void DirtyState()
		{
			needUpdate = true;
		}

		public void Excute(FiniteStateMachine fsm)
		{
			if (!isInited)
			{
				isInited = true;
				Initialize(fsm);
			}

			if (!needUpdate) return;
			needUpdate = false;

			foreach (var t in Transitions)
			{
				t.Update(fsm);
			}
		}

		/// <summary>
		/// First time to Update will trigger
		/// </summary>
		protected abstract void Initialize(FiniteStateMachine fsm);

		public virtual void EnterState(FiniteStateMachine fsm)
		{
			needUpdate = true;

			if (EnterEvent != null)
			{
				EnterEvent.Invoke(fsm);
			}
		}

		public virtual void LeaveState(FiniteStateMachine fsm)
		{
			if (ExitEvent != null)
			{
				ExitEvent.Invoke(fsm);
			}
		}

		public virtual void Update(FiniteStateMachine fsm)
		{
			if (UpdateEvent != null)
			{
				UpdateEvent.Invoke(fsm);
			}
		}

		public virtual void LateUpdate(FiniteStateMachine fsm)
		{
			if (LateUpdateEvent != null)
			{
				LateUpdateEvent.Invoke(fsm);
			}
		}

		public virtual void FixedUpdate(FiniteStateMachine fsm)
		{
			if (FixedUpdateEvent != null)
			{
				FixedUpdateEvent.Invoke(fsm);
			}
		}
	}
}
