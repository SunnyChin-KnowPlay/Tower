using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Plugins.Dream.FSM
{
	public class Transition
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
		}

		private List<Pipeline> pipelines = new List<Pipeline>();
		public List<Pipeline> Pipelines
		{
			get { return pipelines; }
		}

		public Transition(string srcStateName, string targetStateName)
		{
			this.srcStateName = srcStateName;
			this.targetStateName = targetStateName;
		}

		public List<Pipeline> GetPipeline(string srcStateName, string targetStateName)
		{
			var result = new List<Pipeline>();
			for (int i = 0, len = pipelines.Count; i < len; i++)
			{
				var t = pipelines[i];
				if (t.SrcStateName == srcStateName && t.TargetStateName == targetStateName)
					result.Add(t);
			}
			return result;
		}

		public void AddPipeline(Pipeline pipeline)
		{
			if (pipelines.Contains(pipeline)) return;
			pipelines.Add(pipeline);
		}

		public void AddPipelines(List<Pipeline> pipelines)
		{
			foreach (var p in pipelines)
				AddPipeline(p);
		}

		public void RemovePipeline(Pipeline pipeline)
		{
			if (!pipelines.Contains(pipeline)) return;
			pipelines.Remove(pipeline);
		}

		public void Update(FiniteStateMachine fsm)
		{
			foreach (var p in Pipelines)
			{
				if (p.Check(fsm))
				{
					fsm.SetCurState(p.TargetStateName);
					return;
				}
			}
		}

		public void ClearPipelines()
		{
			pipelines.Clear();
		}
	}
}
