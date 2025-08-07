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
    public class KategorijaController : BaseController
    {
        private readonly IKategorijaService _kategorijaService;

        public KategorijaController(
            IKategorijaService kategorijaService,
            ILogger logger)
            : base(logger)
        {
            _kategorijaService = kategorijaService ?? throw new ArgumentNullException(nameof(kategorijaService));
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var kategorije = await _kategorijaService.GetAllAsync();
                return View(kategorije);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Dohvaćanje kategorije");
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                SetErrorMessage("ID kategorije nije specifiran");
                return NotFound();
            }

            try
            {
                var kategorija = await _kategorijaService.GetByIdAsync(id.Value);
                if (kategorija == null)
                {
                    SetErrorMessage("Kategorija nije pronađena");
                    return NotFound();
                }

                return View(kategorija);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Dohvaćanje kategorije");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Opis")] Kategorija kategorija)
        {
            if (!ModelState.IsValid)
            {
                return View(kategorija);
            }

            try
            {
                await _kategorijaService.CreateAsync(kategorija);
                SetSuccessMessage("Kategorija je uspješno kreirana");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                SetErrorMessage(ex.Message);
                return View(kategorija);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Kreiranje kategorije");
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                SetErrorMessage("ID kategorije nije specifiran");
                return NotFound();
            }

            try
            {
                var kategorija = await _kategorijaService.GetByIdAsync(id.Value);
                if (kategorija == null)
                {
                    SetErrorMessage("Kategorija nije pronađena");
                    return NotFound();
                }

                return View(kategorija);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Dohvaćanje kategorije za uređivanje");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis")] Kategorija kategorija)
        {
            if (id != kategorija.Id)
            {
                SetErrorMessage("ID kategorije se ne podudara");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(kategorija);
            }

            try
            {
                await _kategorijaService.UpdateAsync(kategorija);
                SetSuccessMessage("Kategorija je uspješno ažurirana");
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                SetErrorMessage(ex.Message);
                return View(kategorija);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Ažuriranje kategorije");
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                SetErrorMessage("ID kategorije nije specifiran");
                return NotFound();
            }

            try
            {
                var kategorija = await _kategorijaService.GetByIdAsync(id.Value);
                if (kategorija == null)
                {
                    SetErrorMessage("Kategorija nije pronađena");
                    return NotFound();
                }

                return View(kategorija);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Dohvaćanje kategorije za brisanje");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _kategorijaService.DeleteAsync(id);
                if (result)
                {
                    SetSuccessMessage("Kategorija je uspješno obrisana");
                }
                else
                {
                    SetErrorMessage("Kategorija nije pronađena");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Brisanje kategorije");
            }
        }
    }
}
