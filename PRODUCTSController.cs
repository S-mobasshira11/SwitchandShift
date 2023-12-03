using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Switch_and_Shift.Models;
using Microsoft.AspNetCore.Http;


namespace Switch_and_Shift.Controllers
{
    public class PRODUCTSController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PRODUCTSController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: PRODUCTS, if there is any problem in running index, uncomment this
/*        [HttpGet]
        public async Task<IActionResult> Index()
        {                   
            return View(await _context.PRODUCTS.ToListAsync());
        }*/

        [HttpGet]
        public async Task<IActionResult> Index(string productSearch, int? upperRange, int? lowerRange)
        {
            int currentUserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            ViewData["GetProductDetails"] = productSearch;            
            ViewData["GetUpperRange"] = upperRange;            
            ViewData["GetLowerRange"] = lowerRange;            
            var productQuery = from x in _context.PRODUCTS select x;
            productQuery = productQuery.Where(x => x.UserId != currentUserId);
            if (!String.IsNullOrEmpty(productSearch))
            {
                productQuery = productQuery.Where(x => x.product_category.Contains(productSearch));
            }
            if (upperRange!=null)
            {
                productQuery = productQuery.Where(x => x.product_price <= upperRange );
            }
            if (lowerRange != null)
            {
                productQuery = productQuery.Where(x => x.product_price >= lowerRange);
            }
            return View(await productQuery.AsNoTracking().ToListAsync());
        }


        // GET: PRODUCTS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pRODUCTS = await _context.PRODUCTS
                .FirstOrDefaultAsync(m => m.Product_ID == id);
            if (pRODUCTS == null)
            {
                return NotFound();
            }

            return View(pRODUCTS);
        }

        // GET: PRODUCTS/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PRODUCTS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Product_ID,product_category,product_price,product_brand,product_model,product_details,product_warranty,product_usage,product_condition,UserId,ImageFile")] PRODUCTS productsModel)
        {
            
            if (ModelState.IsValid)
            {
                productsModel.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                string wwwrootPath = _hostEnvironment.WebRootPath;
                string FileName = Path.GetFileNameWithoutExtension(productsModel.ImageFile.FileName);
                string extension = Path.GetExtension(productsModel.ImageFile.FileName);
                productsModel.image_name = FileName = FileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwrootPath + "/Image/" + FileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await productsModel.ImageFile.CopyToAsync(fileStream);
                }
                _context.Add(productsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productsModel);
        }

        
        [HttpGet]
        public async Task<IActionResult> MyProducts()
        {
            int currentUserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            var productQuery = from x in _context.PRODUCTS select x;
            productQuery = productQuery.Where(x => x.UserId == currentUserId);
            return View(await productQuery.AsNoTracking().ToListAsync());
        }

        // GET: PRODUCTS/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pRODUCTS = await _context.PRODUCTS.FindAsync(id);
            if (pRODUCTS == null)
            {
                return NotFound();
            }
            return View(pRODUCTS);
        }

        // POST: PRODUCTS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Product_ID,product_category,product_price,product_brand,product_model,product_details,product_warranty,product_usage,product_condition,UserId,image_name")] PRODUCTS pRODUCTS)
        {
            if (id != pRODUCTS.Product_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pRODUCTS);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PRODUCTSExists(pRODUCTS.Product_ID))
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
            return View(pRODUCTS);
        }

        // GET: PRODUCTS/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pRODUCTS = await _context.PRODUCTS
                .FirstOrDefaultAsync(m => m.Product_ID == id);
            if (pRODUCTS == null)
            {
                return NotFound();
            }

            return View(pRODUCTS);
        }

        // POST: PRODUCTS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.PRODUCTS.FindAsync(id);


            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", product.image_name);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }


            _context.PRODUCTS.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PRODUCTSExists(int id)
        {
            return _context.PRODUCTS.Any(e => e.Product_ID == id);
        }
    }
}
