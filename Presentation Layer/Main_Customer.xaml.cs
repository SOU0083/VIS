using DataLayer.DataMapper;
using DomainLayer.DomainModel;
using DomainLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Presentation_Layer
{
    /// <summary>
    /// Interaction logic for Main_Customer.xaml
    /// </summary>
    public partial class Main_Customer : Window
    {
        private StructuralObjectMapper structuralObjectMapper = new StructuralObjectMapper();
        private InstitutionCategoryMapper institutionCategoryMapper = new InstitutionCategoryMapper();
        private InstitutionMapper institutionMapper = new InstitutionMapper();
        private EventMapper eventMapper = new EventMapper();
        private ReservationMapper reservationMapper = new ReservationMapper();
        private ReservationObjectMapper reservationObjectMapper = new ReservationObjectMapper();
        private Utils utils = new Utils();

        public Main_Customer()
        {
            InitializeComponent();
            List<string> list = new List<string>();
            list.Add("");
            list.AddRange(institutionCategoryMapper.FindNames());
            ComboBox_Institution_Category.ItemsSource = list;

            List<string> list2 = new List<string>();
            list2.Add("");
            list2.AddRange(institutionMapper.FindTowns());
            ComboBox_Institution_Town.ItemsSource = list2;
        }

        private void Search_Event_Click(object sender, RoutedEventArgs e)
        {
            if (!DateTime.TryParse(DatePicker_Event_From.Text, out DateTime dt) || !DateTime.TryParse(DatePicker_Event_To.Text, out DateTime dt2))
            {
                MessageBox.Show("Chybný formát data.", "Chyba");
                return;
            }
            int price = 0;
            int.TryParse(Input_Event_MaxPrice.Text, out price);
            dataGridSearchEvents.ItemsSource = eventMapper.Search(dt, dt2, price);
            try
            {
                dataGridSearchEvents.ColumnFromDisplayIndex(1).Visibility = Visibility.Hidden;
            }
            catch (ArgumentOutOfRangeException) { }
        }

        private void Search_Institution_Click(object sender, RoutedEventArgs e)
        {
            string category = null;
            string town = null;
            if (ComboBox_Institution_Category.SelectedItem != null)
                category = (string)ComboBox_Institution_Category.SelectedItem;
            if (ComboBox_Institution_Town.SelectedItem != null)
                town = (string)ComboBox_Institution_Town.SelectedItem;
            dataGridSearchInstitutions.ItemsSource = institutionMapper.Search(TextBox_Institution_Name.Text, category, town);
        }

        private void DataGridSearchInstitutions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridSearchInstitutions.SelectedItem == null)
                return;
            List<StructuralObject> objectList = structuralObjectMapper.FindDescendants(((InstitutionDTO)dataGridSearchInstitutions.SelectedItem).HierarchyId);
            treeViewObjects.Items.Clear();
            treeViewObjects.Items.Add(utils.ListToTree(objectList));
        }

        private void Add_Event_Click(object sender, RoutedEventArgs e)
        {
            if(treeViewObjects.SelectedItem == null)
            {
                MessageBox.Show("Není vybrána žádná položka.", "Chyba");
                return;
            }
        }

        private void Reserve_Event_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridSearchEvents.SelectedItem == null)
            {
                MessageBox.Show("Není vybrána žádná položka.", "Chyba");
                return;
            }
            Event ev = (Event) dataGridSearchEvents.SelectedItem;
            if (!ev.CanReserve())
            {
                MessageBox.Show("Rezervace probíhá od " + ev.CanReserveFrom.ToString() + " do " + ev.CanReserveTo.ToString(), "Chyba");
                return;
            }
            else if (!eventMapper.CanReserve(ev.Id))
            {
                MessageBox.Show("Již nejsou volná místa", "Chyba");
                return;
            }
            else
            {
                reservationMapper.Insert(new Reservation(0, new Customer { Id = 1 }, new ReservationObject { Id = ev.Event_Object.Id }, ev, ev.Start, ev.End, null));
                MessageBox.Show("Rezervace proběhla úspěšně.", "Oznámení");
                return;
            }
        }
    }
}
