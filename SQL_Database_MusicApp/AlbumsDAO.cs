using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_Database_MusicApp
{
    internal class AlbumsDAO
    {
        // DAO for checking app with Fake Data 
        //public List<Album> albums = new List<Album>();
        string connectionString = "datasource=localhost;port=3306;username=root;password=root;database=music;";
        public List<Album> getAllAlbums()
        {
            // start with an empty list
            List<Album> returnThese = new List<Album>();

            //connect to MY SQL Server
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            //SQL Querries for retriving data from database
            MySqlCommand command = new MySqlCommand("SELECT ID, ALBUM_TITLE, ARTIST, YEAR, IMAGE_NAME, DESCRIPTION FROM albums", connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Album a = new Album()
                    {
                        ID = reader.GetInt32(0),
                        AlbumTittle = reader.GetString(1),
                        ArtistName = reader.GetString(2),
                        Year = reader.GetInt32(3),
                        ImageURL = reader.GetString(4),
                        Description = reader.GetString(5)
                    };

                    a.Tracks = getTracksForAlbums(a.ID);


                    returnThese.Add(a);
                }
            }
            connection.Close();
            return returnThese;

        }


        public List<Album> searchTittles(string searchTerm)
        {
            // start with an empty list
            List<Album> returnThese = new List<Album>();

            //connect to MY SQL Server
            MySqlConnection connection = new MySqlConnection(connectionString);


            string searchWildPhrase = "%" + searchTerm + "%";

            //SQL Querries for retriving data from database
            MySqlCommand command = new MySqlCommand();

            command.CommandText = "SELECT ID, ALBUM_TITLE, ARTIST, YEAR, IMAGE_NAME, DESCRIPTION FROM albums WHERE ALBUM_TITLE LIKE @Search";
            command.Parameters.AddWithValue("@Search", searchWildPhrase);
            command.Connection = connection;

            connection.Open();

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Album a = new Album()
                    {
                        ID = reader.GetInt32(0),
                        AlbumTittle = reader.GetString(1),
                        ArtistName = reader.GetString(2),
                        Year = reader.GetInt32(3),
                        ImageURL = reader.GetString(4),
                        Description = reader.GetString(5)
                    };
                    returnThese.Add(a);
                }
            }
            connection.Close();
            return returnThese;

        }


        internal int addOneAlbum(Album album)
        {
            //connect to MY SQL Server
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            //SQL Querries for retriving data from database
            MySqlCommand command = new MySqlCommand("INSERT INTO `albums`(`ALBUM_TITLE`, `ARTIST`, `YEAR`, `IMAGE_NAME`, `DESCRIPTION`) VALUES (@albumTittle, @artist, @year, @imageURL, @description)", connection);

            command.Parameters.AddWithValue("@albumTittle", album.AlbumTittle);
            command.Parameters.AddWithValue("@artist", album.ArtistName);
            command.Parameters.AddWithValue("@year", album.Year);
            command.Parameters.AddWithValue("@@imageURL", album.ImageURL);
            command.Parameters.AddWithValue("@description", album.Description);

            int newRows = command.ExecuteNonQuery();
            connection.Close();
            return newRows;
        }

        public List<Track> getTracksForAlbums(int albumID)
        {
            // start with an empty list
            List<Track> returnThese = new List<Track>();

            //connect to MY SQL Server
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();


            //SQL Querries for retriving data from database
            MySqlCommand command = new MySqlCommand();

            command.CommandText = "SELECT * FROM TRACKS WHERE albums_ID = @albumid";
            command.Parameters.AddWithValue("@albumid", albumID);
            command.Connection = connection;

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Track t = new Track()
                    {
                        Id = reader.GetInt32(0),
                        TrackName = reader.GetString(1),
                        TrackNumber = reader.GetInt32(2),
                        //VideoIRL = reader.GetString(3),
                        //TrackLyrics = reader.GetString(4),
                    };
                    returnThese.Add(t);
                }
            }
            connection.Close();
            return returnThese;

        }
        public List<JObject> getTracksUsingJoin(int albumID)
        {
            // start with an empty list
            List<JObject> returnThese = new List<JObject>();

            //connect to MY SQL Server
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();


            //SQL Querries for retriving data from database
            MySqlCommand command = new MySqlCommand();

            command.CommandText = "SELECT tracks.ID as trackID, albums.ALBUM_TITLE, track_tittle, track_number FROM tracks JOIN albums ON albums_ID = albums.ID WHERE albums_ID = @albumid";
            command.Parameters.AddWithValue("@albumid", albumID);
            command.Connection = connection;

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    JObject newTrack = new JObject();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        newTrack.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());

                    }

                    returnThese.Add(newTrack);
                }
            }
            connection.Close();
            return returnThese;
        }

        internal int deleteTrack(int trackID)
        {
            //connect to MY SQL Server
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            //SQL Querries for retriving data from database
            MySqlCommand command = new MySqlCommand("DELETE FROM `tracks` WHERE `tracks`.`ID` = @trackID;", connection);

            command.Parameters.AddWithValue("@trackID", trackID);
       
            int results = command.ExecuteNonQuery();
            connection.Close();
            return results;

        }
    }
}
