using MVCTietokantaProj.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTietokantaProj.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project
        public ActionResult Projects()
        {
            return View();
        }
        public JsonResult GetList()
        {
            MVCTietokantaEntities entities = new MVCTietokantaEntities();
            //List<Projektit> model = entities.Projektits.ToList();

            var model = (from p in entities.Projektit
                         select new
                         {
                             ProjektiId = p.ProjektiId,
                             Nimi = p.Nimi
                         }).ToList();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSingleProject(string id)
        {
            MVCTietokantaEntities entities = new MVCTietokantaEntities();
            int iid = int.Parse(id);
            var model = (from p in entities.Projektit
                         where p.ProjektiId == iid
                         select new
                         {
                             ProjektiId = p.ProjektiId,
                             Nimi = p.Nimi
                         }).FirstOrDefault();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Update(Projektit proj)
        {
            MVCTietokantaEntities entities = new MVCTietokantaEntities();
            //int id = proj.ProjektiId;


            bool OK = false;


            if (proj.ProjektiId == 0)
            {
                //uuden lisäys
                Projektit dbItem = new Projektit()
                {
                    //ProjektiId = proj.ProjektiId.Substring(0, 5).Trim().ToUpper(),
                    Nimi = proj.Nimi
                };

                //tallennus tietokantaan
                entities.Projektit.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }

            else
            {

                //Muokkaus, haetaan idn:n perusteella rivi tietokannasta
                Projektit dbItem = (from p in entities.Projektit
                                    where p.ProjektiId == proj.ProjektiId
                                    select p).FirstOrDefault();

                if (dbItem != null)
                {
                    //dbItem.ProjektiId = proj.ProjektiId;
                    dbItem.Nimi = proj.Nimi;

                    //tallennus tietokantaan
                    entities.SaveChanges();
                    OK = true;
                }
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Delete(string id)
        {
            MVCTietokantaEntities entities = new MVCTietokantaEntities();

            int projektiid = int.Parse(id);


            //etsitään id:n perusteella projektirivi kannasta
            bool OK = false;
            Projektit dbItem = (from p in entities.Projektit
                                where p.ProjektiId == projektiid
                                select p).FirstOrDefault();

            if (dbItem != null)
            {
                //tietokannasta poisto
                entities.Projektit.Remove(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }
    }
}