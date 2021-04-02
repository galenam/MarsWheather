using System;
using System.Collections.Generic;

namespace Mars
{
    public class RoverInfoDTO
    {
        public DateTime landing_date { get; set; }
        public DateTime launch_date { get; set; }
        public string status { get; set; }
    }
    public class MarsPhotoDTO
    {
        public string img_src { get; set; }
        public DateTime earth_date { get; set; }
        public RoverInfoDTO rover { get; set; }
    }

    public class MarsPhotosDTO
    {
        public IEnumerable<MarsPhotoDTO> photos { get; set; }
    }
}