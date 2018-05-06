﻿using System;
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
        public string mType { get; set; }  // file type
        public string mPath{ get; set; }
        public float mLength{ get; set; }

        // media tag
        public string mTitle{ get; set; }
        public string mArtist{ get; set; }
        public string mAllbum{ get; set; }
        public int mYear{ get; set; }
        public int mTrack{ get; set; }
        public string mGenre{ get; set; }
        public string mcomment{ get; set; }
    }
}
