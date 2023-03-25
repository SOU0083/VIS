using DataLayer.DataMapper;
using DomainLayer.DomainModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation_Layer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Institution loggedInstitution = new Institution()
        {
            HierarchyId = "/3/"
        };

        private StructuralObjectMapper structuralObjectMapper = new StructuralObjectMapper();
        private EventMapper eventMapper = new EventMapper();
        private ReservationMapper reservationMapper = new ReservationMapper();
        private Utils utils = new Utils();

        public MainWindow()
        {
            InitializeComponent();
            dataGridEvents.ItemsSource = eventMapper.FindDescendantsEvents(loggedInstitution.HierarchyId);
            try
            {
                dataGridEvents.ColumnFromDisplayIndex(1).Visibility = Visibility.Hidden;
            }
            catch (ArgumentOutOfRangeException) { }

            List<StructuralObject> objectList = structuralObjectMapper.FindDescendants(loggedInstitution.HierarchyId);
            treeViewObjects.Items.Add(utils.ListToTree(objectList));
        }

        private void Add_Object_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewObjects.SelectedItem == null)
                MessageBox.Show("Není vybrána žádná položka.", "Chyba");
        }

        private void Add_Reservation_Object_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewObjects.SelectedItem == null)
                MessageBox.Show("Není vybrána žádná položka.", "Chyba");
        }

        private void Add_Event_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewObjects.SelectedItem == null)
                MessageBox.Show("Není vybrána žádná položka.", "Chyba");
        }

        private void Delete_Object_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewObjects.SelectedItem == null)
            {
                MessageBox.Show("Není vybrána žádná položka.", "Chyba");
                return;
            }
            TreeViewItem tvi = treeViewObjects.SelectedItem as TreeViewItem;
            StructuralObject selectedObject = structuralObjectMapper.Find(Int32.Parse(tvi.Name.Substring(8)));
            if (selectedObject.HierarchyId.Count(x => x == '/') == 2)
            { 
                MessageBox.Show("Kořenový objekt nelze smazat.", "Chyba");
                return;
            }
            if (MessageBox.Show("Opravdu chcete objekt smazat?", "Potvrzení smazání", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                List<Reservation> rList = reservationMapper.FindDescendantsReservations(selectedObject.HierarchyId);
                foreach (Reservation r in rList)
                    r.WriteEmailToCustomer("Vaše rezervace ID: " + r.Id + " byla zrušena.");

                reservationMapper.DeleteDescendantsReservations(selectedObject.HierarchyId);
                eventMapper.DeleteDescendantsEvents(selectedObject.HierarchyId);
                structuralObjectMapper.DeleteDescendants(selectedObject.HierarchyId);

                List<StructuralObject> objectList = structuralObjectMapper.FindDescendants(loggedInstitution.HierarchyId);
                treeViewObjects.Items.Clear();
                treeViewObjects.Items.Add(utils.ListToTree(objectList));
            }
        }

        private void Update_Reservation_Object_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewObjects.SelectedItem == null)
            {
                MessageBox.Show("Není vybrána žádná položka.", "Chyba");
                return;
            }
        }

        private void DataGridEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridEvents.SelectedItem == null)
                textBlockEvent.Text = "";
            else
                textBlockEvent.Text = ((Event)dataGridEvents.SelectedItem).Name;
        }

        private void Update_Event_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridEvents.SelectedItem == null)
            {
                MessageBox.Show("Není vybrána žádná položka.", "Chyba");
                return;
            }
        }

        private void Delete_Event_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridEvents.SelectedItem == null)
            {
                MessageBox.Show("Není vybrána žádná položka.", "Chyba");
                return;
            }
            int eventId = ((Event)dataGridEvents.SelectedItem).Id;

            List<Reservation> rList = reservationMapper.FindEventReservations(eventId);
            foreach (Reservation r in rList)
                r.Reservation_Customer.WriteEmailAndPayBack("Vaše rezervace ID: " + r.Id + " byla zrušena.");

            reservationMapper.DeleteEventReservations(eventId);
            eventMapper.Delete(eventId);

            dataGridEvents.ItemsSource = eventMapper.FindDescendantsEvents(loggedInstitution.HierarchyId);
        }
    }
}
