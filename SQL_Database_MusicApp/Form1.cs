using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQL_Database_MusicApp
{
    public partial class Form1 : Form
    {
        BindingSource albumBindingSource = new BindingSource();

        BindingSource tracksBindingSource = new BindingSource();

        List<Album> albums = new List<Album>();

        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {

        }

        public void button1_Click(object sender, EventArgs e)
        {
            /* Create fake data
            AlbumsDAO albumsDAO = new AlbumsDAO();
            Album a1 = new Album
            {
                ID = 1,
                AlbumTittle = "Fake Data 1",
                ArtistName = "Akansha",
                Year = 2024,
                ImageURL = "Nothig Yet",
                Description = "No Data"
            };

            Album a2 = new Album
            {
                ID = 2,
                AlbumTittle = "Fake Data 2",
                ArtistName = "Akansha",
                Year = 2024,
                ImageURL = "Nothig Yet",
                Description = "No Data"
            };

            albumsDAO.albums.Add(a1);
            albumsDAO.albums.Add(a2);

            //connect the list to data grid view
            albumBindingSource.DataSource = albumsDAO.albums;
            dataGridView1.DataSource = albumBindingSource; */

            AlbumsDAO albumsDAO = new AlbumsDAO();

            albums = albumsDAO.getAllAlbums();

            //connect the list to data grid view
            albumBindingSource.DataSource = albums;

            dataGridView1.DataSource = albumBindingSource;

            pictureBox1.Load ("https://upload.wikimedia.org/wikipedia/en/8/84/MarvinGayeWhat%27sGoingOnalbumcover.jpg");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AlbumsDAO albumsDAO = new AlbumsDAO();
            
            //connect the list to search box

            albumBindingSource.DataSource = albumsDAO.searchTittles(textBox1.Text);

            dataGridView1.DataSource = albumBindingSource;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView) sender;

            //get the row number

            int rowClicked = dataGridView.CurrentRow.Index;
            //MessageBox.Show ("You clicked row " + rowClicked);

            String imageURL = dataGridView.Rows[rowClicked].Cells[4].Value.ToString();
            //MessageBox.Show ("URL = " + imageURL);

            pictureBox1.Load(imageURL);


            // AlbumsDAO albumsDAO = new AlbumsDAO();

            //connect the list to data grid view

            //tracksBindingSource.DataSource = albumsDAO.getTracksForAlbums((int)dataGridView.Rows[rowClicked].Cells[0].Value);

            //tracksBindingSource.DataSource = albumsDAO.getTracksUsingJoin((int)dataGridView.Rows[rowClicked].Cells[0].Value);
            tracksBindingSource.DataSource = albums[rowClicked].Tracks;
            dataGridView2.DataSource = tracksBindingSource;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //add new albums to the table
            Album album = new Album
            {
                AlbumTittle = txt_album.Text,
                ArtistName = txt_artist.Text,
                Year = Int32.Parse(txt_year.Text),
                ImageURL = txt_imageURL.Text,
                Description = txt_description.Text
            };

            AlbumsDAO albumsDAO = new AlbumsDAO();
            
            int result = albumsDAO.addOneAlbum(album);

            MessageBox.Show(result + "no. of rows are interted");

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //get the row number

            int rowClicked = dataGridView2.CurrentRow.Index;
            MessageBox.Show ("You deleted row " + rowClicked);
            int trackID = (int) dataGridView2.Rows[rowClicked].Cells[0].Value;
            MessageBox.Show ("ID of Track = " + trackID );

            AlbumsDAO albumsDAO = new AlbumsDAO();

            int results = albumsDAO.deleteTrack(trackID);

            MessageBox.Show ("Results" + results );

            dataGridView2.DataSource =null;

            albums = albumsDAO.getAllAlbums();  

        }
    }

}
