# How to configure CustomEditor in WPF PropertyGrid
## Overview 
The PropertyGrid is a powerful UI component that allows users to inspect and edit the properties of objects at runtime. However, its default behavior may not always meet the specific needs of developers who require more control over how properties are displayed and edited.

This repository sample demonstrates how to configure custom value editors (CustomEditor) for specific properties in the Syncfusion WPF PropertyGrid. By registering a custom editor, you can replace default editors (for example, TextBox) with tailored UI such as ComboBox, DatePicker, or any custom control. 

By using this approach, developers can create more flexible and user-friendly interfaces, especially in scenarios where complex data types or specialized input methods are involved. The article also includes code samples and practical tips to help developers implement this feature effectively.

## XAML (MainWindow.xaml)

```XAML
   <!--Command which used to load AddressCustomEditor at load time-->
    <ie:Interaction.Triggers>
        <ie:EventTrigger EventName="Loaded">
            <ie:InvokeCommandAction Command="{Binding WindowLoadedCommand}" CommandParameter="{Binding ElementName=window}"/>
        </ie:EventTrigger>
    </ie:Interaction.Triggers>
    <Window.DataContext>
        <local:ViewModel/>
    </Window.DataContext>
   
    <Grid Name="grid">
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="*"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>
        <Grid.RowDefinitions>
            <RowDefinition Height="30"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        <TextBlock FontWeight="Bold" Margin="3" Text="Select any of the item to show the properties in the PropertyGrid" Grid.ColumnSpan="2"/>

        <!--Command which is used to load PropertyGrid at selection of item--> 
        <syncfusion:SfDataGrid  x:Name="Listview" ItemsSource="{Binding Collection}" Grid.Row="1" Margin="1" >
            <ie:Interaction.Triggers>
                <ie:EventTrigger EventName="SelectionChanged">
                    <ie:InvokeCommandAction Command="{Binding SelectedItemChangedCommand}"  CommandParameter="{Binding ElementName=Listview}"/>
                </ie:EventTrigger>
            </ie:Interaction.Triggers>

            <syncfusion:SfDataGrid.Columns>
                <syncfusion:GridTextColumn MappingName="Name" DisplayBinding="{Binding Name}" Width="115"/>
                <syncfusion:GridTextColumn MappingName="Street" DisplayBinding="{Binding Address.Street}" Width="115"/>
                <syncfusion:GridTextColumn MappingName="City" DisplayBinding="{Binding Address.Region}" Width="115"/>
                <syncfusion:GridTextColumn MappingName="Country" DisplayBinding="{Binding Address.Country}" Width="115"/>
            </syncfusion:SfDataGrid.Columns>
        </syncfusion:SfDataGrid>
    </Grid>
```

## Custom editor for Address property
```C#
    /// <summary>
    /// Represents custom editor class for address.
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
    public class ViewModel
    {
        private ObservableCollection<Model> _collection;
        /// <summary>
        /// Collection of Model
        /// </summary>
        public ObservableCollection<Model> Collection
        {
            get { return _collection; }
            set { _collection = value; }
        }

        public PropertyGrid propertygrid = new PropertyGrid();

        /// <summary>
        /// Constructor of the ViewModel
        /// </summary>
        public ViewModel()
        {
            GetCollection();
            ShowProperties();
        }

        /// <summary>
        /// To show the properties in PropertyGrid
        /// </summary>
        void ShowProperties()
        {
            WindowLoadedCommand = new DelegateCommand<object>((window) =>
            {
                CustomEditor Addresseditor = new CustomEditor() { HasPropertyType = true, PropertyType = typeof(Address), Editor = new AddressEditor() };
                propertygrid.CustomEditorCollection.Add(Addresseditor);
                propertygrid.RefreshPropertygrid();
            });

            SelectedItemChangedCommand = new DelegateCommand<object>((Listview) =>
            {
                propertygrid.ButtonPanelVisibility = Visibility.Visible;
                propertygrid.VerticalAlignment = VerticalAlignment.Stretch;
                propertygrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                propertygrid.Width = 435;
                propertygrid.EnableGrouping = true;
                propertygrid.HorizontalAlignment = HorizontalAlignment.Center;
                propertygrid.PropertyExpandMode = PropertyExpandModes.NestedMode;
                propertygrid.Margin = new Thickness(2);
                propertygrid.DescriptionPanelVisibility = Visibility.Visible;
                propertygrid.DescriptionPanelHeight = new GridLength(100);

                
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
        /// SelectedItemChanged Command
        /// </summary>
        public DelegateCommand<object> SelectedItemChangedCommand { get; set; }

        public DelegateCommand<object> WindowLoadedCommand { get; set; }

        /// <summary>
        /// Get the collection of Model
        /// </summary>
        void GetCollection()
        {
            Collection = new ObservableCollection<Model>();
            Collection.Add(new Model() { Name = "Mark", Age = 27,  DateOfBirth = new DateTime(1991, 10, 10), Email = "Cust2010@gmail.com", Sex = Sex.Male });
            Collection.Add(new Model() { Name = "Jean", Age = 28,  DateOfBirth = new DateTime(1990, 10, 11), Email = "Cust2011@gmail.com", Sex = Sex.Male });
            Collection.Add(new Model() { Name = "Pant", Age = 29,  DateOfBirth = new DateTime(1989, 10, 01), Email = "Cust2012@gmail.com", Sex = Sex.Male });
            Collection.Add(new Model() { Name = "Perk", Age = 24,  DateOfBirth = new DateTime(1992, 10, 02), Email = "Cust2013@gmail.com", Sex = Sex.Female });
            Collection.Add(new Model() { Name = "Jaso", Age = 23,  DateOfBirth = new DateTime(1994, 10, 03), Email = "Cust2014@gmail.com", Sex = Sex.Male });
        }
    }
```
