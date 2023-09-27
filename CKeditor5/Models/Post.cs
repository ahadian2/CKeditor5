using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CKeditor5.Models
{
    [Table(name:"T-Post")]
    public class Post
    {
        [Key]
        [Required]
        [Display(Name ="شناسه پست")]
        public int PostID { get; set; }
        [Required]
        [MaxLength(500)]
        [Display(Name = "تایتل")]
        public string PostTitle { get; set; }
        [Display(Name = "محتوا")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string PostContent { get; set; }

    }
}