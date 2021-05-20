using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace mapaNawigacja
{
    class DaneGeograficzne
    {
        public static BasicGeoposition pktStartowy;
        public static BasicGeoposition pktDocelowy;
        public static string opisCelu = null;
        public static string odleglosc = null;
    }
}
