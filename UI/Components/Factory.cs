﻿using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.UI.Components
{
	internal class Factory : IComponentFactory
	{
		public string ComponentName => "Subnautica Ship Explosion Info";
		public string Description => "";
		public ComponentCategory Category => ComponentCategory.Information;
		public string UpdateName => ComponentName;
		public string XMLURL => "";
		public string UpdateURL => "";
		public Version Version => Version.Parse("0.0.1");

		public IComponent Create(LiveSplitState state)
		{
			return new Component(state);
		}
	}
}
