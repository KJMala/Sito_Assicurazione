using RCA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RCA.Controllers
{
    public class UserController : Controller
    {
        public bool ControlloUser(string username)
        {
            if (User.Identity.Name == username)
            {
                return true;
            }
            return false;
        }

        // GET: User
        [HttpGet]
        public ActionResult InfoUtente(int ID)
        {
            Proprietario utente = new Proprietario();
            foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
            {
                if (p.ID == ID)
                {
                    if (Convert.ToBoolean(ControlloUser(p.LogUtente.Username)))
                    {
                        utente = p;
                        return View(utente);
                    }
                    
                }
            }
            return RedirectToAction("Login", "RCA");
        }

        [HttpGet]
        public ActionResult EditUtente(int ID)
        {
            Proprietario utente = new Proprietario();
            foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
            {
                if (p.ID == ID)
                {
                    if (Convert.ToBoolean(ControlloUser(p.LogUtente.Username)))
                    {
                        utente = p;
                        return View(utente);
                    }
                }
            }
            return RedirectToAction("Login", "RCA");
        }

        [HttpPost]
        public ActionResult EditUtente(Proprietario proprietario)
        {
            if (ModelState.IsValid)
            {
                foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                {
                    if (proprietario.ID == p.ID)
                    {
                        if (Convert.ToBoolean(ControlloUser(p.LogUtente.Username)))
                        {
                            p.LogUtente.Username = proprietario.LogUtente.Username;
                            p.LogUtente.Password = proprietario.LogUtente.Password;
                            p.Indirizzo = proprietario.Indirizzo;
                            p.ComuneResidenza = proprietario.ComuneResidenza;
                            p.Sesso = proprietario.Sesso;
                            p.Telefono = proprietario.Telefono;
                            TempData["ConfermaModificheUtente"] = "Utente modificato con successo!";
                            return RedirectToAction("InfoUtente", new { @id = proprietario.ID });
                        }
                    }
                }
                return RedirectToAction("Login", "RCA");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult InfoPolizze(int ID)
        {
            if (ModelState.IsValid)
            {
                List<Polizza> listaPolizzeUtente = new List<Polizza>();
                foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                {
                    if (p.ID == ID)
                    {
                        if (Convert.ToBoolean(ControlloUser(p.LogUtente.Username)))
                        {
                            foreach (Veicolo v in p.listaVeicoli)
                            {
                                foreach (Polizza pol in v.listaPolizze)
                                {
                                    listaPolizzeUtente.Add(pol);
                                }
                            }
                            return View(listaPolizzeUtente);
                        }

                    }
                }
                return RedirectToAction("Login", "RCA");
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public ActionResult CercaSinistro(int ID)
        {
            if (ModelState.IsValid)
            {
                foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                {
                    if (p.ID == ID)
                    {
                        if (Convert.ToBoolean(ControlloUser(p.LogUtente.Username)))
                        {
                            return View(p);
                        }
                    }
                }
                return RedirectToAction("Login", "RCA");
            }
            else
            {
                return View();
            }
                
        }

        [HttpPost]
        public ActionResult CercaSinistro(Proprietario proprietario)
        {
            if (ModelState.IsValid)
            {
                int sxID = Convert.ToInt32(Request.QueryString["idSX"]);
                int idProp = Convert.ToInt32(Request.QueryString["idUser"]);
                foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                {
                    if (p.ID == idProp)
                    {
                        if (Convert.ToBoolean(ControlloUser(p.LogUtente.Username)))
                        {
                            foreach (Veicolo v in p.listaVeicoli)
                            {
                                foreach (Sinistro sx in v.listaSinistri)
                                {
                                    if (sx.ID == sxID)
                                    {
                                        //return RedirectToAction("DettagliSinistroUtente", new {propID = idProp, sinistroID = sx.ID });
                                        return RedirectToAction("DettagliSinistroUtente");
                                    }
                                }
                            }
                            return View();

                        }

                    }
                }
                return RedirectToAction("Login", "RCA");
            }
            else
            {
                return View();
            }
            
        }

        [HttpGet]
        public ActionResult DettagliSinistroUtente()
        {
            if (ModelState.IsValid)
            {
                int propID = Convert.ToInt32(Session["idUser"]);
                int sxID = Convert.ToInt32(Request.QueryString["idSX"]);

                bool sxInUser = false;
                Sinistro sinistroUtente = new Sinistro();
                //int propID = Convert.ToInt32(Request.QueryString["propID"]);
                foreach (Proprietario p in AssicurazioniRCA.listaProprietari)
                {
                    if (p.ID == propID)
                    {
                        if (Convert.ToBoolean(ControlloUser(p.LogUtente.Username)))
                        {
                            foreach (Veicolo v in p.listaVeicoli)
                            {
                                foreach (Sinistro sx in v.listaSinistri)
                                {
                                    if (sx.ID == sxID)
                                    {
                                        sxInUser = true;
                                        sinistroUtente = sx;
                                        break;
                                    }
                                }
                                if (sxInUser)
                                {
                                    return View(sinistroUtente);
                                }
                                else
                                {
                                    TempData["ErroreSinistro"] = "La ricerca non ha prodotto nessun risultato, nessun sinistro associato a questo utente";
                                    return View();
                                }
                            }

                        }

                    }
                }
                return RedirectToAction("Login", "RCA");
            }
            else
            {
                return View();
            }
            
        }
    }
}