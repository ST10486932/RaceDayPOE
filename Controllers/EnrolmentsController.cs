using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay2.Data;
using RaceDay2.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RaceDay2.Controllers;

[Authorize]
public class EnrolmentsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _users;

    public EnrolmentsController(ApplicationDbContext db, UserManager<ApplicationUser> users)
    {
        _db = db;
        _users = users;
    }

    // 1. PARTICIPANT: View their own enrolments
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> My()
    {
        var userId = _users.GetUserId(User);

        var myEnrolments = await _db.Enrolments
            .Include(e => e.Event)
            .Include(e => e.Category)
            .Where(e => e.ParticipantId == userId)
            .OrderBy(e => e.Event!.Date)
            .ToListAsync();

        return View(myEnrolments);
    }

    // 2. ORGANISER: View all participants enrolled in a specific event
    [Authorize(Roles = "Admin,Organiser")]
    public async Task<IActionResult> ForEvent(int eventId)
    {
        var ev = await _db.Events.FindAsync(eventId);
        if (ev == null) return NotFound();

        var userId = _users.GetUserId(User);
        if (ev.OrganiserId != userId && !User.IsInRole("Admin")) return Forbid();

        var enrolments = await _db.Enrolments
            .Include(e => e.Category)
            .Include(e => e.Participant)
            .Where(e => e.EventId == eventId)
            .ToListAsync();

        ViewBag.EventName = ev.Name;
        ViewBag.EventId = ev.Id;

        // This automatically looks for Views/Enrolments/ForEvent.cshtml
        return View(enrolments);
    }

    // 3. PARTICIPANT: Actually enroll in an event
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> Create(int eventId, int categoryId)
    {
        var userId = _users.GetUserId(User);
        if (userId == null) return Challenge();

        var alreadyEnrolled = await _db.Enrolments
            .AnyAsync(e => e.EventId == eventId && e.ParticipantId == userId);

        if (alreadyEnrolled)
        {
            TempData["Error"] = "You are already enrolled in this event!";
            return RedirectToAction("Details", "Events", new { id = eventId });
        }

        var enrolment = new Enrolment
        {
            EventId = eventId,
            CategoryId = categoryId,
            ParticipantId = userId,
            Status = "Pending",
            EnrolledOn = DateTime.UtcNow
        };

        _db.Enrolments.Add(enrolment);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Successfully enrolled! Your status is currently Pending.";
        return RedirectToAction("Details", "Events", new { id = eventId });
    }

    // 4. PARTICIPANT: Cancel their enrolment
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> Cancel(int id)
    {
        var enrolment = await _db.Enrolments.FindAsync(id);
        if (enrolment == null) return NotFound();

        var userId = _users.GetUserId(User);
        if (enrolment.ParticipantId != userId) return Forbid();

        _db.Enrolments.Remove(enrolment);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Enrolment cancelled.";
        return RedirectToAction(nameof(My));
    }
}