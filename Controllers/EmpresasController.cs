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




}