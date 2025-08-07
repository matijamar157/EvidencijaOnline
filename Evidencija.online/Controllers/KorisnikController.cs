using Evidencija.online.Data;
using Evidencija.online.Models;
using Evidencija.online.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evidencija.online.Controllers
{
    public class KorisnikController : BaseController
    {
        private readonly IKorisnikService _korisnikService;

        public KorisnikController(
            IKorisnikService korisnikService,
            ILogger logger)
            : base(logger)
        {
            _korisnikService = korisnikService ?? throw new ArgumentNullException(nameof(korisnikService));
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var korisnici = await _korisnikService.GetAllAsync();
                return View(korisnici);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Dohvaćanje korisnika");
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                SetErrorMessage("ID korisnika nije specifiran");
                return NotFound();
            }

            try
            {
                var korisnik = await _korisnikService.GetByIdAsync(id);
                if (korisnik == null)
                {
                    SetErrorMessage("Korisnik nije pronađen");
                    return NotFound();
                }

                return View(korisnik);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Dohvaćanje korisnika");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ime,Prezime")] Korisnik korisnik)
        {
            try
            {
                var userEmail = GetCurrentUserEmail();
                await _korisnikService.CreateAsync(korisnik, userEmail);
                SetSuccessMessage("Korisnik je uspješno kreiran");
                return RedirectToAction("Create", "Trosak");
            }
            catch (ArgumentException ex)
            {
                SetErrorMessage(ex.Message);
                return View(korisnik);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Kreiranje korisnika");
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                SetErrorMessage("ID korisnika nije specifiran");
                return NotFound();
            }

            try
            {
                var korisnik = await _korisnikService.GetByIdAsync(id);
                if (korisnik == null)
                {
                    SetErrorMessage("Korisnik nije pronađen");
                    return NotFound();
                }

                return View(korisnik);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Dohvaćanje korisnika za uređivanje");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Ime,Prezime,Email")] Korisnik korisnik)
        {
            if (id != korisnik.Ime)
            {
                SetErrorMessage("ID korisnika se ne podudara");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(korisnik);
            }

            try
            {
                await _korisnikService.UpdateAsync(korisnik);
                SetSuccessMessage("Korisnik je uspješno ažuriran");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                SetErrorMessage(ex.Message);
                return View(korisnik);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Ažuriranje korisnika");
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                SetErrorMessage("ID korisnika nije specifiran");
                return NotFound();
            }

            try
            {
                var korisnik = await _korisnikService.GetByIdAsync(id);
                if (korisnik == null)
                {
                    SetErrorMessage("Korisnik nije pronađen");
                    return NotFound();
                }

                return View(korisnik);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Dohvaćanje korisnika za brisanje");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var result = await _korisnikService.DeleteAsync(id);
                if (result)
                {
                    SetSuccessMessage("Korisnik je uspješno obrisan");
                }
                else
                {
                    SetErrorMessage("Korisnik nije pronađen");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Brisanje korisnika");
            }
        }
    }
}
