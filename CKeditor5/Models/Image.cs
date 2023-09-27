using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace CKeditor5.Models
{
    [Table(name: "T-Image")]
    public class Image
    {
        [Key]
        [Required]
        [Display(Name = "شناسه تصویر")]
        public int ImageId { get; set; }
        [MaxLength(100)]
        [Display(Name = "نام تصویر")]
        public string ImageName { get; set; }
        [MaxLength(400)]
        [Display(Name = "توضیحات تصویر")]
        public string ImageDescription { get; set; }
    }
}