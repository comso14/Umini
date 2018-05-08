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

        private string mName { get; set; }
        private string mAccount { get; set; }

    }

    public class MediaFile
    { // mp3,wmv,  mp4
        // file's information
        private string mType { get; set; }  // file type
        private string mPath { get; set; }
        private float mLength { get; set; }

        // media tag
        public string mTitle { get; set; }
        public string mArtist { get; set; }
        public string mAllbum { get; set; }
        public string mYear { get; set; }
        public int mTrack { get; set; }
        public string mGenre { get; set; }
        public string mcomment { get; set; }
        public string mImagePath { get; set; }
        public string mLyric { get; set; }
        // youtube tag

        public string mYoutubeId { get; set; }
        public string mYoutbueTitle { get; set; }
    }
}
