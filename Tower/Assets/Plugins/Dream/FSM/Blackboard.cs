using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Plugins.Dream.FSM
{
	/// <summary>
	/// 黑板数据的更新类型
	/// </summary>
	public enum BlackboardUpdateType
	{
		Add,
		Remove,
		Modify
	}

	/// <summary>
	/// FSM中的黑板数据，便于，绑定外部数据，方便在FSM中对外部数据的获取与判断而使用
	/// 有点像BT（Behavior Tree）中的 Black board而设计
	/// </summary>
	public class Blackboard
	{
		private Dictionary<string, object> datasStore = new Dictionary<string, object>();

		public event Action<Blackboard, BlackboardUpdateType> UpdateEvent;

		private void OnUpdateEvent(BlackboardUpdateType updateType)
		{
			if (UpdateEvent != null)
			{
				UpdateEvent(this, updateType);
			}
		}

		public bool HaveDatas
		{
			get
			{
				return datasStore.Keys.Count > 0;
			}
		}

		public T GetData<T>(string name)
		{
			return (T)datasStore[name];
		}

		public void AddData(string name, object data)
		{
			var srcHad = datasStore.ContainsKey(name);
			datasStore[name] = data;
			OnUpdateEvent(srcHad ? BlackboardUpdateType.Modify : BlackboardUpdateType.Add);
		}

		public object RemoveData(string name)
		{
			if (!datasStore.ContainsKey(name))
				return null;
			object result = datasStore[name];
			datasStore.Remove(name);
			OnUpdateEvent(BlackboardUpdateType.Remove);
			return result;
		}

		public void UpdateData(string name)
		{
			if (!datasStore.ContainsKey(name))
				return;
			OnUpdateEvent(BlackboardUpdateType.Modify);
		}
	}
}
