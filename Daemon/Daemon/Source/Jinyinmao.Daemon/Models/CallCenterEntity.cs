namespace Jinyinmao.Daemon.Models
{
    /// <summary>
    /// Class CallCenterEntity.
    /// </summary>
    public class CallCenterEntity
    {
        /// <summary>
        /// Gets or sets the customer unique identifier.
        /// </summary>
        //[JsonProperty("customerguid")]
        public string customerguid { get; set; }
        /// <summary>
        /// Gets or sets the register phone.
        /// </summary>
        //[JsonProperty("registerphone")]
        public string registerphone { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        //[JsonProperty("name")]
        public string name { get; set; }
        /// <summary>
        /// Gets or sets the papers number.
        /// </summary>
        //[JsonProperty("papersnumber")]
        public string papersnumber { get; set; }
        /// <summary>
        /// Gets or sets the register date.
        /// </summary>
        //[JsonProperty("registerdate")]
        public string registerdate { get; set; }
        /// <summary>
        /// Gets or sets the validate date.
        /// </summary>
        //[JsonProperty("validatedate")]
        public string validatedate { get; set; }
        /// <summary>
        /// Gets or sets the name of the customer level.
        /// </summary>
        //[JsonProperty("custlevelname")]
        public string custlevelname { get; set; }
        /// <summary>
        /// Gets or sets the name of the customer status.
        /// </summary>
        //[JsonProperty("custstatusname")]
        public string custstatusname { get; set; }
        /// <summary>
        /// Gets or sets the use platform.
        /// </summary>
        //[JsonProperty("useplatform")]
        public string useplatform { get; set; }
        /// <summary>
        /// Gets or sets the tel1.
        /// </summary>
        //[JsonProperty("tel1")]
        public string tel1 { get; set; }
        /// <summary>
        /// Gets or sets the tel2.
        /// </summary>
        //[JsonProperty("tel2")]
        public string tel2 { get; set; }
        /// <summary>
        /// Gets or sets the validatephone.
        /// </summary>
        //[JsonProperty("customerguid")]
        public string validatephone { get; set; }
        /// <summary>
        /// Gets or sets the sex.
        /// </summary>
        //[JsonProperty("sex")]
        public string sex { get; set; }
        /// <summary>
        /// Gets or sets the type of the papers.
        /// </summary>
        //[JsonProperty("paperstype")]
        public string paperstype { get; set; }
        /// <summary>
        /// Gets or sets the last contact date.
        /// </summary>
        //[JsonProperty("lastcontactdate")]
        public string lastcontactdate { get; set; }
        /// <summary>
        /// Gets or sets the name of the customer tag.
        /// </summary>
        //[JsonProperty("custtagname")]
        public string custtagname { get; set; }
        /// <summary>
        /// Gets or sets the birthday.
        /// </summary>
        //[JsonProperty("birthday")]
        public string birthday { get; set; }
        /// <summary>
        /// Gets or sets the path way.
        /// </summary>
        //[JsonProperty("pathway")]
        public string pathway { get; set; }
        /// <summary>
        /// Gets or sets the name of the service user.
        /// </summary>
        //[JsonProperty("serviceusername")]
        public string serviceusername { get; set; }

        public string channelid { get; set; }

        public string channelname { get; set; }
        /// <summary>
        /// Gets or sets the ob.
        /// </summary>
        //[JsonProperty("OB")]
        public string OB { get; set; }
        /// <summary>
        /// Gets or sets the ib.
        /// </summary>
        //[JsonProperty("IB")]
        public string IB { get; set; }
    }


    public class HjzxStateTable
    {
        public string UserIdentifier { get; set; }

        public string UserState { get; set; }
    }

    public class AgreementTable
    {
        public int Id { get; set; }
        public string ACode { get; set; }
        public string ChineseName { get; set; }
    }
}