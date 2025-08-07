using Evidencija.online.Data;
using Evidencija.online.Models;
using Evidencija.online.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evidencija.online.Controllers
{
    [Authorize]
    public class TrosakController : BaseController
    {
        private readonly ITrosakService _trosakService;
        private readonly IKategorijaService _kategorijaService;
        private readonly IKorisnikService _korisnikService;

        public TrosakController(
            ITrosakService trosakService,
            IKategorijaService kategorijaService,
            IKorisnikService korisnikService,
            ILogger logger)
            : base(logger)
        {
            _trosakService = trosakService ?? throw new ArgumentNullException(nameof(trosakService));
            _kategorijaService = kategorijaService ?? throw new ArgumentNullException(nameof(kategorijaService));
            _korisnikService = korisnikService ?? throw new ArgumentNullException(nameof(korisnikService));
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userEmail = GetCurrentUserEmail();
                var troskovi = await _trosakService.GetAllByUserAsync(userEmail);
                return View(troskovi);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Dohvaćanje troškova");
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                SetErrorMessage("ID troška nije specifiran");
                return NotFound();
            }

            try
            {
                var trosak = await _trosakService.GetByIdAsync(id.Value);
                if (trosak == null)
                {
                    SetErrorMessage("Trošak nije pronađen");
                    return NotFound();
                }

                return View(trosak);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Dohvaćanje troška");
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var userEmail = GetCurrentUserEmail();

                if (!await _trosakService.CanUserCreateExpenseAsync(userEmail))
                {
                    SetWarningMessage("Dosegnuli ste limit od 5 troškova za besplatni plan");
                    return RedirectToAction("Packages", "Home");
                }

                await LoadViewDataAsync();
                return View();
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Priprema forme za kreiranje troška");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Opis,KorisnikId,KategorijaId,Datum,Iznos,Valuta")] Trosak trosak)
        {
            try
            {
                var userEmail = GetCurrentUserEmail();
                await _trosakService.CreateAsync(trosak, userEmail);
                SetSuccessMessage("Trošak je uspješno kreiran");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                SetErrorMessage(ex.Message);
                await LoadViewDataAsync();
                return View(trosak);
            }
            catch (InvalidOperationException ex)
            {
                SetErrorMessage(ex.Message);
                await LoadViewDataAsync();
                return View(trosak);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Kreiranje troška");
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                SetErrorMessage("ID troška nije specifiran");
                return NotFound();
            }

            try
            {
                var trosak = await _trosakService.GetByIdAsync(id.Value);
                if (trosak == null)
                {
                    SetErrorMessage("Trošak nije pronađen");
                    return NotFound();
                }

                await LoadViewDataAsync(trosak);
                return View(trosak);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Dohvaćanje troška za uređivanje");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Opis,KorisnikId,KategorijaId,Datum,Iznos,Valuta")] Trosak trosak)
        {
            if (id != trosak.Id)
            {
                SetErrorMessage("ID troška se ne podudara");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadViewDataAsync(trosak);
                return View(trosak);
            }

            try
            {
                await _trosakService.UpdateAsync(trosak);
                SetSuccessMessage("Trošak je uspješno ažuriran");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                SetErrorMessage(ex.Message);
                await LoadViewDataAsync(trosak);
                return View(trosak);
            }
            catch (InvalidOperationException ex)
            {
                SetErrorMessage(ex.Message);
                await LoadViewDataAsync(trosak);
                return View(trosak);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Ažuriranje troška");
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                SetErrorMessage("ID troška nije specifiran");
                return NotFound();
            }

            try
            {
                var trosak = await _trosakService.GetByIdAsync(id.Value);
                if (trosak == null)
                {
                    SetErrorMessage("Trošak nije pronađen");
                    return NotFound();
                }

                return View(trosak);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Dohvaćanje troška za brisanje");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _trosakService.DeleteAsync(id);
                if (result)
                {
                    SetSuccessMessage("Trošak je uspješno obrisan");
                }
                else
                {
                    SetErrorMessage("Trošak nije pronađen");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Brisanje troška");
            }
        }

        // Helper methods - Following SRP
        private async Task LoadViewDataAsync(Trosak trosak = null)
        {
            try
            {
                var kategorije = await _kategorijaService.GetAllAsync();
                var korisnici = await _korisnikService.GetAllAsync();

                ViewData["KategorijaId"] = new SelectList(kategorije, "Id", "Naziv", trosak?.KategorijaId);
                ViewData["KorisnikId"] = new SelectList(korisnici, "Id", "Ime", trosak?.KorisnikId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Greška pri učitavanju podataka za ViewData", ex);
                // Set empty SelectLists to avoid null reference exceptions
                ViewData["KategorijaId"] = new SelectList(new List<Kategorija>(), "Id", "Naziv");
                ViewData["KorisnikId"] = new SelectList(new List<Korisnik>(), "Id", "Ime");
            }
        }
    }
}
