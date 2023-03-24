using DataLayer;
using DataLayer.DataMapper;
using DomainLayer.DomainModel;
using DomainLayer.DTO;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for Main_Admin.xaml
    /// </summary>
    public partial class Main_Admin : Window
    {
        private InstitutionMapper im = new InstitutionMapper();
        private StructuralObjectMapper structuralObjectMapper = new StructuralObjectMapper();
        private EventMapper eventMapper = new EventMapper();
        private ReservationMapper reservationMapper = new ReservationMapper();
        private JsonFileSerializer<List<InstitutionDTO>> serializer = new JsonFileSerializer<List<InstitutionDTO>>();

        public Main_Admin()
        {
            InitializeComponent();
            dataGridInstitutions.ItemsSource = im.FindWithObject();
        }

        private void Delete_Institution_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridInstitutions.SelectedItem == null)
            {
                MessageBox.Show("Není vybrána žádná položka.", "Chyba");
                return;
            }
            if (MessageBox.Show("Opravdu chcete instituci smazat?", "Potvrzení smazání", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                InstitutionDTO selectedObject = (InstitutionDTO)dataGridInstitutions.SelectedItem;

                List<Reservation> rList = reservationMapper.FindDescendantsReservations(selectedObject.HierarchyId);
                foreach (Reservation r in rList)
                    r.Reservation_Customer.WriteEmail("Vaše rezervace ID: " + r.Id + " byla zrušena.");

                reservationMapper.DeleteDescendantsReservations(selectedObject.HierarchyId);
                eventMapper.DeleteDescendantsEvents(selectedObject.HierarchyId);
                structuralObjectMapper.DeleteDescendants(selectedObject.HierarchyId);

                dataGridInstitutions.ItemsSource = im.FindWithObject();
            }
        }

        private void Save_Institution_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_File.Text.Equals(""))
            {
                MessageBox.Show("Není zadán název souboru.", "Chyba");
                return;
            }
            serializer.Serialize((List<InstitutionDTO>)dataGridInstitutions.ItemsSource, TextBox_File.Text + ".json");
        }

        private void Load_Institution_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_File.Text.Equals(""))
            {
                MessageBox.Show("Není zadán název souboru.", "Chyba");
                return;
            }
            try
            {
                dataGridInstitutions.ItemsSource = serializer.Deserialize(TextBox_File.Text + ".json");
            }
            catch (FileNotFoundException exc)
            {
                MessageBox.Show("Soubor nenalezen.", "Chyba");
                return;
            }
        }
    }
}
