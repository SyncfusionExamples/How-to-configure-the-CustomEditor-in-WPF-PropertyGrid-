using Syncfusion.Windows.PropertyGrid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace PropertyGridSample_CustomObject
{
    /// <summary>
    /// Model class
    /// </summary>
    public class Model : INotifyPropertyChanged
    {
        private string _name;
        private int _age = 18;
        private DateTime _dateOfBirth = DateTime.Now;
        private string _email;
        private bool _frequentBuyer;
        private Sex sex;
        private Brush Preferedcolor;
        private Address _address;
        


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Gets od sets PrefferedColor
        /// </summary>
        [CategoryAttribute("Marketting Settings"), DescriptionAttribute("Purchase details of the customer")]
        public Brush PrefferedColor
        {
            get { return Preferedcolor; }
            set { Preferedcolor = value; NotifyPropertyChanged("Name"); }
        }

        /// <summary>
        /// Gets or sets name of the customer
        /// </summary>
        [CategoryAttribute("ID Settings"), DescriptionAttribute("Name of the customer")]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets or sets Date of Birth of the Customer
        /// </summary>
        [CategoryAttribute("ID Settings"), DescriptionAttribute("Date of Birth of the Customer (optional)")]
        public DateTime DateOfBirth
        {
            get
            {
                return _dateOfBirth;
            }
            set
            {
                _dateOfBirth = value;
                NotifyPropertyChanged("DateOfBirth");
            }
        }

        /// <summary>
        /// Gets or sets age of the customer
        /// </summary>
        [CategoryAttribute("ID Settings"), DescriptionAttribute("Age of the customer")]
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                if (value >= 18)
                    _age = value;

                NotifyPropertyChanged("Age");
            }
        }

        /// <summary>
        /// Gets or sets FrequentBuyer or not
        /// </summary>
        [CategoryAttribute("Marketting Settings"), DescriptionAttribute("If the customer as bought more than 10 times, this is set to true")]
        public bool FrequentBuyer
        {
            get
            {
                return _frequentBuyer;
            }
            set
            {
                _frequentBuyer = value;
                NotifyPropertyChanged("FrequentBuyer");
            }
        }

        /// <summary>
        /// Gets or sets most current e-mail of the customer
        /// </summary>
        [CategoryAttribute("Marketting Settings"), DescriptionAttribute("Most current e-mail of the customer")]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                NotifyPropertyChanged("Email");
            }
        }

        /// <summary>
        /// Gets or sets Sex of the customer
        /// </summary>
        [CategoryAttribute("ID Settings"), DescriptionAttribute("Sex of the customer")]
        public Sex Sex
        {
            get
            {
                return sex;
            }
            set
            {
                sex = value;
                NotifyPropertyChanged("Sex");
            }
        }

        /// <summary>
        /// Gets or sets Sex of the customer
        /// </summary>
        [CategoryAttribute("Customer Address"), DescriptionAttribute("Address details of the customer")]
        public Address Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                NotifyPropertyChanged("Address");
            }
        }
    }

    /// <summary>
    /// Address Class
    /// </summary>
    public class Address : INotifyPropertyChanged
    {
        private string _Street;
        private string _Region;
        private string _Country;
        private string _AddressName;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets Street name
        /// </summary>
        [CategoryAttribute("Address"), DescriptionAttribute("Address of the customer")]
        public string Street
        {
            get { return _Street; }
            set { _Street = value; NotifyPropertyChanged("Street"); }
        }

        /// <summary>
        /// Gets or sets Region name
        /// </summary>
        [CategoryAttribute("Address"), DescriptionAttribute("Address of the customer")]
        public string Region
        {
            get { return _Region; }
            set { _Region = value; NotifyPropertyChanged("Region"); }
        }

        /// <summary>
        /// Gets or sets Country name
        /// </summary>
        [CategoryAttribute("Address"), DescriptionAttribute("Address of the customer")]
        public string Country
        {
            get { return _Country; }
            set { _Country = value; NotifyPropertyChanged("Country"); }
        }

        public string AddressName
        {
            get
            {
                return _AddressName;
            }
            set
            {
                _AddressName = value;
                NotifyPropertyChanged("AddressName");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum Sex { Male, Female }

    /// <summary>
    /// AddressEditor custom class 
    /// </summary>
    public class AddressEditor : ITypeEditor
    {
        public ObservableCollection<Address> address;
        ComboBox addressCombo;

        /// <summary>
        /// Represents the Attach method
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
        /// Represents the Detach method
        /// </summary>
        public void Detach(PropertyViewItem property)
        {
            
        }
    }
}
