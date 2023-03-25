using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.DataMapper;
using DomainLayer.DomainModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayerWeb.Controllers
{
    public class AdminController : Controller
    {
        private InstitutionMapper im = new InstitutionMapper();
        private InstitutionCategoryMapper icm = new InstitutionCategoryMapper();
        private StructuralObjectMapper structuralObjectMapper = new StructuralObjectMapper();
        private EventMapper eventMapper = new EventMapper();
        private ReservationMapper reservationMapper = new ReservationMapper();

        public ActionResult Statistics()
        {
            return View(icm.FindStatistics());
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View(im.FindWithObject());
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(String id)
        {
            ViewBag.Hierarchy = id;
            return View();
        }

        // POST: Admin/Delete/5
        public ActionResult DeleteConfirmed(String id)
        {
            id = "/" + id + "/";
            List<Reservation> rList = reservationMapper.FindDescendantsReservations(id);
            foreach (Reservation r in rList)
                r.WriteEmailToCustomer("Vaše rezervace ID: " + r.Id + " byla zrušena.");

            reservationMapper.DeleteDescendantsReservations(id);
            eventMapper.DeleteDescendantsEvents(id);
            structuralObjectMapper.DeleteDescendants(id);

            return RedirectToAction(nameof(Index));
        }
    }
}