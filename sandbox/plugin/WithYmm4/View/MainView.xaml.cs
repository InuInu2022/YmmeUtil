using System.Windows.Controls;

namespace YmmeUtil.Sandbox.View;

/// <summary>
/// Interaction logic for UserControl1.xaml
/// </summary>
public partial class MainView : UserControl
{
	public MainView()
	{
		InitializeComponent();

		this.Loaded += (s, e) =>
		{
			if (this.DataContext is MainViewModel vm)
				vm.AttachView(this);
		};
	}
}
