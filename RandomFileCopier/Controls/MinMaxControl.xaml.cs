using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace RandomFileCopier.Controls
{
    /// <summary>
    /// Interaction logic for MinMaxControl.xaml
    /// </summary>
    public partial class MinMaxControl
        : UserControl
    {
        public MinMaxControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(MinMaxControl), new FrameworkPropertyMetadata());
        public static readonly DependencyProperty SizeUnitProperty = DependencyProperty.Register("SizeUnit", typeof(string), typeof(MinMaxControl), new FrameworkPropertyMetadata());
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(double), typeof(MinMaxControl), new FrameworkPropertyMetadata(null, MinCoerceCheck) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged });
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(double), typeof(MinMaxControl), new FrameworkPropertyMetadata(null, MaxCoerceCheck) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged });
        public static readonly DependencyProperty MaxSliderValueProperty = DependencyProperty.Register("MaxSliderValue", typeof(int), typeof(MinMaxControl), new FrameworkPropertyMetadata(0));
        private bool _dragStarted;

        private static object MinCoerceCheck(DependencyObject d, object baseValue)
        {
            var control = (MinMaxControl)d;
            var returnValue = baseValue;
            
            if (BindingOperations.IsDataBound(control, MaxValueProperty) && (double)baseValue > control.MaxValue)
            {
                returnValue = control.MaxValue;
            }
            return returnValue;
        }

        private static object MaxCoerceCheck(DependencyObject d, object baseValue)
        {
            var control = (MinMaxControl)d;
            var returnValue = baseValue;
            if (BindingOperations.IsDataBound(control, MinValueProperty) && (double)baseValue < control.MinValue)
            {
                returnValue = control.MinValue;
            }
            return returnValue;
        }
                
        public int MaxSliderValue
        {
            get { return (int)GetValue(MaxSliderValueProperty); }
            set { SetValue(MaxSliderValueProperty, value); }
        }

        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set{ SetValue(MinValueProperty, value); }
        }

        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public double SizeUnit
        {
            get { return (double)GetValue(SizeUnitProperty); }
            set { SetValue(SizeUnitProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_dragStarted && Command != null && IsLoaded)
            {
                Command.Execute(null);
            }
        }

        private void Slider_DragCompleted(object sender, RoutedEventArgs e)
        {
            Command.Execute(null);
            _dragStarted = false;
        }

        private void Slider_DragStarted(object sender, RoutedEventArgs e)
        {
            _dragStarted = true;
        }
    }
}
