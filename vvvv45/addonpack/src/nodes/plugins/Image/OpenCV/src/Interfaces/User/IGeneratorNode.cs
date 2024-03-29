﻿using System;
using System.Collections.Generic;
using System.Text;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes.OpenCV
{
	public abstract class IGeneratorNode<T> : INode, IDisposable where T : IGeneratorInstance, new()
	{
		[Input("Timestamp delay ms", Visibility = PinVisibility.OnlyInspector)]
		private IDiffSpread<int> FPinInTimestampDelay;

		[Input("Enabled", DefaultValue=0)]
		private ISpread<bool> FPinInEnabled;

		[Output("Output", Order = -1)]
		private ISpread<CVImageLink> FPinOutOutput;

		[Output("Status")]
		private ISpread<string> FPinOutStatus;

		protected ProcessGenerator<T> FProcessor;

		public override void Evaluate(int SpreadMax)
		{
			if (FProcessor == null)
				FProcessor = new ProcessGenerator<T>(FPinOutOutput);

			bool changed = FProcessor.CheckInputSize(SpreadMax);
			for (int i = 0; i < SpreadMax; i++)
			{
				FProcessor[i].TimestampDelay = FPinInTimestampDelay[i];
				FProcessor[i].Enabled = FPinInEnabled[i];
			}

			Update(FProcessor.SliceCount, changed);

			FPinOutStatus.SliceCount = SpreadMax;
			for (int i = 0; i < SpreadMax; i++)
				FPinOutStatus[i] = FProcessor[i].Status;
		}

		public void Dispose()
		{
			// sometimes we get a double dispose from vvvv on quit
			if (FProcessor != null)
				FProcessor.Dispose();
		}
	}
}
