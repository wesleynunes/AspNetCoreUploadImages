using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UploadImages.Models
{
    public class UploadImage
    {
        [Key]
        public int ImageId { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Imagens")]
        public string ImageFile { get; set; }

        
    }
}
