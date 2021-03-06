﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolNetwork.Data;
using SchoolNetwork.Models;
using SchoolNetwork.Models.ViewModels;
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

        public async Task<IActionResult> Index()
        {
            var courses = _context.Assignments
                .Include(c => c.ApplicationUser)
                .Include(c => c.Course)
                .AsNoTracking();
            return View(await courses.ToListAsync());
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
                .Include(c => c.Reviews)
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

            var assignmentViewModel = new AssignmentViewModel();

            assignmentViewModel.Assignment = assignment;

            foreach (var question in assignment.Questions)
            {
                assignmentViewModel.Questions.Add(question);

                foreach (var answer in question.Answers)
                {
                    assignmentViewModel.Choices.Add(new ChoiceViewModel
                    {
                        AnswerID = answer.AnswerID,
                    });
                }
            }

            return View(assignmentViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Start(AssignmentViewModel assignmentViewModel)
        {

            // if multiple correct answers do something else...

            var result = new Result();
            var choices = new List<Choice>();

            result.AssignmentID = assignmentViewModel.Assignment.AssignmentID;
            result.ResultDate = DateTime.Now;
            result.ApplicationUserID = await GetCurrentUserId();

            foreach (var c in assignmentViewModel.Choices)
            {
                if (c.IsSelected)
                {
                    var tempAnswer = _context.Answers.Find(c.AnswerID);
                    var tempQuestion = _context.Questions.Find(tempAnswer.QuestionID);

                    choices.Add(new Choice
                    {
                        ResultID = result.ResultID,
                        AnswerID = c.AnswerID,
                        QuestionID = tempAnswer.QuestionID
                    });

                    if (tempAnswer.Value)
                    {
                        result.Score += tempQuestion.Value;
                    }
                }
            }

            result.Choices = choices;

            foreach (var c in choices)
            {
                _context.Add(c);
            }

            _context.Add(result);
            await _context.SaveChangesAsync();

            return Redirect("/Assignment/Finish/" + result.ResultID);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Finish(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var result = await _context.Results
                .Include(a => a.Choices)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ResultID == id);

            if (result.ApplicationUserID != await GetCurrentUserId())
            {
                return BadRequest();
            }

            var assignment = await _context.Assignments
                .Include(c => c.Questions)
                    .ThenInclude(d => d.Answers)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AssignmentID == result.AssignmentID);

            var resultViewModel = new ResultViewModel();

            resultViewModel.Result = result;
            resultViewModel.Assignment = assignment;

            foreach (var question in assignment.Questions)
            {
                resultViewModel.Questions.Add(question);

                foreach (var answer in question.Answers)
                {
                    resultViewModel.Answers.Add(answer);
                }
            }

            foreach (var choice in result.Choices)
            {
                resultViewModel.Choices.Add(choice);
            }

            return View(resultViewModel);
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

                foreach (Question q in questions)
                {
                    _context.Add(q);
                    assignment.Value += q.Value;

                    foreach (Answer a in answers)
                    {
                        if (q.QuestionID == a.QuestionID)
                        {
                            q.Answers.Add(a);
                        }
                    }
                }

                _context.Add(assignment);
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
        public async Task<IActionResult> Edit(Assignment assignment, List<Question> questions, List<Answer> answers)
        {
            if (ModelState.IsValid)
            {
                // add course options
                // also update assignment value

                var updatedAssignment = _context.Assignments.Find(assignment.AssignmentID);
                updatedAssignment.Questions = questions;
                _context.Add(assignment);

                foreach (Question question in questions)
                {
                    var questionExists = await _context.Questions.FindAsync(question.QuestionID);

                    question.AssignmentID = assignment.AssignmentID;

                    if (questionExists == null)
                    {
                        if (question.isDeleted != true)
                        {
                            _context.Add(question);
                        }
                    } 
                    else
                    {
                        if (question.isDeleted != true)
                        {
                            _context.Update(question);
                        }
                        else
                        {
                            _context.Remove(question);
                        }
                    }

                    foreach (Answer answer in answers)
                    {
                        if (question.QuestionID == answer.QuestionID)
                        {

                            var answerExists = await _context.Answers.FindAsync(answer.AnswerID);

                            answer.QuestionID = question.QuestionID;

                            if (answerExists == null)
                            {
                                if (answer.isDeleted != true)
                                {
                                    _context.Add(answer);
                                }
                            }
                            else
                            {
                                if (answer.isDeleted != true)
                                {
                                    _context.Update(answer);
                                }
                                else
                                {
                                    _context.Remove(answer);
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(assignment);
        }

        [Authorize]
        public async Task<IActionResult> Review(int id)
        {
            ViewBag.AssignmentID = id;
            ViewBag.ApplicationUserID = await GetCurrentUserId();

            return View(new Review());
        }

        [HttpPost]
        public async Task<IActionResult> Review([Bind("ReviewID, AssignmentID, ApplicationUserID, Title, Description, Score")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
            }

            return Redirect("/Assignment/Details/" + review.AssignmentID);
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