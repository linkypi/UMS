using System;
using System.Text;
using Telerik.Windows.Controls.Map;

namespace UserMS
{
    public class BingChinaMapProvider : TiledProvider
    {
         /// <summary>
       /// Initializes a new instance of the MyMapProvider class.
       /// </summary>
        public BingChinaMapProvider()
             : base()
       {
           BingChinaMapSource source = new BingChinaMapSource();
             this.MapSources.Add(source.UniqueId, source);
       }
       /// <summary>
       /// Returns the SpatialReference for the map provider.
       /// </summary>
       public override ISpatialReference SpatialReference
       {
             get
             {
                    return new MercatorProjection();
             }
       }
    }
    public class BingChinaMapSource : TiledMapSource
    {
        /// <summary>
        /// Initializes a new instance of the MyMapSource class.
        /// </summary>
        public BingChinaMapSource()
            : base(1, 23, 256, 256)
        {
        }
        /// <summary>
        /// Initialize provider.
        /// </summary>
        public override void Initialize()
        {
            // Raise provider initialized event.
            this.RaiseIntializeCompleted();
        }
        /// <summary>
        /// Gets the image URI.
        /// </summary>
        /// <param name="tileLevel">Tile level.</param>
        /// <param name="tilePositionX">Tile X.</param>
        /// <param name="tilePositionY">Tile Y.</param>
        /// <returns>URI of image.</returns>
        public static string TileXYToQuadKey(int tileX, int tileY, int levelOfDetail)
        {
            StringBuilder quadKey = new StringBuilder();
            for (int i = levelOfDetail; i > 0; i--)
            {
                char digit = '0';
                int mask = 1 << (i - 1);
                if ((tileX & mask) != 0)
                {
                    digit++;
                }
                if ((tileY & mask) != 0)
                {
                    digit++;
                    digit++;
                }
                quadKey.Append(digit);
            }
            return quadKey.ToString();
        }
//        protected override Uri GetTile(int tileLevel, int tilePositionX, int tilePositionY)
//        {
//            int zoomLevel = ConvertTileToZoomLevel(tileLevel);
//            string quadkey =  TileXYToQuadKey(tilePositionX, tilePositionY, zoomLevel);
//
//            string uri = "http://r3.tiles.ditu.live.com/tiles/r"
//                + quadkey + ".png?g=47";
//            return new Uri(uri);
//
//        }
//        protected override Uri GetTile(int tileLevel, int tilePositionX, int tilePositionY)
//        {
//            int zoomLevel = ConvertTileToZoomLevel(tileLevel);
//            string str = "Galileo";
//            int length = new Random().Next(str.Length);
//            str = str.Substring(0, length);
//            var s = "https://mts{0}.google.com/vt/lyrs=m@228000000&hl=x-local&src=app&x={1}&y={2}&z={3}&s={4}";
//            var f = "http://mt{0}.google.com/vt/lyrs=m@104&hl=zh-CN&x={1}&y={2}&z={3}&s={4}.png";
//            return new Uri(string.Format(f, new object[] { tilePositionX % 4, tilePositionX, tilePositionY, zoomLevel, str }));
//        }
        protected override Uri GetTile(int tileLevel, int tilePositionX, int tilePositionY)
        {
            int zoomLevel = ConvertTileToZoomLevel(tileLevel);
            string str = "Galileo";
            int length = new Random().Next(str.Length);
            str = str.Substring(0, length);
            var s = "http://{0}.maps.nlp.nokia.com/maptile/2.1/maptile/newest/normal.day/{3}/{1}/{2}/256/png8?lg=chi&app_id=zWxWIRdjhcaAKHCSFJxj&token=rNUrWr0pJtbKFUzc4sN_PA";
            var f = "http://mt{0}.google.com/vt/lyrs=m@104&hl=zh-CN&x={1}&y={2}&z={3}&s={4}.png";
            return new Uri(string.Format(s, new object[] { tilePositionX % 3+1, tilePositionX, tilePositionY, zoomLevel, str }));
        }
        
    }
}