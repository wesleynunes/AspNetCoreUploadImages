using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UploadImages.Data;
using UploadImages.Models;

namespace UploadImages.Controllers
{
    public class UploadImagesController : Controller
    {
        private readonly UploadImagesDbContext _context;
        private readonly IHostingEnvironment _hostingEnv;

        public UploadImagesController(UploadImagesDbContext context, IHostingEnvironment hostingEnv)
        {
            _context = context;
            _hostingEnv = hostingEnv;
        }

        // GET: UploadImages
        public async Task<IActionResult> Index()
        {
            return View(await _context.UploadImages.ToListAsync());
        }

        // GET: UploadImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadImage = await _context.UploadImages
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (uploadImage == null)
            {
                return NotFound();
            }

            return View(uploadImage);
        }

        // GET: UploadImages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UploadImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UploadImage uploadImage)
        {
            string uploadPath = "uploads/images/";
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        var uploadPathWithfileName = Path.Combine(uploadPath, fileName);

                        var uploadAbsolutePath = Path.Combine(_hostingEnv.WebRootPath, uploadPathWithfileName);

                        using (var fileStream = new FileStream(uploadAbsolutePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            uploadImage.ImageFile = uploadPathWithfileName;
                         
                        }
                    }
                }
                _context.Add(uploadImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return View(uploadImage);

            //if (ModelState.IsValid)
            //{
            //    _context.Add(uploadImage);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(uploadImage);
        }

        // GET: UploadImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadImage = await _context.UploadImages.FindAsync(id);
            if (uploadImage == null)
            {
                return NotFound();
            }
            return View(uploadImage);
        }

        // POST: UploadImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UploadImage uploadImage)
        {
            if (id != uploadImage.ImageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    var files = HttpContext.Request.Form.Files;
                    string uploadPath = "uploads/images/";

                    foreach (var file in files)
                    {
                        if (file != null && file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                            var uploadPathWithfileName = Path.Combine(uploadPath, fileName);

                            var uploadAbsolutePath = Path.Combine(_hostingEnv.WebRootPath, uploadPathWithfileName);

                            using (var fileStream = new FileStream(uploadAbsolutePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                uploadImage.ImageFile = uploadPathWithfileName;
                            }
                        }
                    }
                    _context.Update(uploadImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UploadImageExists(uploadImage.ImageId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(uploadImage);


            //if (id != uploadImage.ImageId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(uploadImage);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!UploadImageExists(uploadImage.ImageId))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(uploadImage);
        }

        // GET: UploadImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadImage = await _context.UploadImages
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (uploadImage == null)
            {
                return NotFound();
            }

            return View(uploadImage);
        }

        // POST: UploadImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uploadImage = await _context.UploadImages.FindAsync(id);
            _context.UploadImages.Remove(uploadImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UploadImageExists(int id)
        {
            return _context.UploadImages.Any(e => e.ImageId == id);
        }
    }
}
