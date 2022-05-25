using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace WPF_Buhl
{
    /// <summary>
    /// Interaction logic for Page_Person.xaml
    /// </summary>
    public partial class Page_Person : Page
    {
        public Page_Person()
        {
            InitializeComponent();
        }
        //Creating an instance of Sql database
        SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\kamal\\OneDrive\\Dokumente\\New_PersonDB.mdf;Integrated Security=True;Connect Timeout= 30");
        //creating an instance  of adapter to fill table of database
        SqlDataAdapter adapter = new SqlDataAdapter();
        // Create a new DataTable
        DataTable table = new DataTable();
        public TextBox txt_extracontact;
        public int Id;
        //string childname = "";


        private void Button_Click(object sender, RoutedEventArgs e)
        { //Insert
            InsertData();
            ReadData();
            ClearTxtbox();


        }

        private void InsertData()
        {
            if (IsValid())
            {
                //to execute sql query to insert data  in th table
                SqlCommand cmd = new SqlCommand(@"Insert Into  [Table_Person] values ( @Name, @Surname,@StreetName,@HouseNo,@PostCode,@City,@Contact,@ExtraContact,@Picture)", conn);
                //
                cmd.Parameters.AddWithValue("@Name", txt_name.Text);
                cmd.Parameters.AddWithValue("@Surname", txt_surname.Text);
                cmd.Parameters.AddWithValue("@StreetName", txt_streetname.Text);
                cmd.Parameters.AddWithValue("@HouseNo", txt_houseno.Text);
                cmd.Parameters.AddWithValue("@PostCode", txt_postcode.Text);
                cmd.Parameters.AddWithValue("@City", txt_city.Text);
                cmd.Parameters.AddWithValue("@Contact", txt_contact.Text);

                string childname = "";
                foreach (object child in tbPanel.Children)
                {

                    if (child is TextBox)
                    {
                        childname += (child as TextBox).Text + ",";
                    }


                }

                cmd.Parameters.AddWithValue("@ExtraContact", childname);
                cmd.Parameters.AddWithValue("@Picture", img1.IsLoaded);


                conn.Open();
                //executing query and putting data in database
                cmd.ExecuteNonQuery();

                conn.Close();
                ClearTxtbox();
                MessageBox.Show("Data Inserted Succesfully");
            }
            else
            {
                MessageBox.Show("Please Fill Up The Form");
            }
            ReadData();
            ClearTxtbox();
        }
        private bool IsValid()
        {
            if (txt_name.Name == String.Empty)
            {
                MessageBox.Show("Please enter a name", "Select?", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            return true;
        }
        private void ClearTxtbox()
        {
           
            txt_city.Clear();
            txt_contact.Clear();
            txt_houseno.Clear();
            txt_name.Clear();
            txt_postcode.Clear();
            txt_streetname.Clear();
            txt_surname.Clear();
            txt_search.Clear();
            
            foreach (object child in tbPanel.Children)
            {

                if (child is TextBox)
                {
                    (child as TextBox).Text = "";
                }
            }

        }
        private void ReadData()
        {
            //to execute sql query to insert data  in th table
            SqlCommand cmd = new SqlCommand(@"Select * from [Table_Person]", conn);
            //creating an instance  of adapter to fill table of database
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            conn.Open();
            adapter.Fill(table);
            dg.ItemsSource = table.DefaultView;
            cmd.ExecuteNonQuery();
            conn.Close();

        }
        private void SearchData()
        {
            //to execute sql query to insert data  in th table
            SqlCommand cmd = new SqlCommand("Select * from [Table_Person] where Name = '" + this.txt_search.Text + "'", conn);
            //creating an instance  of adapter to fill table of database
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable tableS = new DataTable();
            conn.Open();
            adapter.Fill(tableS);
            dgs.ItemsSource = tableS.DefaultView;
            cmd.ExecuteNonQuery();
            conn.Close();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Show Data
            ReadData();
        }
   

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
          
            DeleteData();
        }
        public void DeleteData()
        {
            //to execute sql query to insert data  in th table
            SqlCommand cmd = new SqlCommand("Delete  from [Table_Person] where Id=@Id", conn);

            cmd.Parameters.AddWithValue("@Id", txt_delId.Text);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        { 
                UpdateData();
        }
        private void UpdateData()
        {
            if (txt_name.Text != null || txt_surname.Text != null || txt_streetname.Text != null || txt_houseno.Text != null || txt_postcode.Text != null || txt_city.Text != null)
            {
                string childname = "";
                foreach (object child in tbPanel.Children)
                {

                    if (child is TextBox)
                    {
                        childname += (child as TextBox).Text + ",";
                    }


                }
                conn.Open();
                //to execute sql query to insert data  in th table
                SqlCommand cmd = new SqlCommand("UPDATE [Table_Person] SET  Name = '" + this.txt_name.Text + "', Surname = '" + this.txt_surname.Text + "',StreetName = '" + this.txt_streetname.Text + "',HouseNo = '" + this.txt_houseno.Text + "',PostCode = '" + this.txt_postcode.Text + "', City = '" + this.txt_city.Text + "', Contact = '" + this.txt_contact.Text + "',ExtraContact = '" + childname + "' WHERE Id = @Id ", conn);

                cmd.Parameters.AddWithValue("@Id", txt_IdToUpdate.Text);


                cmd.ExecuteNonQuery();


                conn.Close();
            }
            else
            {
                MessageBox.Show("Not Updated");
            }

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //clearing textbox
            ClearTxtbox();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {  //Search button
            SearchData();
        }

        private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {   //Upload button
            
            try
            {
                //creating an instance of OpenFileDialog
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                //Filtering file type
                dlg.Filter = "PNG Image | *.png";
                dlg.DefaultExt = "png";
                dlg.FilterIndex = 1;
                if (dlg.ShowDialog() == true)
                {
                    //open the file as a stream
                    System.IO.Stream stream = System.IO.File.Open(dlg.FileName, System.IO.FileMode.Open);
                    // Creating the image source.
                    BitmapImage imgsrc = new BitmapImage();
                    imgsrc.BeginInit();
                    imgsrc.StreamSource = stream;
                    imgsrc.EndInit();
                    // Setting the image source.
                    img1.Source = imgsrc;

                }
            }
            catch
            {

            }          
        
        }
        
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            TextBox txt_extracontact = new TextBox();
           // Added StackPanel to fecilitate more textbox creation
            tbPanel.Children.Add(txt_extracontact);
            

        }


    }
}
