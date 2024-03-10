using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_Database_MusicApp
{
    internal class Album
    {
        public int ID { get; set; }
        public string AlbumTittle { get; set; }
        public string ArtistName { get; set; }
        public int Year { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }

        //Later will create <Track> List

        public List<Track> Tracks { get; set; }
    }
};
