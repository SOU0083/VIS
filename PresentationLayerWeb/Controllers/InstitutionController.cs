using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.DataMapper;
using DomainLayer.DomainModel;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayerWeb.Controllers
{
    public class InstitutionController : Controller
    {
        private EventMapper eventMapper = new EventMapper();
        private ReservationMapper reservationMapper = new ReservationMapper();

        public IActionResult Events()
        {
            return View(eventMapper.FindDescendantsEvents("/3/"));
        }
        // GET: Institution/Delete/5
        public ActionResult EventsDelete(int id)
        {
            ViewBag.EventId = id;
            return View();
        }
        // POST: Institution/Delete/5
        public ActionResult EventsDeleteConfirmed(int id)
        {
            List<Reservation> rList = reservationMapper.FindEventReservations(id);
            foreach (Reservation r in rList)
                r.Reservation_Customer.WriteEmailAndPayBack("Vaše rezervace ID: " + r.Id + " byla zrušena.");

            reservationMapper.DeleteEventReservations(id);
            eventMapper.Delete(id);

            return RedirectToAction(nameof(Events));
        }
        public IActionResult Objects()
        {
            return View();
        }
    }
}