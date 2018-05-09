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

    public class Youtube : MediaFile
    {
        public string mURL { get; set; }
        public string mYoutubeID { get; set; }
        public string mYoutubeTitle { get; set; }
    }


    public class MediaFile
    { // mp3,wmv,  mp4
      // file's information

        public string mType { get; set; }  // file type
        public string mExt { get; set; }
        public string mPath { get; set; }
        public float mLength { get; set; }

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
    }
}
