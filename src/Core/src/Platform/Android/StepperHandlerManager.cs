﻿using System.ComponentModel;
using Android.Views;
using AButton = Android.Widget.Button;
using AView = Android.Views.View;

namespace Microsoft.Maui
{
	public interface IStepperHandler
	{
		AButton? UpButton { get; }

		AButton? DownButton { get; }

		AButton CreateButton();

		IStepper? VirtualView { get; }
	}

	public class StepperHandlerHolder : Java.Lang.Object
	{
		public StepperHandlerHolder(IStepperHandler handler)
		{
			StepperHandler = handler;
		}

		public IStepperHandler StepperHandler { get; set; }
	}

	public static class StepperHandlerManager
	{
		public static void CreateStepperButtons<TButton>(IStepperHandler handler, out TButton? downButton, out TButton? upButton)
			where TButton : AButton
		{
			downButton = (TButton)handler.CreateButton();
			downButton.Focusable = true;

			upButton = (TButton)handler.CreateButton();
			upButton.Focusable = true;

			downButton.Gravity = GravityFlags.Center;
			downButton.Tag = new StepperHandlerHolder(handler);
			downButton.SetOnClickListener(StepperListener.Instance);

			upButton.Gravity = GravityFlags.Center;
			upButton.Tag = new StepperHandlerHolder(handler);
			upButton.SetOnClickListener(StepperListener.Instance);

			// IMPORTANT:
			// Do not be decieved. These are NOT the same characters. Neither are a "minus" either.
			// The Text is a visually pleasing "minus", and the description is the phonetically correct "minus".
			// The little key on your keyboard is a dash/hyphen.
			downButton.Text = "－";
			downButton.ContentDescription = "−";

			// IMPORTANT:
			// Do not be decieved. These are NOT the same characters.
			// The Text is a visually pleasing "plus", and the description is the phonetically correct "plus"
			// (which, unlike the minus, IS found on your keyboard).
			upButton.Text = "＋";
			upButton.ContentDescription = "+";

			downButton.NextFocusForwardId = upButton.Id;
		}

		public static void UpdateButtons<TButton>(IStepper stepper, TButton? downButton, TButton? upButton, PropertyChangedEventArgs? e = null)
			where TButton : AButton
		{
			// NOTE: a value of `null` means that we are forcing an update
			if (downButton != null)
				downButton.Enabled = stepper.IsEnabled && stepper.Value > stepper.Minimum;

			if (upButton != null)
				upButton.Enabled = stepper.IsEnabled && stepper.Value < stepper.Maximum;
		}

		class StepperListener : Java.Lang.Object, AView.IOnClickListener
		{
			public static readonly StepperListener Instance = new StepperListener();

			public void OnClick(AView? view)
			{
				if (!(view?.Tag is StepperHandlerHolder HandlerHolder))
					return;

				if (!(HandlerHolder.StepperHandler?.VirtualView is IStepper stepper))
					return;

				var increment = stepper.Step;

				if (view == HandlerHolder.StepperHandler.DownButton)
					increment = -increment;

				HandlerHolder.StepperHandler.VirtualView.Value = stepper.Value + increment;
				UpdateButtons(HandlerHolder.StepperHandler.VirtualView, HandlerHolder.StepperHandler.DownButton, HandlerHolder.StepperHandler.UpButton);
			}
		}
	}
}