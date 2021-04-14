<!-- default file list -->
*Files to look at*:

* [MouseEventArgsConverter.cs](./CS/Common/MouseEventArgsConverter.cs) (VB: [MouseEventArgsConverter.vb](./VB/Common/MouseEventArgsConverter.vb))
* **[MainView.xaml](./CS/View/MainView.xaml) (VB: [MainView.xaml](./VB/View/MainView.xaml))**
* [MainViewModel.cs](./CS/ViewModel/MainViewModel.cs) (VB: [MainViewModel.vb](./VB/ViewModel/MainViewModel.vb))
<!-- default file list end -->
# How to: Use EventToCommand

The [EventToCommand](https://docs.devexpress.com/WPF/DevExpress.Mvvm.UI.EventToCommand) class is a **Behavior** that allows you to bind an event to a command. This way, the bound command is invoked like an event handler when the event is raised.

This example demonstrates how to use the EventToCommand:

Assume that there is a **ListBox** control that displays data. When an end user clicks an item in the **ListBox**, it is necessary to show an edit form.

To accomplish this task, you can use the **EventToCommand** behavior. Place the **EventToCommand** into the **Interaction.Behaviors** collection for the **ListBox** control and customize it as shown below.

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

This code defines an **EventToCommand** that processes the **MouseDoubleClick** event for the **ListBox**. When the event is raised, the **EventToCommand** invokes the bound **EditCommand**. This command requires a parameter: a **Person** object to be edited.

The code below shows the **EditCommand** implementation code both in a [Runtime POCO ViewModel](https://docs.devexpress.com/WPF/17352/mvvm-framework/viewmodels/poco-viewmodels) and a common ViewModel.


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

In this case, it is necessary to take the clicked **ListBoxItem** and obtain its **DataContext** - this is what the **EventToCommand** should pass to the **EditCommand** as a parameter. This operation is performed by the custom **ListBoxEventArgsConverter**. Its code is shown below.

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

The **ListBoxEventArgsConverter** class inherits from the **EventArgsConverterBase** class and contains **Convert** method, which is used by the **EventToCommand** for conversion event arguments.

In this scenario, the **EventToCommand** passes a **MouseEventArgs** object to the **ListBoxEventArgsConverter**. The converter finds the clicked **ListBoxItem** using the [](https://docs.devexpress.com/WPF/17673/mvvm-framework/layouttreehelper) class and returns its **DataContext**, which contains an underlying **Person** object. The resulting **Person** object is then passed to the bound **EditCommand**.
