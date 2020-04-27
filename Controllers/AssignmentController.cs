using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolNetwork.Data;
using SchoolNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolNetwork.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AssignmentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments
                .Include(a => a.ApplicationUser)
                .Include(b => b.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AssignmentID == id);

            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Start(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments
                .Include(a => a.ApplicationUser)
                .Include(b => b.Course)
                .Include(c => c.Questions)
                    .ThenInclude(d => d.Answers)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AssignmentID == id);

            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        /*
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Start([Bind("AssignmentID")] Assignment assignment, List<Question> questions, List<Answer> answers, bool[] isChecked)
        {
            var result = new Result();

            result.AssignmentID = assignment.AssignmentID;
            result.ResultDate = DateTime.Now;
            result.ApplicationUserID = await GetCurrentUserId();

            TempData["Check"] = questions.Count() + "," + answers.Count() + "," + isChecked.Length;

            var choices = new List<Choice>();

            foreach (Question q in questions)
            {
                foreach (Answer a in answers)
                {
                    if (a.QuestionID == q.QuestionID)
                    {
                        if (isChecked[a.AnswerID] == true)
                        {
                            choices.Add(new Choice
                            {
                                QuestionID = q.QuestionID,
                                AnswerID = a.AnswerID
                            });

                            if (a.Value)
                            {
                                result.Score += q.Value;
                            }
                        }
                        else
                        {
                            return View(assignment);
                        }
                    }
                }
            }

            foreach (var c in choices)
            {
                _context.Add(c);
            }

            result.Choices = choices;

            _context.Add(result);
            await _context.SaveChangesAsync();

            return RedirectToAction("Finish");
        }
        */

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Finish()
        {
            ViewBag.Check = TempData["Check"];
            return View();
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet]
        public IActionResult Create()
        {
            var assignment = new Assignment
            {
                Questions = new List<Question>(),
            };

            return View(assignment);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("AssignmentID, Title, Value")]  Assignment assignment, [Bind("QuestionID, Title, Value")]  List<Question> questions, [Bind("AnswerID, Title, Value")]  List<Answer> answers)
        {
            if (ModelState.IsValid)
            {
                assignment.CourseID = 1;
                assignment.ApplicationUserID = await GetCurrentUserId();
                assignment.Questions = questions;
                _context.Add(assignment);

                foreach (Question q in questions)
                {
                    _context.Add(q);

                    foreach (Answer a in answers)
                    {
                        if (q.QuestionID == a.QuestionID)
                        {
                            q.Answers.Add(a);
                        }
                    }

                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(assignment);
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments
                .Include(a => a.ApplicationUser)
                .Include(b => b.Course)
                .Include(c => c.Questions)
                    .ThenInclude(d => d.Answers)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AssignmentID == id);

            if (assignment.ApplicationUserID != await GetCurrentUserId())
            {
                return BadRequest();
            }

            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("AssignmentID, Title, Value")]  Assignment assignment, [Bind("QuestionID, Title, Value, isDeleted")]  List<Question> questions, [Bind("AnswerID, Title, Value, isDeleted")]  List<Answer> answers)
        {
            if (ModelState.IsValid)
            {
                // if exist: update else add
                // if deleted and exist: delete else ignore
                // add course options

                if (assignment.ApplicationUserID != await GetCurrentUserId())
                {
                    return BadRequest();
                }

                assignment.CourseID = 1;
                assignment.Questions = questions;
                _context.Add(assignment);

                foreach (Question q in questions)
                {
                    _context.Add(q);

                    foreach (Answer a in answers)
                    {
                        if (q.QuestionID == a.QuestionID)
                        {
                            q.Answers.Add(a);
                        }
                    }

                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(assignment);
        }

        public PartialViewResult AddQuestion()
        {
            return PartialView("_Question", new Question());
        }

        public PartialViewResult AddAnswer([FromQuery(Name = "questionPrefix")] string prefix)
        {
            ViewData["Prefix"] = "Questions[" + prefix + "]";
            return PartialView("_Answer", new Answer());
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [HttpGet]
        public async Task<int> GetCurrentUserId()
        {
            ApplicationUser usr = await GetCurrentUserAsync();
            return usr.Id;
        }
    }
}