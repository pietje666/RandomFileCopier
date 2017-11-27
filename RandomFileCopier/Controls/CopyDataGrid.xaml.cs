using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RandomFileCopier.Controls
{
    /// <summary>
    /// Interaction logic for CopyDataGrid.xaml
    /// </summary>
    public partial class CopyDataGrid 
        : DataGrid
    {
        public CopyDataGrid()
        {
            InitializeComponent();
            BindingOperations.SetBinding(SelectedColumn, DataGridColumn.WidthProperty, new Binding
            {
                Path = new PropertyPath(SelectedColumnWidthProperty),
                Source = this,
            });
            BindingOperations.SetBinding(FileNameColumn, DataGridColumn.WidthProperty, new Binding
            {
                Path = new PropertyPath(FileNameColumnWidthProperty),
                Source = this,
            });
            BindingOperations.SetBinding(PathColumn, DataGridColumn.WidthProperty, new Binding
            {
                Path = new PropertyPath(PathColumnWidthProperty),
                Source = this,
            });
        }

        public static readonly DependencyProperty SelectedColumnWidthProperty = DependencyProperty.Register("SelectedColumnWidth", typeof(DataGridLength), typeof(CopyDataGrid), new FrameworkPropertyMetadata(new DataGridLength(10, DataGridLengthUnitType.Star)));

        public DataGridLength SelectedColumnWidth
        {
            get { return (DataGridLength)GetValue(SelectedColumnWidthProperty); }
            set { SetValue(SelectedColumnWidthProperty, value); }
        }

        public static readonly DependencyProperty FileNameColumnWidthProperty = DependencyProperty.Register("FileNameColumnWidth", typeof(DataGridLength), typeof(CopyDataGrid), new FrameworkPropertyMetadata(new DataGridLength(20, DataGridLengthUnitType.Star)));

        public DataGridLength FileNameColumnWidth
        {
            get { return (DataGridLength)GetValue(FileNameColumnWidthProperty); }
            set { SetValue(FileNameColumnWidthProperty, value); }
        }

        public static readonly DependencyProperty PathColumnWidthProperty = DependencyProperty.Register("PathColumnWidth", typeof(DataGridLength), typeof(CopyDataGrid), new FrameworkPropertyMetadata(new DataGridLength(25, DataGridLengthUnitType.Star)));

        public DataGridLength PathColumnWidth
        {
            get { return (DataGridLength)GetValue(PathColumnWidthProperty); }
            set { SetValue(PathColumnWidthProperty, value); }
        }
    }
}
