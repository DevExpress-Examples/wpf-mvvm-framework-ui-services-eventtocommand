<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128658245/21.1.5%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T142075)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
# Use the EventToCommand Behavior to Execute Commands when an Event is Raised

The [EventToCommand](https://docs.devexpress.com/WPF/DevExpress.Mvvm.UI.EventToCommand) [behavior](https://docs.devexpress.com/WPF/17442/mvvm-framework/behaviors) allows you to bind an event to a command. When used, the bound command is invoked like an event handler when the event is raised.

In this example, the [EventToCommand](https://docs.devexpress.com/WPF/DevExpress.Mvvm.UI.EventToCommand) is used (to call an event) to display an edit form when a user clicks an item in the [ListBox](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.listbox).

The code snippet below defines the [EventToCommand](https://docs.devexpress.com/WPF/DevExpress.Mvvm.UI.EventToCommand) that processes the [ListBox](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.listbox)'s [MouseDoubleClick](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.control.mousedoubleclick) event. When the event is raised, the [EventToCommand](https://docs.devexpress.com/WPF/DevExpress.Mvvm.UI.EventToCommand) invokes the bound **EditCommand**. This command requires a parameter: a **Person** object to be edited.

```xaml
<UserControl x:Class="Example.View.MainView" ...
    xmlns:ViewModel="clr-namespace:Example.ViewModel"
    xmlns:Common="clr-namespace:Example.Common"
    xmlns:dxmvvm="http://schemas.devexpress.com/winfx/2008/xaml/mvvm"
    DataContext="{dxmvvm:ViewModelSource Type=ViewModel:MainViewModel}">
    <Grid x:Name="LayoutRoot" Background="White">
        <ListBox ItemsSource="{Binding Persons}">
            <dxmvvm:Interaction.Behaviors>
                <dxmvvm:EventToCommand EventName="MouseDoubleClick" Command="{Binding EditCommand}">
                    <dxmvvm:EventToCommand.EventArgsConverter>
                        <Common:ListBoxEventArgsConverter/>
                    </dxmvvm:EventToCommand.EventArgsConverter>
                </dxmvvm:EventToCommand>
            </dxmvvm:Interaction.Behaviors>
            <ListBox.ItemTemplate>
                ...
            </ListBox.ItemTemplate>
        </ListBox>
    </Grid>
</UserControl>
```

The code below illustrates an implementation of the **EditCommand** in both the [Runtime POCO ViewModel](https://docs.devexpress.com/WPF/17352/mvvm-framework/viewmodels/poco-viewmodels) and the common ViewModel.


```csharp
//POCO ViewModel
[POCOViewModel]
public class MainViewModel {
	public void Edit(Person person) {
		...
	}
	public bool CanEdit(Person person) {
		return person != null;
	}
}
//Common ViewModel
public class MainViewModel {
	public ICommand<Person> EditCommand { get; private set; }
	public MainViewModel() {
		EditCommand = new DelegateCommand<Person>(Edit, CanEdit);
	}
	public void Edit(Person person) {
		... 
	}
	public bool CanEdit(Person person) {
		return person != null;
	}
}
```

In this specific instance, you must obtain the **DataContext** for the clicked [ListBoxItem](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.listboxitem). This is what the [EventToCommand](https://docs.devexpress.com/WPF/DevExpress.Mvvm.UI.EventToCommand) should pass to the **EditCommand** as a parameter. This operation is performed by the custom **ListBoxEventArgsConverter**:

```csharp
using DevExpress.Mvvm.UI;
using System.Linq;
public class ListBoxEventArgsConverter : EventArgsConverterBase<MouseEventArgs> {
    protected override object Convert(object sender, MouseEventArgs args) {
        ListBox parentElement = (ListBox)sender;
        DependencyObject clickedElement = (DependencyObject)args.OriginalSource;
        ListBoxItem clickedListBoxItem = 
            LayoutTreeHelper.GetVisualParents(child: clickedElement, stopNode: parentElement)
            .OfType<ListBoxItem>()
            .FirstOrDefault();
        if(clickedListBoxItem != null)
            return (Person)clickedListBoxItem.DataContext;
        return null;
    }
}
```

The **ListBoxEventArgsConverter** class inherits from the **EventArgsConverterBase** class and contains the **Convert** method. The [EventToCommand](https://docs.devexpress.com/WPF/DevExpress.Mvvm.UI.EventToCommand) uses this method to convert event arguments.

In this scenario, the [EventToCommand](https://docs.devexpress.com/WPF/DevExpress.Mvvm.UI.EventToCommand) passes a [MouseEventArgs](https://docs.microsoft.com/en-us/dotnet/api/system.windows.input.mouseeventargs) object to the **ListBoxEventArgsConverter**. The converter uses the [LayoutTreeHelper](https://docs.devexpress.com/WPF/17673/mvvm-framework/layouttreehelper) class to locate the clicked **ListBoxItem** and then returns its **DataContext** (which contains the underlying **Person** object). The resulting **Person** object is then passed to the bound **EditCommand**.


<!-- default file list -->
## Files to Look At

* [MouseEventArgsConverter.cs](./CS/Common/MouseEventArgsConverter.cs) (VB: [MouseEventArgsConverter.vb](./VB/Common/MouseEventArgsConverter.vb))
* **[MainView.xaml](./CS/View/MainView.xaml) (VB: [MainView.xaml](./VB/View/MainView.xaml))**
* [MainViewModel.cs](./CS/ViewModel/MainViewModel.cs) (VB: [MainViewModel.vb](./VB/ViewModel/MainViewModel.vb))
<!-- default file list end -->

## Documentation

* [EventToCommand Behavior](https://docs.devexpress.com/WPF/DevExpress.Mvvm.UI.EventToCommand)
* [Behaviors](https://docs.devexpress.com/WPF/17442/mvvm-framework/behaviors)
<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=wpf-mvvm-framework-ui-services-eventtocommand&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=wpf-mvvm-framework-ui-services-eventtocommand&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
