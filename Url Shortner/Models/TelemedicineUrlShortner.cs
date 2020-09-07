using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Url_Shortner.Models
{
    [Table("af_tbl_telemedicine_short_url")]
    public class TelemedicineUrlShortner
    {
        [Key]
        [Column("id")]
        public long id { set; get; }
        [Column("long_url")]
        public string longUrl { set; get; }
        [Column("short_url")]
        public string shortUrl { set; get; }
        [Column("token")]
        public string token { set; get; }
        [Column("clicked")]
        public long clicked { set; get; }
        [Column("created_by")]
        public string createdBy { set; get; }
        [Column("created_date")]
        public DateTime createdDate { set; get; }
        [Column("modfied_by")]
        public string modfiedBy { set; get; }
        [Column("modified_date")]
        public DateTime modifiedDate { set; get; }
        [Column("deleted")]
        public bool deleted { set; get; }
    }
}