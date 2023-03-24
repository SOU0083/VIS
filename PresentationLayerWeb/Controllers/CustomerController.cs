using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.DataMapper;
using DomainLayer.DomainModel;
using DomainLayer.DTO;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayerWeb.Controllers
{
    public class CustomerController : Controller
    {
        private EventMapper eventMapper = new EventMapper();
        private ReservationMapper reservationMapper = new ReservationMapper();

        public IActionResult Events()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Search(SearchEventForm form)
        {
            if (!DateTime.TryParse(form.From, out DateTime dt) || !DateTime.TryParse(form.To, out DateTime dt2))
            {
                return Content("Chybný formát data.");
            }
            return View(eventMapper.Search(dt, dt2, form.Price));
        }
        public IActionResult Reserve(int id, DateTime canReserveFrom, DateTime canReserveTo, int eventId, DateTime start, DateTime end)
        {
            Event e = new Event()
            {
                Id = id,
                CanReserveFrom = canReserveFrom,
                CanReserveTo = canReserveTo,
            };
            if (!e.CanReserve())
                ViewBag.Text = "Rezervace probíhá od " + e.CanReserveFrom.ToString() + " do " + e.CanReserveTo.ToString();
            else if (!eventMapper.CanReserve(e.Id))
                ViewBag.Text = "Již nejsou volná místa";
            else
            {
                reservationMapper.Insert(new Reservation(0, new Customer { Id = 1 }, new ReservationObject { Id = eventId }, e, start, end, null));
                ViewBag.Text = "Rezervace proběhla úspěšně.";
            }
            return View();
        }
        public IActionResult Objects()
        {
            return View();
        }
        public IActionResult Reservations()
        {
            return View(reservationMapper.FindCustomerReservations(1));
        }
    }
}