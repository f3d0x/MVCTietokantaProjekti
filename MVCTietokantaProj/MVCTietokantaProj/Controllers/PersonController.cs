using MVCTietokantaProj.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTietokantaProj.Controllers
{
    public class PersonController : Controller
    {
        // GET: Person
        public ActionResult Persons()
        {
            return View();
        }
        public JsonResult GetList()
        {
            MVCTietokantaEntities entities = new MVCTietokantaEntities();


            var model = (from h in entities.Henkilot
                         select new
                         {
                             HenkiloId = h.HenkiloId,
                             Etunimi = h.Etunimi,
                             Sukunimi = h.Sukunimi,
                             Osoite = h.Osoite,
                             Esimies = h.Esimies
                         }).ToList();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSinglePerson(string id)
        {
            MVCTietokantaEntities entities = new MVCTietokantaEntities();
            int iid = int.Parse(id);
            var model = (from h in entities.Henkilot
                         where h.HenkiloId == iid
                         select new
                         {
                             HenkiloId = h.HenkiloId,
                             Etunimi = h.Etunimi,
                             Sukunimi = h.Sukunimi,
                             Osoite = h.Osoite,
                             Esimies = h.Esimies
                         }).FirstOrDefault();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Update(Henkilot henk)
        {
            MVCTietokantaEntities entities = new MVCTietokantaEntities();



            bool OK = false;


            if (henk.HenkiloId == 0)
            {
                //uuden lisäys
                Henkilot dbItem = new Henkilot()
                {
                    Etunimi = henk.Etunimi,
                    Sukunimi = henk.Sukunimi,
                    Osoite = henk.Osoite,
                    Esimies = henk.Esimies
                };

                //tallennus tietokantaan
                entities.Henkilot.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }

            else
            {

                //Muokkaus, haetaan idn:n perusteella rivi tietokannasta
                Henkilot dbItem = (from h in entities.Henkilot
                                   where h.HenkiloId == henk.HenkiloId
                                   select h).FirstOrDefault();

                if (dbItem != null)
                {

                    dbItem.Etunimi = henk.Etunimi;
                    dbItem.Sukunimi = henk.Sukunimi;
                    dbItem.Osoite = henk.Osoite;
                    dbItem.Esimies = henk.Esimies;

                    //tallennus tietokantaan
                    entities.SaveChanges();
                    OK = true;
                }
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Delete(int id)
        {
            MVCTietokantaEntities entities = new MVCTietokantaEntities();

            //etsitään id:n perusteella rivi kannasta
            bool OK = false;
            Henkilot dbItem = (from h in entities.Henkilot
                               where h.HenkiloId == id
                               select h).FirstOrDefault();

            if (dbItem != null)
            {
                //tietokannasta poisto
                entities.Henkilot.Remove(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }
    }
}

