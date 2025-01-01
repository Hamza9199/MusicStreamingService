using System.Windows.Input;

namespace MusicStreamingService.Behaviors
{
	public class EventToCommandBehavior : Behavior<Slider>
	{
		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(EventToCommandBehavior));

		public static readonly BindableProperty CommandParameterProperty =
			BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(EventToCommandBehavior));

		public ICommand Command
		{
			get => (ICommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		public object CommandParameter
		{
			get => GetValue(CommandParameterProperty);
			set => SetValue(CommandParameterProperty, value);
		}

		protected override void OnAttachedTo(Slider bindable)
		{
			base.OnAttachedTo(bindable);
			bindable.ValueChanged += OnValueChanged;
		}

		protected override void OnDetachingFrom(Slider bindable)
		{
			base.OnDetachingFrom(bindable);
			bindable.ValueChanged -= OnValueChanged;
		}

		private void OnValueChanged(object sender, ValueChangedEventArgs e)
		{
			if (Command == null)
				return;

			var parameter = CommandParameter ?? e.NewValue;
			if (Command.CanExecute(parameter))
			{
				Command.Execute(parameter);
			}
		}
	}

}
