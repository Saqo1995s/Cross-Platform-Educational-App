using System.Windows.Input;
using Xamarin.Forms;

namespace LearningPlatform.Behaviors
{
    public class SearchBehavior : BindableBehavior<SearchBar>
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(
                nameof(Command), 
                typeof(ICommand), 
                typeof(SearchBehavior));

        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        protected override void OnAttachedTo(SearchBar visualElement)
        {
            base.OnAttachedTo(visualElement);
            AssociatedObject.TextChanged += SearchBarOnTextChanged;
        }

        private void SearchBarOnTextChanged(object sender, TextChangedEventArgs e)
        {
            Command?.Execute(null);
        }
    }
}