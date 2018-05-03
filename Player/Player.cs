using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    public class Account
    {
        private string mID { get; set; }
        List<Playlist> mPlaylists { get; set; }
    }

    public class Playlist
    {
        List<MediaFile> mMediaFiles { get; set; }

        private string mName{ get; set; }
        private string mAccount{ get; set; }
        
    }
    
    public class MediaFile
    { // mp3,wmv,  mp4
        // file's information
        private string mType { get; set; }  // file type
        private string mPath{ get; set; }
        private float mLength{ get; set; }
        
        // media tag
        private string mTitle{ get; set; }
        private string mArtist{ get; set; }
        private string mAllbum{ get; set; }
        private int mYear{ get; set; }
        private int mTrack{ get; set; }
        private string mGenre{ get; set; }
        private string mcomment{ get; set; }
    }
}
