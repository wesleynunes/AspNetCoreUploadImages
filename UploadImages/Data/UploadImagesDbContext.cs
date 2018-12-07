using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UploadImages.Models;

namespace UploadImages.Data
{
    public class UploadImagesDbContext : DbContext
    {
        public UploadImagesDbContext(DbContextOptions<UploadImagesDbContext> options) : base(options)
        {

        }

        public DbSet<UploadImage> UploadImages { get; set; }
    }
}
