using Syncfusion.UI.Xaml.Grid;
using Syncfusion.Windows.PropertyGrid;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PropertyGridSample_CustomObject
{
    /// <summary>
    /// ViewModel class
    /// </summary>
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
}
