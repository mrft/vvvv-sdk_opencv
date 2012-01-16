﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes.OpenCV
{
	public abstract class IFilterNode<T> : IPluginEvaluate, IDisposable where T : IFilterInstance, new()
	{
		[Input("Input", Order = -1)]
		private ISpread<CVImageLink> FInput;

		[Output("Output", Order = -1)]
		private ISpread<CVImageLink> FOutput;

		protected ProcessFilter<T> FProcessor;

		public void Evaluate(int SpreadMax)
		{
			if (FProcessor == null)
				FProcessor = new ProcessFilter<T>(FInput, FOutput);

			FProcessor.CheckInputSize(SpreadMax);

			Update(FProcessor.SliceCount);
		}

		protected abstract void Update(int InstanceCount);

		public void Dispose()
		{
			FProcessor.Dispose();
		}
	}
}
