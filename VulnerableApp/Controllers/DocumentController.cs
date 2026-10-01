using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VulnerableApp.Models;

namespace VulnerableApp.Controllers
{
    public class DocumentController : Controller
    {
        private readonly ApplicationDbContext context;

        public DocumentController(ApplicationDbContext context)
        {
            this.context = context;
        }

        private Document GetDocument(int id)
        {
            var document = context.Documents.FirstOrDefault(d => d.Id == id);
            return document;
        }

        [Authorize]
        public IActionResult Index()
        {
            var documents = new List<Document>();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (User.IsInRole("Admin"))
            {
                documents = context.Documents.ToList();
            }
            else
            {
                int uid = int.Parse(userId.Value);
                documents = context.Documents.Where(d => d.OwnerId == uid)
                    .ToList();
            }
            return View(documents);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            Document document = GetDocument(id);
            if (document == null)
            {
                return NotFound();
            }
            return View(document);

        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Document document = GetDocument(id);
            if (document == null)
            {
                return NotFound();
            }
            return View(document);

        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        public IActionResult Edit(int id, string content)
        {
            Document document = GetDocument(id);
            if (document == null)
            {
                return NotFound();
            }
            document.Content = content;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Document document = GetDocument(id);
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