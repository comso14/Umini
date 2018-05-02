using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    public class Account
    {
        private string m_ID { get; set; }
        List<Playlist> m_Playlists { get; set; }
    }

    public class Playlist
    {
        List<MediaFile> m_MediaFiles { get; set; }

        private string m_Name{ get; set; }
        private string m_Account{ get; set; }
    }

    public class MediaFile
    {
        // file's information
        private string m_Type { get; set; }  // file type
        private string m_Path{ get; set; }
        private float m_Length{ get; set; }
        
        // media tag
        private string m_Title{ get; set; }
        private string m_Artist{ get; set; }
        private string m_Allbum{ get; set; }
        private int m_Year{ get; set; }
        private int m_Track{ get; set; }
        private string Genre{ get; set; }
        private string comment{ get; set; }
    }
}
