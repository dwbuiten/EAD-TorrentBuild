namespace EAD.PeerToPeer
{
    using EAD.Conversion;
    using System;
    using System.Web;

    public class LinkGeneration
    {
        private string BTInfoBase32;
        private string BTInfoRaw;
        private HashChanger convertme = new HashChanger();
        private const string ED2KPrefix = "ed2k://|file|";
        private string ED2KRawHash;
        private string LinkedFilename;
        private long LinkedFileSize;
        private const string MagnetBitPrintPrefix = "xt=urn:bitprint:";
        private const string MagnetBTIHPrefix = "xt=urn:btih:";
        private const string MagnetED2KPrefix = "xt=urn:ed2k:";
        private const string MagnetPrefix = "magnet:?";
        private const string MagnetSHA1Prefix = "xt=urn:sha1:";
        private const string MagnetTigerPrefix = "xt=urn:tree:tiger:";
        private string SHA1Base32Hash;
        private string SHA1RawHash;
        private string TigerTreeBase32Hash;
        private string TigerTreeRootRawHash;

        public string BTIH
        {
            get
            {
                return this.BTInfoBase32;
            }
            set
            {
                this.BTInfoBase32 = value;
                this.BTInfoRaw = null;
            }
        }

        public string BTIHHex
        {
            get
            {
                this.convertme = new HashChanger();
                this.convertme.rawhash = this.BTInfoRaw;
                return this.convertme.hexhash;
            }
            set
            {
                this.convertme = new HashChanger();
                this.convertme.hexhash = value;
                this.BTInfoRaw = this.convertme.rawhash;
                this.BTInfoBase32 = this.convertme.base32;
            }
        }

        public byte[] BTInfoBytes
        {
            get
            {
                this.convertme = new HashChanger();
                this.convertme.rawhash = this.BTInfoRaw;
                return this.convertme.bytehash;
            }
            set
            {
                this.convertme = new HashChanger();
                this.convertme.bytehash = value;
                this.BTInfoRaw = this.convertme.rawhash;
                this.BTInfoBase32 = this.convertme.base32;
            }
        }

        public string BTInfoHashRaw
        {
            get
            {
                return this.BTInfoRaw;
            }
            set
            {
                this.BTInfoRaw = value;
                this.convertme = new HashChanger();
                this.convertme.rawhash = value;
                this.BTInfoBase32 = this.convertme.base32;
            }
        }

        public string ClassicED2KLink
        {
            get
            {
                HashChanger changer = new HashChanger {
                    rawhash = this.ED2KRawHash
                };
                if (((changer.hexhash != "") && (this.LinkedFilename != "")) && (this.LinkedFileSize > 0L))
                {
                    return string.Concat(new object[] { "ed2k://|file|", HttpUtility.UrlEncode(this.LinkedFilename), "|", this.LinkedFileSize, "|", changer.hexhash, "|/" });
                }
                return "";
            }
        }

        public byte[] ed2kbytes
        {
            get
            {
                this.convertme = new HashChanger();
                this.convertme.rawhash = this.ED2KRawHash;
                return this.convertme.bytehash;
            }
            set
            {
                this.convertme = new HashChanger();
                this.convertme.bytehash = value;
                this.ED2KRawHash = this.convertme.rawhash;
            }
        }

        public string ED2KHex
        {
            get
            {
                this.convertme = new HashChanger();
                this.convertme.rawhash = this.ED2KRawHash;
                return this.convertme.hexhash;
            }
            set
            {
                this.convertme = new HashChanger();
                this.convertme.hexhash = value;
                this.ED2KRawHash = this.convertme.rawhash;
            }
        }

        public string ED2KRaw
        {
            get
            {
                return this.ED2KRawHash;
            }
            set
            {
                this.ED2KRawHash = value;
            }
        }

        public string FileName
        {
            get
            {
                return this.LinkedFilename;
            }
            set
            {
                this.LinkedFilename = HttpUtility.UrlDecode(value);
            }
        }

        public long FileSize
        {
            get
            {
                return this.LinkedFileSize;
            }
            set
            {
                this.LinkedFileSize = value;
            }
        }

        public string MagnetBitPrint
        {
            get
            {
                if (!(this.MagnetBitPrintContent != "") || !(this.LinkedFilename != ""))
                {
                    return "";
                }
                if (this.LinkedFileSize > 0L)
                {
                    return ("magnet:?" + this.MagnetBitPrintContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename) + "&xl=" + this.LinkedFileSize.ToString());
                }
                return ("magnet:?" + this.MagnetBitPrintContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename));
            }
        }

        public string MagnetBitPrintBTIH
        {
            get
            {
                if ((!(this.MagnetBitPrintContent != "") || !(this.MagnetBTIHContent != "")) || !(this.LinkedFilename != ""))
                {
                    return "";
                }
                if (this.LinkedFileSize > 0L)
                {
                    return ("magnet:?" + this.MagnetBitPrintContent + "&" + this.MagnetBTIHContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename) + "&xl=" + this.LinkedFileSize.ToString());
                }
                return ("magnet:?" + this.MagnetBitPrintContent + "&" + this.MagnetBTIHContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename));
            }
        }

        public string MagnetBitPrintContent
        {
            get
            {
                if ((this.TigerTreeBase32Hash != "") && (this.SHA1Base32Hash != ""))
                {
                    return ("xt=urn:bitprint:" + this.SHA1Base32Hash + "." + this.TigerTreeBase32Hash);
                }
                return "";
            }
        }

        public string MagnetBitPrintHybrid
        {
            get
            {
                if ((!(this.MagnetBitPrintContent != "") || !(this.MagnetED2KContent != "")) || !(this.LinkedFilename != ""))
                {
                    return "";
                }
                if (this.LinkedFileSize > 0L)
                {
                    return ("magnet:?" + this.MagnetBitPrintContent + "&" + this.MagnetED2KContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename) + "&xl=" + this.LinkedFileSize.ToString());
                }
                return ("magnet:?" + this.MagnetBitPrintContent + "&" + this.MagnetED2KContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename));
            }
        }

        public string MagnetBTIH
        {
            get
            {
                if ((this.MagnetBTIHContent != "") && (this.LinkedFilename != ""))
                {
                    return ("magnet:?" + this.MagnetBTIHContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename));
                }
                return "";
            }
        }

        public string MagnetBTIHContent
        {
            get
            {
                if (this.BTInfoBase32 != "")
                {
                    return ("xt=urn:btih:" + this.BTInfoBase32);
                }
                return "";
            }
        }

        public string MagnetED2K
        {
            get
            {
                if (!(this.MagnetED2KContent != "") || !(this.LinkedFilename != ""))
                {
                    return "";
                }
                if (this.LinkedFileSize > 0L)
                {
                    return ("magnet:?" + this.MagnetED2KContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename) + "&xl=" + this.LinkedFileSize.ToString());
                }
                return ("magnet:?" + this.MagnetED2KContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename));
            }
        }

        public string MagnetED2KBTIH
        {
            get
            {
                if (((this.MagnetED2KContent != "") && (this.MagnetBTIHContent != "")) && (this.LinkedFilename != ""))
                {
                    return ("magnet:?" + this.MagnetED2KContent + "&" + this.MagnetBTIHContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename));
                }
                return "";
            }
        }

        public string MagnetED2KContent
        {
            get
            {
                this.convertme = new HashChanger();
                this.convertme.rawhash = this.ED2KRawHash;
                if (this.convertme.rawhash != "")
                {
                    return ("xt=urn:ed2k:" + this.convertme.hexhash);
                }
                return "";
            }
        }

        public string MagnetFull
        {
            get
            {
                if ((!(this.MagnetBTIHContent != "") || !(this.MagnetBitPrintContent != "")) || (!(this.MagnetED2KContent != "") || !(this.LinkedFilename != "")))
                {
                    return "";
                }
                if (this.LinkedFileSize > 0L)
                {
                    return ("magnet:?" + this.MagnetBitPrintContent + "&" + this.MagnetED2KContent + "&" + this.MagnetBTIHContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename) + "&xl=" + this.LinkedFileSize.ToString());
                }
                return ("magnet:?" + this.MagnetBitPrintContent + "&" + this.MagnetED2KContent + "&" + this.MagnetBTIHContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename));
            }
        }

        public string MagnetSHA1
        {
            get
            {
                if (!(this.MagnetSHA1Content != "") || !(this.LinkedFilename != ""))
                {
                    return "";
                }
                if (this.LinkedFileSize > 0L)
                {
                    return ("magnet:?" + this.MagnetSHA1Content + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename) + "&xl=" + this.LinkedFileSize.ToString());
                }
                return ("magnet:?" + this.MagnetSHA1Content + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename));
            }
        }

        public string MagnetSHA1BTIH
        {
            get
            {
                if ((!(this.MagnetSHA1Content != "") || !(this.MagnetBTIHContent != "")) || !(this.LinkedFilename != ""))
                {
                    return "";
                }
                if (this.LinkedFileSize > 0L)
                {
                    return ("magnet:?" + this.MagnetSHA1Content + "&" + this.MagnetBTIHContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename) + "&xl=" + this.LinkedFileSize.ToString());
                }
                return ("magnet:?" + this.MagnetSHA1Content + "&" + this.MagnetBTIHContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename));
            }
        }

        public string MagnetSHA1Content
        {
            get
            {
                if (this.SHA1Base32Hash != "")
                {
                    return ("xt=urn:sha1:" + this.SHA1Base32Hash);
                }
                return "";
            }
        }

        public string MagnetSHA1ED2KBTIH
        {
            get
            {
                if (((this.MagnetSHA1Content != "") && (this.MagnetED2KContent != "")) && ((this.MagnetBTIHContent != "") && (this.LinkedFilename != "")))
                {
                    return ("magnet:?" + this.MagnetSHA1Content + "&" + this.MagnetED2KContent + "&" + this.MagnetBTIHContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename));
                }
                return "";
            }
        }

        public string MagnetSHA1Hybrid
        {
            get
            {
                if ((!(this.MagnetED2KContent != "") || !(this.MagnetSHA1Content != "")) || !(this.LinkedFilename != ""))
                {
                    return "";
                }
                if (this.LinkedFileSize > 0L)
                {
                    return ("magnet:?" + this.MagnetSHA1Content + "&" + this.MagnetED2KContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename) + "&xl=" + this.LinkedFileSize.ToString());
                }
                return ("magnet:?" + this.MagnetSHA1Content + "&" + this.MagnetED2KContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename));
            }
        }

        public string MagnetTigerBTIH
        {
            get
            {
                if ((!(this.MagnetTigerTreeContent != "") || !(this.MagnetBTIHContent != "")) || !(this.LinkedFilename != ""))
                {
                    return "";
                }
                if (this.LinkedFileSize > 0L)
                {
                    return ("magnet:?" + this.MagnetTigerTreeContent + "&" + this.MagnetBTIHContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename) + "&xl=" + this.LinkedFileSize.ToString());
                }
                return ("magnet:?" + this.MagnetTigerTreeContent + "&" + this.MagnetBTIHContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename));
            }
        }

        public string MagnetTigerED2K
        {
            get
            {
                if ((!(this.MagnetTigerTreeContent != "") || !(this.MagnetED2KContent != "")) || !(this.LinkedFilename != ""))
                {
                    return "";
                }
                if (this.LinkedFileSize > 0L)
                {
                    return ("magnet:?" + this.MagnetTigerTreeContent + "&" + this.MagnetED2KContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename) + "&xl=" + this.LinkedFileSize.ToString());
                }
                return ("magnet:?" + this.MagnetTigerTreeContent + "&" + this.MagnetED2KContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename));
            }
        }

        public string MagnetTigerED2KBTIH
        {
            get
            {
                if ((!(this.MagnetTigerTreeContent != "") || !(this.MagnetED2KContent != "")) || (!(this.MagnetBTIHContent != "") || !(this.LinkedFilename != "")))
                {
                    return "";
                }
                if (this.LinkedFileSize > 0L)
                {
                    return ("magnet:?" + this.MagnetTigerTreeContent + "&" + this.MagnetED2KContent + "&" + this.MagnetBTIHContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename) + "&xl=" + this.LinkedFileSize.ToString());
                }
                return ("magnet:?" + this.MagnetTigerTreeContent + "&" + this.MagnetED2KContent + "&" + this.MagnetBTIHContent + "&dn=" + HttpUtility.UrlEncode(this.LinkedFilename));
            }
        }

        public string MagnetTigerTreeContent
        {
            get
            {
                if (this.TigerTreeBase32Hash != "")
                {
                    return ("xt=urn:tree:tiger:" + this.TigerTreeBase32Hash);
                }
                return "";
            }
        }

        public byte[] SHA1Bytes
        {
            get
            {
                this.convertme = new HashChanger();
                this.convertme.rawhash = this.SHA1RawHash;
                return this.convertme.bytehash;
            }
            set
            {
                this.convertme = new HashChanger();
                this.convertme.bytehash = value;
                this.SHA1RawHash = this.convertme.rawhash;
            }
        }

        public string SHA1Hash
        {
            get
            {
                return this.SHA1Base32Hash;
            }
            set
            {
                this.SHA1Base32Hash = value;
                this.SHA1RawHash = null;
            }
        }

        public string SHA1Hex
        {
            get
            {
                this.convertme = new HashChanger();
                this.convertme.rawhash = this.SHA1RawHash;
                return this.convertme.hexhash;
            }
            set
            {
                this.convertme = new HashChanger();
                this.convertme.hexhash = value;
                this.SHA1RawHash = this.convertme.rawhash;
                this.SHA1Base32Hash = this.convertme.base32;
            }
        }

        public string SHA1Raw
        {
            get
            {
                return this.SHA1RawHash;
            }
            set
            {
                this.convertme = new HashChanger();
                this.convertme.rawhash = value;
                this.SHA1Base32Hash = this.convertme.base32;
                this.SHA1RawHash = value;
            }
        }

        public byte[] TigerBytes
        {
            get
            {
                this.convertme = new HashChanger();
                this.convertme.rawhash = this.TigerTreeRootRawHash;
                return this.convertme.bytehash;
            }
            set
            {
                this.convertme = new HashChanger();
                this.convertme.bytehash = value;
                this.TigerTreeRootRawHash = this.convertme.rawhash;
            }
        }

        public string TigerHex
        {
            get
            {
                this.convertme = new HashChanger();
                this.convertme.rawhash = this.TigerTreeRootRawHash;
                return this.convertme.hexhash;
            }
            set
            {
                this.convertme = new HashChanger();
                this.convertme.hexhash = value;
                this.TigerTreeRootRawHash = this.convertme.rawhash;
                this.TigerTreeBase32Hash = this.convertme.base32;
            }
        }

        public string TigerRaw
        {
            get
            {
                return this.TigerTreeRootRawHash;
            }
            set
            {
                this.convertme = new HashChanger();
                this.convertme.rawhash = value;
                this.TigerTreeBase32Hash = this.convertme.base32;
                this.TigerTreeRootRawHash = value;
            }
        }

        public string TTH
        {
            get
            {
                return this.TigerTreeBase32Hash;
            }
            set
            {
                this.TigerTreeBase32Hash = value;
                this.TigerTreeRootRawHash = null;
            }
        }
    }
}

