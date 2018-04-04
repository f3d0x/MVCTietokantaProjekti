using MVCTietokantaProj.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTietokantaProj.Controllers
{
    public class HourController : Controller
    {
        // GET: Hour
        public ActionResult Hours()
        {
            return View();
        }
        public JsonResult GetList()
        {
            MVCTietokantaEntities entities = new MVCTietokantaEntities();
            //List<Tunnit> model = entities.Tunnits.ToList();

            var model = (from t in entities.Tunnit
                         select new
                         {
                             TuntiId = t.TuntiId,
                             ProjektiId = t.ProjektiId,
                             HenkiloId = t.HenkiloId,
                             Pvm = t.Pvm
                         }).ToList();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSingleHour(string id)
        {
            MVCTietokantaEntities entities = new MVCTietokantaEntities();
            int iid = int.Parse(id);
            var model = (from t in entities.Tunnit
                         where t.TuntiId == iid
                         select new
                         {
                             TuntiId = t.TuntiId,
                             ProjektiId = t.ProjektiId,
                             HenkiloId = t.HenkiloId,
                             Pvm = t.Pvm
                         }).FirstOrDefault();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Update(Tunnit tun)
        {
            MVCTietokantaEntities entities = new MVCTietokantaEntities();
            // int id = tun.TuntiId;


            bool OK = false;


            if (tun.TuntiId == 0)
            {
                // uuden lisäys
                Tunnit dbItem = new Tunnit()
                {
                    //         TuntiId = tun.ProjektiId.Substring(0, 5).Trim().ToUpper(),
                    ProjektiId = tun.ProjektiId,
                    HenkiloId = tun.HenkiloId,
                    Pvm = tun.Pvm
                };

                //tallennus tietokantaan
                entities.Tunnit.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }

            else
            {

                //Muokkaus, haetaan idn:n perusteella rivi tietokannasta
                Tunnit dbItem = (from t in entities.Tunnit
                                 where t.TuntiId == tun.TuntiId
                                 select t).FirstOrDefault();

                if (dbItem != null)
                {
                    //      dbItem.TuntiId = tun.TuntiId;
                    dbItem.ProjektiId = tun.ProjektiId;
                    dbItem.HenkiloId = tun.HenkiloId;
                    dbItem.Pvm = tun.Pvm;

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
            Tunnit dbItem = (from t in entities.Tunnit
                             where t.TuntiId == id
                             select t).FirstOrDefault();

            if (dbItem != null)
            {
                //tietokannasta poisto
                entities.Tunnit.Remove(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }
    }
}