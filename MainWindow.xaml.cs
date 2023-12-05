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
using VideoTekka.model;
using System.IO;
using System.Data;
using Microsoft.Data.Sqlite;


namespace VideoTekka
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string connection_string = "Data Source = MovieBase.db";
        private string _image_path;
        private byte[] _image;
        int a = 0;
        int b = 1;
        int c = 2;
        
        Movie movie = new Movie();
        List<Movie> movies = new List<Movie>();
        public MainWindow()
        {
            InitializeComponent();
          

            

        }

        private void bDown_Click(object sender, RoutedEventArgs e)
        {
            if (movies.Count == 1)
            {
                lLabel2.Content = movies[0].Title;

            }
            if (movies.Count == 2)
            {
                int temp;
                temp = a;
                a = b;
                b = temp;

                lLabel1.Content = movies[a].Title;
                lLabel2.Content = movies[b].Title;
                txtTitle.Text = movies[b].Title;
                txtOpis.Text = movies[b].Description;
                txtDirector.Text = movies[b].Director;

                MemoryStream memoryStream = new MemoryStream(movies[b].cover);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                Cover.Source = bitmapImage;
            }
            if (movies.Count >= 3)
            {
                a--;
                b--;
                c--;

                if (a < 0)
                {
                    a = movies.Count-1;
                }
                if (b < 0)
                {
                    b = movies.Count-1;
                }
                if (c < 0)
                {
                    c = movies.Count - 1;
                }

                lLabel1.Content = movies[a].Title;
                lLabel2.Content = movies[b].Title;
                lLabel3.Content = movies[c].Title;

                txtTitle.Text = movies[b].Title;
                txtOpis.Text = movies[b].Description;
                txtDirector.Text = movies[b].Director;

                MemoryStream memoryStream = new MemoryStream(movies[b].cover);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                Cover.Source = bitmapImage;
            }
        }

        private void bUp_Click(object sender, RoutedEventArgs e)
        {
            if(movies.Count == 1)
            {
                lLabel2.Content = movies[0].Title;
                
            }
            if (movies.Count == 2)
            {
                int temp;
                temp = a; 
                a = b;
                b = temp;
               
                lLabel1.Content = movies[a].Title;
                lLabel2.Content = movies[b].Title;
                txtTitle.Text = movies[b].Title;
                txtOpis.Text = movies[b].Description;
                txtDirector.Text = movies[b].Director;

                MemoryStream memoryStream = new MemoryStream(movies[b].cover);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                Cover.Source = bitmapImage;
            }
            if(movies.Count >= 3)
            {
                a++;
                b++;
                c++;

                if(a == movies.Count)
                {
                    a = 0;
                }
                if(b == movies.Count)
                {
                    b = 0;
                }
                if(c == movies.Count)
                {
                    c = 0;
                }

                lLabel1.Content = movies[a].Title;
                lLabel2.Content = movies[b].Title;
                lLabel3.Content = movies[c].Title;

                txtTitle.Text = movies[b].Title;
                txtOpis.Text = movies[b].Description;
                txtDirector.Text = movies[b].Director;

                MemoryStream memoryStream = new MemoryStream(movies[b].cover);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                Cover.Source = bitmapImage;
            }
        }

        private void bCreate_Click(object sender, RoutedEventArgs e)
        {
            Create create = new Create();
            create.ShowDialog();

        }

        private void bUpdate_Click(object sender, RoutedEventArgs e)
        {
            Update update = new Update(movies[b].Title);
            update.ShowDialog();

        }

        private void bDelete_Click(object sender, RoutedEventArgs e)
        {
            SqliteConnection sqliteConnection = new SqliteConnection(connection_string);
            sqliteConnection.Open();
            SqliteCommand sqliteCommand = sqliteConnection.CreateCommand();
            sqliteCommand.CommandText = $"DELETE FROM Movies WHERE Title = '{movies[b].Title}'";
            sqliteCommand.Connection = sqliteConnection;
            sqliteCommand.ExecuteNonQuery();

        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bLoad_Click(object sender, RoutedEventArgs e)
        {
            movies.Clear();
            SqliteConnection connection = new SqliteConnection(connection_string);
            SqliteCommand command = connection.CreateCommand();
            if (!File.Exists("MovieBase.db"))
            {
                connection.Open();
                command.CommandText = "CREATE TABLE Movies(Id INTEGER PRIMARY KEY , Title VARCHAR(30), Director VARCHAR(30), Description VARCHAR(40), Cover BLOB)";
                command.Connection = connection;
                command.ExecuteNonQuery();
                connection.Close();
            }
            connection.Open();
            command.CommandText = "SELECT * FROM Movies";
            command.Connection = connection;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Movie tempMovie = new Movie();
                tempMovie.Id = Convert.ToInt32(reader["Id"]);
                tempMovie.Title = reader["Title"].ToString();
                tempMovie.Director = reader["Director"].ToString();
                tempMovie.Description = reader["Description"].ToString();
                tempMovie.cover = (byte[])reader["Cover"];
                movies.Add(tempMovie);
            }

            connection.Close();
            if (movies[0].Title != null)
            {
                if (movies.Count >= 0)
                {
                    lLabel2.Content = movies[0].Title;
                }
                if (movies.Count > 0)
                {
                    lLabel1.Content = movies[1].Title;
                }
                if (movies.Count > 1)
                {
                    lLabel3.Content = movies[2].Title;
                }
                txtTitle.Text = movies[0].Title;
                txtOpis.Text = movies[0].Description;
                txtDirector.Text = movies[0].Director;

                MemoryStream memoryStream = new MemoryStream(movies[0].cover);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                Cover.Source = bitmapImage;
            }
        }
    }
}
