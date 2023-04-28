﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using P011_IsTakip.Business.Abstract.ModelsService;
using P011_IsTakip.Entities.Classes;

namespace P011_IsTakip.WebUI.Controllers.ModelsController
{
    public class AjandaController : Controller
    {

        private readonly IAjandaService _ajandaService;

        public AjandaController(IAjandaService ajandaService)
        {
            _ajandaService = ajandaService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var model = await _ajandaService.GetListAsync();
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Musteri = new SelectList(_ajandaService.GetSelectList(), "MusteriId");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Ajanda model)
        {
            if (model == null)
            {
                return RedirectToAction(nameof(IndexAsync));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.OlusturmaTarihi = DateTime.Now;
            model.OlusturanKullaniciId = model.Id;
            model.Aktif = true;
            model.Silindi = false;
            await _ajandaService.AddAsync(model);

            return RedirectToAction(nameof(IndexAsync));
        }

        public IActionResult Edit(int id)
        {

            var model = _ajandaService.GetById(id);

            if (model is null)
                return RedirectToAction(nameof(IndexAsync));

            ViewBag.Musteri = new SelectList(_ajandaService.GetSelectList(), "MusteriId");

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Ajanda model)
        {
            if (model == null)
            {
                return RedirectToAction(nameof(IndexAsync));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var data = _ajandaService.GetById(model.Id);

            if (data == null)
            {
                return RedirectToAction(nameof(IndexAsync));
            }

            data.NotTarihi = model.NotTarihi;
            data.Aciklama = model.Aciklama;
            data.MusteriId = model.MusteriId;
            data.GuncellemeTarihi = DateTime.Now;
            data.GuncelleyenKullaniciId = model.Id;

            _ajandaService.Update(data);

            return RedirectToAction(nameof(IndexAsync));
        }

        public IActionResult Detail(int id)
        {

            var data = _ajandaService.GetById(id);
            if (data is null)
                return RedirectToAction(nameof(IndexAsync));

            return View(data);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {

            var data = _ajandaService.GetById(id);
            if (data is null)
                return RedirectToAction(nameof(IndexAsync));

            return View(data);
        }

        [HttpPost]
        public IActionResult Delete(Ajanda model)
        {
            if (model is null)
                return RedirectToAction(nameof(IndexAsync));

            var data = _ajandaService.GetById(model.Id);
            if (data is null)
                return RedirectToAction(nameof(IndexAsync));

            data.Silindi = true;


            _ajandaService.Delete(data);

            return RedirectToAction(nameof(IndexAsync));
        }


        //[HttpPost]
        //public async Task<IActionResult> BolumGetir(int id)
        //{
        //    var satirlar = await _context.Bolum.Where(t => t.DepartmanId == id).ToListAsync();

        //    return Json(satirlar);
        //}


    }
}