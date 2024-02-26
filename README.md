# How to configure Custom Editors in WPF PropertyGrid?
 

## CustomEditor support of PropertyGrid 
In the PropertyGrid control, the [CustomEditor](https://help.syncfusion.com/wpf/propertygrid/customeditor-support) support enables you set different custom value editors for properties instead of default editors such as TextBox to display string and DoubleTextBox to display integer values.

The following code example demonstrates how to customize the selected object of PropertyGrid and assign different values of property for one business object using CustomEditor at run time.

## MainWindow.xaml

```XAML
<!--Command used to assign AddressCustomEditor at load time-->
    <ie:Interaction.Triggers>
        <ie:EventTrigger EventName="Loaded">
            <ie:InvokeCommandAction Command="{Binding WindowLoadedCommand}" CommandParameter="{Binding ElementName=window}"/>
        </ie:EventTrigger>
    </ie:Interaction.Triggers>
    <Window.DataContext>
        <local:ViewModel/>
    </Window.DataContext>
 
 
<!--Command used to load PropertyGrid at selection of item--> 
        <syncfusion:SfDataGrid  x:Name="Listview" ItemsSource="{Binding Collection}" Grid.Row="1" Margin="1" >
            <ie:Interaction.Triggers>
                <ie:EventTrigger EventName="SelectionChanged">
                    <ie:InvokeCommandAction Command="{Binding SelectedItemChangedCommand}"  CommandParameter="{Binding ElementName=Listview}"/>
                </ie:EventTrigger>
            </ie:Interaction.Triggers> 
``` 
 

## Custom editor class for the Address property

```C#
ICustomTypeEditors.cs

    /// <summary>
    /// AddressEditor custom class custom editor class
    /// </summary>
    public class AddressEditor : ITypeEditor
    {
        public ObservableCollection<Address> address;
        ComboBox addressCombo;
 
        /// <summary>
        /// Represents the attach method
        /// </summary>
        public void Attach(PropertyViewItem property, PropertyItem info)
        {
 
            var binding = new Binding("Value")
            {
                Mode = BindingMode.TwoWay,
                Source = info,
                ValidatesOnExceptions = true,
                ValidatesOnDataErrors = true
            };
            BindingOperations.SetBinding(addressCombo, ListBox.SelectedItemProperty, binding);
        }
 
        public object Create(PropertyInfo propertyInfo)
        {
            addressCombo = new ComboBox()
            {
               
            };
            addressCombo.Loaded += new RoutedEventHandler(addressCombo_Loaded);
            return addressCombo;
        }
 
        /// <summary>
        /// Loaded the address of the customer
        /// </summary>
        void addressCombo_Loaded(object sender, RoutedEventArgs e)
        {
            address = new ObservableCollection<Address>();
            address.Add(new Address() { AddressName = "Address 1" ,Country = "India", Region = "Chennai", Street = "Godown Street" });
            address.Add(new Address() { AddressName = "Address 2", Country = "USA", Region = "New York", Street = "Bourbon Street" });
            address.Add(new Address() { AddressName = "Address 3", Country = "UK", Region = "London", Street = "Oxford Street" });
            address.Add(new Address() { AddressName = "Address 4", Country = "Australia", Region = "Sydney", Street = "Elizabeth Street" });
            addressCombo.ItemsSource = address;
            addressCombo.DisplayMemberPath = "AddressName";
 
        }
 
        /// <summary>
        /// Represents the detach method
        /// </summary>
        public void Detach(PropertyViewItem property)
        {
            
        }
    }
``` 

## ViewModel.cs

```C#

        /// <summary>
        /// Code used to show PropertyGrid on selecting DataGrid.
        /// </summary>
        void ShowProperties()
        {
<! --Command used to assign AddressCustomEditor at load time-->
 
            WindowLoadedCommand = new DelegateCommand<object>((window) =>
            {
                CustomEditor Addresseditor = new CustomEditor() { HasPropertyType = true, PropertyType = typeof(Address), Editor = new AddressEditor() };
                propertygrid.CustomEditorCollection.Add(Addresseditor);
                propertygrid.RefreshPropertygrid();
            });
 
<! --Command used to load PropertyGrid at selection of item-->
            SelectedItemChangedCommand = new DelegateCommand<object>((Listview) =>
            {   
                Grid grid = ((Listview as SfDataGrid).Parent as Grid);
                if (Listview != null)
                {
                    propertygrid.SelectedObject = (Listview as SfDataGrid).SelectedItem;
                }
                Grid.SetColumn(propertygrid, 1);
                Grid.SetRow(propertygrid, 1);
                if (!grid.Children.Contains(propertygrid))
                {
                    grid.Children.Add(propertygrid);
                }
            });
        }
 
        /// <summary>
        /// SelectedItemChanged command
        /// </summary>
        public DelegateCommand<object> SelectedItemChangedCommand { get; set; }
 
        public DelegateCommand<object> WindowLoadedCommand { get; set; }
``` 
      
 
 
