using System.Runtime.Serialization;

namespace PB.Viewings
{
    [DataContract]
    public class LegacyAdvert
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public LegacyAdvertStatus status { get; set; }
        [DataMember]
        public bool accompaniedViewings { get; set; }
    }
}