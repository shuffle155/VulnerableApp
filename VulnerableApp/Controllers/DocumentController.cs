using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VulnerableApp.Models;

namespace VulnerableApp.Controllers
{
    public class DocumentController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            using (var context = new ApplicationDbContext())
            {
                var documents = new List<Document>();
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (User.IsInRole("Admin"))
                {
                    documents = context.Documents.ToList();
                }
                else
                {
                    int uid = int.TryParse(userId, out int parsedUserId) ? parsedUserId : 0;
                    documents = context.Documents.Where(d => d.OwnerId == uid).ToList();
                }
                return View(documents);
            }
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var document = context.Documents.FirstOrDefault(d => d.Id == id);
                if (document == null)
                {
                    return NotFound();
                }
                return View(document);
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var document = context.Documents.FirstOrDefault(d => d.Id == id);
                if (document == null)
                {
                    return NotFound();
                }
                return View(document);
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        public IActionResult Edit(int id, string content)
        {
            using (var context = new ApplicationDbContext())
            {
                var document = context.Documents.FirstOrDefault(d => d.Id == id);
                if (document == null)
                {
                    return NotFound();
                }
                document.Content = content;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Delete(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var document = context.Documents.FirstOrDefault(d => d.Id == id);
                if (document == null)
                {
                    return NotFound();
                }
                context.Documents.Remove(document);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
