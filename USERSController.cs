using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Switch_and_Shift.Models;
using Microsoft.AspNetCore.Identity;

namespace Switch_and_Shift.Controllers
{
    public class USERSController : Controller
    {
        private readonly ApplicationDbContext db;

        public object Server { get; private set; }

        public USERSController(ApplicationDbContext _db)
        {
            db = _db;
        }

        int id;
        string password;

       

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(USERS uSER)
        {

            if (db.USERS.Any(x => x.Email == uSER.Email))
            {
                ModelState.Clear();
                ViewBag.Notification = "An account with this email already exits";
            }
            else
            {

                db.USERS.Add(uSER);
                db.SaveChanges();
                ModelState.Clear();
                HttpContext.Session.SetString("UserId", uSER.UserId.ToString());
                HttpContext.Session.SetString("Email", uSER.Email.ToString());
                
                //Session["Password"] = uSER.Password.ToString();
                return RedirectToAction("Index", "Home");

            }
            return View();
        }


        [HttpPost]
        public ActionResult SignUpToRedirect()
        {            
            return RedirectToAction("SignUp", "USERS");
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult LogIn(USERS uSER)
        {
            var checklogin = db.USERS.Where(x => x.Email.Equals(uSER.Email) && x.Password.Equals(uSER.Password)).FirstOrDefault();

            if (checklogin != null)
            {
                    
                HttpContext.Session.SetString("Email", checklogin.Email.ToString());
                HttpContext.Session.SetString("FirstName", checklogin.FirstName.ToString());
                HttpContext.Session.SetString("UserId", checklogin.UserId.ToString());
                id = checklogin.UserId;
                password = checklogin.Password;
               
                
                ModelState.Clear();
                return RedirectToAction("Welcome", "Home");
            }
            else
            {
                ModelState.Clear();
                ViewBag.Notification = "Wrong username or password";
            }
            return View();
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.SetString("Email", null);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ViewProfile()
        {
            if(HttpContext.Session.GetString("Email")!=null)
            {
                string email = HttpContext.Session.GetString("Email");
                var userdetails = db.USERS.Where(c => c.Email.Equals(email)).FirstOrDefault();
                return View(userdetails);
            }
            else
            {
                return View();
            }         
        }

        [HttpGet]
        public ActionResult UserReview(int? id)
        {
            var reviewee = db.USERS.Where(c => c.UserId == id).FirstOrDefault();
            ViewBag.Name = reviewee.FirstName + " " + reviewee.LastName;
            ViewBag.Email = reviewee.Email;
            return View();
        }

        [HttpPost]
         public ActionResult UserReview(USERREVIEW uSERREVIEW)
        {
            db.USERREVIEW.Add(uSERREVIEW);
            db.SaveChanges();
            return RedirectToAction("Welcome", "Home");
        }
        [HttpGet]
        public ActionResult ShowReview(USERREVIEW uSERREVIEW)
        {
            var reviewer = db.USERS.Where(c => c.UserId == id).FirstOrDefault();
            ViewBag.Name = reviewer.FirstName + " " + reviewer.LastName;
            ViewBag.Email = reviewer.Email;
            ViewBag.Description = db.USERREVIEW.ToListAsync();

            return View();
        }
       







        // GET: USERS
        public async Task<IActionResult> Index()
        {
            return View(await db.USERS.ToListAsync());
        }

        // GET: USERS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uSERS = await db.USERS
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (uSERS == null)
            {
                return NotFound();
            }

            return View(uSERS);
        }

        // GET: USERS/Create
        /*public IActionResult Create()
        {
            return View();
        }

        // POST: USERS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,GamingName,Location,Email,Phone,Password")] USERS uSERS)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uSERS);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uSERS);
        }*/
    
       
        //GET: USERS/Edit/5
        public ActionResult Edit()
        {
            string userMail = HttpContext.Session.GetString("Email");
            var userDetails = db.USERS.Where(x => x.Email.Equals(userMail)).FirstOrDefault();
            return View(userDetails);
        }

        // POST: USERS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(USERS uSERS)
        {
            if (HttpContext.Session.GetString("Email")==null)
            {
                return NotFound();
            }

            if (HttpContext.Session.GetString("Email")!=null)
            {
                    string email = HttpContext.Session.GetString("Email");
                    var checklogin = db.USERS.Where(x => x.Email.Equals(email)).FirstOrDefault();
                
                    checklogin.FirstName = uSERS.FirstName;
                    checklogin.LastName = uSERS.LastName;
                    checklogin.District = uSERS.District;
                    checklogin.Location = uSERS.Location;
                    checklogin.Email = uSERS.Email;
                    checklogin.Phone = uSERS.Phone;
                    checklogin.Password = checklogin.Password;
                    db.Update(checklogin);
                    db.SaveChanges();
                    return RedirectToAction(nameof(ViewProfile));
                
               
            }
            return View();
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(USERS uSERS)
        {
            if (uSERS.Email != null)
            {
                string email = uSERS.Email;
                var checklogin = db.USERS.Where(x => x.Email.Equals(email)).FirstOrDefault();

                checklogin.FirstName = checklogin.FirstName;
                checklogin.LastName = checklogin.LastName;
                checklogin.District = checklogin.District;
                checklogin.Location = checklogin.Location;
                checklogin.Email = checklogin.Email;
                checklogin.Phone = checklogin.Phone;
                checklogin.Password = uSERS.Password;
                db.Update(checklogin);
                db.SaveChanges();
                return RedirectToAction(nameof(LogIn));


            }
            return View();
        }


        /*// GET: USERS/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uSERS = await _context.USERS
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (uSERS == null)
            {
                return NotFound();
            }

            return View(uSERS);
        }

        // POST: USERS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uSERS = await _context.USERS.FindAsync(id);
            _context.USERS.Remove(uSERS);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool USERSExists(int id)
        {
            return db.USERS.Any(e => e.UserId == id);
        }
    }

    public class HttpPostedFileBase
    {
        public int ContentLength { get; internal set; }

        internal void SaveAs(string path)
        {
            throw new NotImplementedException();
        }
    }
}
