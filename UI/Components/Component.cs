﻿using LiveSplit.ComponentUtil;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Timers;
using System.Xml;

namespace LiveSplit.UI.Components
{
	public class Component : IComponent
	{

		protected InfoTextComponent InternalComponent { get; set; }

		private Timer infoUpdateTimer = new Timer(2500);

		public string ComponentName => "Subnautica Ship Explosion Info";
		public float HorizontalWidth => InternalComponent.HorizontalWidth;
		public float MinimumHeight => InternalComponent.MinimumHeight;
		public float VerticalHeight => InternalComponent.VerticalHeight;
		public float MinimumWidth => InternalComponent.MinimumWidth;
		public float PaddingTop => InternalComponent.PaddingTop;
		public float PaddingBottom => InternalComponent.PaddingBottom;
		public float PaddingLeft => InternalComponent.PaddingLeft;
		public float PaddingRight => InternalComponent.PaddingRight;

		public IDictionary<string, Action> ContextMenuControls => null;

		public Component(LiveSplitState state)
		{
			InternalComponent = new InfoTextComponent("Explosion Time", "0:00:00");

			infoUpdateTimer.Elapsed += (sender, e) => UpdateInfo();
			infoUpdateTimer.Start();
		}

		public void Dispose()
		{
			InternalComponent.Dispose();
		}

		public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
		{
			InternalComponent.NameLabel.HasShadow
			= InternalComponent.ValueLabel.HasShadow
			= state.LayoutSettings.DropShadows;

			InternalComponent.NameLabel.ForeColor = state.LayoutSettings.TextColor;
			InternalComponent.ValueLabel.ForeColor = state.LayoutSettings.TextColor;

			InternalComponent.DrawHorizontal(g, state, height, clipRegion);
		}

		public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
		{
			InternalComponent.NameLabel.HasShadow
			= InternalComponent.ValueLabel.HasShadow
			= state.LayoutSettings.DropShadows;

			InternalComponent.NameLabel.ForeColor = state.LayoutSettings.TextColor;
			InternalComponent.ValueLabel.ForeColor = state.LayoutSettings.TextColor;

			InternalComponent.DrawVertical(g, state, width, clipRegion);
		}

		public XmlNode GetSettings(XmlDocument document)
		{
			return document.CreateElement("Settings");
		}

		public System.Windows.Forms.Control GetSettingsControl(LayoutMode mode)
		{
			return new SubnauticaShipExplosionInfo.Settings();
		}

		public void SetSettings(XmlNode settings)
		{
			
		}

		public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
		{
			InternalComponent.Update(invalidator, state, width, height, mode);
		}

		private Process gameProcess;
		private DeepPointer timeToStartCountdownPtr = new DeepPointer("UnityPlayer.dll", 0x1847f40, 0xe0, 0x510, 0x40, 0x4e0, 0x1940, 0xd0, 0x8, 0x70, 0x0, 0x84);
		private DeepPointer timeToStartWarningPtr = new DeepPointer("UnityPlayer.dll", 0x1847f40, 0xe0, 0x510, 0x40, 0x4e0, 0x1940, 0xd0, 0x8, 0x70, 0x0, 0x90);

		private void UpdateInfo()
		{
			gameProcess = Process.GetProcessesByName("Subnautica").FirstOrDefault(p => !p.HasExited);
			//Debug.WriteLine($"found Subnautica at pid {gameProcess.Id}");
			if (gameProcess == null) { return; }
			
			float countdownTime = timeToStartCountdownPtr.Deref<float>(gameProcess);
			float warningTime = timeToStartWarningPtr.Deref<float>(gameProcess);
			float explosionTimeFloat = countdownTime - warningTime;

			TimeSpan explosionTime = TimeSpan.FromSeconds(explosionTimeFloat);
			//Debug.WriteLine($"{InternalComponent.InformationValue}");
			InternalComponent.InformationValue = explosionTime.ToString(@"h\:mm\:ss");
		}
	}
}
