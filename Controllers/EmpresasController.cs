using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CnpjMvc.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace CnpjMvc.Controllers;

public class EmpresasController : Controller
{

    public readonly string uriBase = "http://myprojects.somee.com/CnpjApi/Empresas";

    [HttpGet]
    public async Task<ActionResult> IndexAsync()
    {
        try
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(uriBase);
            string serialized = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<EmpresasViewModel> listaEmpresas = await Task.Run(() =>
                    JsonConvert.DeserializeObject<List<EmpresasViewModel>>(serialized));

                return View(listaEmpresas);
            }
            else
                throw new System.Exception(serialized);
        }
        catch (System.Exception ex)
        {
            TempData["MensagemErro"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(EmpresasViewModel novaEmpresa)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string uricomplementar = "/Adicionar";
                var content = new StringContent(JsonConvert.SerializeObject(novaEmpresa));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase + uricomplementar, content);

                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Mensagem"] = string.Format("Empresa {0} foi adicionada com sucesso!", novaEmpresa.Nome);
                    return RedirectToAction("Index", "Empresas");
                }
                else
                    throw new System.Exception(serialized);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Create");
            }
        }
        



}