using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Logging;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;

namespace ContosoUniversity.Controllers
{
    public class MechanicController : Controller
    {
        private TaxiDepotContext db = new TaxiDepotContext();
        private ILogger _logger = new Logger();
        public ActionResult Index(int? id, int? courseID)
        {
            var viewModel = new MechanicIndexData();
            viewModel.Mechanics = db.Mechanics
                .Include(i => i.Box)
                .Include(i => i.Cars.Select(c => c.Department))
                .OrderBy(i => i.LastName);

            if (id != null)
            {
                ViewBag.MechanicID = id.Value;
                viewModel.Cars = viewModel.Mechanics.Where(
                    i => i.ID == id.Value).Single().Cars;
            }

            if (courseID != null)
            {
                ViewBag.CarID = courseID.Value;

                var selectedCars = viewModel.Cars.Where(x => x.CarID == courseID).Single();
                db.Entry(selectedCars).Collection(x => x.Contracts).Load();

                foreach (Contract contract in selectedCars.Contracts)
                {
                    db.Entry(contract).Reference(x => x.Driver).Load();
                }

                viewModel.Contracts = selectedCars.Contracts;
            }

            return View(viewModel);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mechanic instructor = db.Mechanics.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        public ActionResult Create()
        {
            var instructor = new Mechanic();
            instructor.Cars = new List<Car>();
            PopulateAssignedCourseData(instructor);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstMidName,HireDate,Box")] Mechanic instructor, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                instructor.Cars = new List<Car>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = db.Cars.Find(int.Parse(course));
                    instructor.Cars.Add(courseToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                db.Mechanics.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mechanic instructor = db.Mechanics
                .Include(i => i.Box)
                .Include(i => i.Cars)
                .Where(i => i.ID == id)
                .Single();
            if (instructor == null)
            {
                return HttpNotFound();
            }
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        private void PopulateAssignedCourseData(Mechanic instructor)
        {
            var allCars = db.Cars;
            var instructorCourses = new HashSet<int>(instructor.Cars.Select(c => c.CarID));
            var viewModel = new List<AssignedCarData>();
            foreach (var car in allCars)
            {
                viewModel.Add(new AssignedCarData
                {
                    CarID = car.CarID,
                    Title = car.Title,
                    Assigned = instructorCourses.Contains(car.CarID)
                });
            }
            ViewBag.Cars = viewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var instructorToUpdate = db.Mechanics
               .Include(i => i.Box)
               .Include(i => i.Cars)
               .Where(i => i.ID == id)
               .Single();

            if (TryUpdateModel(instructorToUpdate, "",
               new string[] { "LastName", "FirstMidName", "HireDate", "Box" }))
            {
                try
                {
                    UpdateInstructorCourses(selectedCourses, instructorToUpdate);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException dex)
                {
                    _logger.Error(dex.Message);
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateAssignedCourseData(instructorToUpdate);
            return View(instructorToUpdate);
        }

        private void UpdateInstructorCourses(string[] selectedCourses, Mechanic instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.Cars = new List<Car>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (instructorToUpdate.Cars.Select(c => c.CarID));

            foreach (var course in db.Cars)
            {
                if (selectedCoursesHS.Contains(course.CarID.ToString()))
                {
                    if (!instructorCourses.Contains(course.CarID))
                    {
                        instructorToUpdate.Cars.Add(course);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.CarID))
                    {
                        instructorToUpdate.Cars.Remove(course);
                    }
                }
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mechanic instructor = db.Mechanics.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mechanic instructor = db.Mechanics
              .Include(i => i.Box)
              .Where(i => i.ID == id)
              .Single();

            db.Mechanics.Remove(instructor);

            var department = db.Departments
                .Where(d => d.MechanicID == id)
                .SingleOrDefault();

            if (department != null)
            {
                department.MechanicID = null;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
