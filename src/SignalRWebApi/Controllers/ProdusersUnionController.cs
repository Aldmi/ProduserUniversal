using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbstractProduser.Enums;
using AbstractProduser.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProdusersMediator;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdusersUnionController : ControllerBase
    {
        private readonly ProdusersFactory _produsersFactory;
        private readonly ProdusersUnion _produsersUnion;



        public ProdusersUnionController(ProdusersFactory produsersFactory, ProdusersUnion produsersUnion)
        {
            _produsersFactory = produsersFactory;
            _produsersUnion = produsersUnion;
        }




        // GET api/ProdusersUnion
        [HttpGet]
        public ActionResult<ProduserOptionAgregator> Get()
        {
            var agrOptions = _produsersFactory.GetProduserUnionOptionAgregator();
            return new JsonResult(agrOptions);
        }



        // POST api/ProdusersUnion
        [HttpPost]
        public ActionResult Post([FromBody] ProduserOptionAgregator value)
        {
            _produsersFactory.FillProduserUnionByOptionAgregator(value);
            return Ok();
        }


        // DELETE api/ProdusersUnion/key
        [HttpDelete("{key}")]
        public ActionResult Delete(string key)
        {
           var res= _produsersFactory.DeleteProduserUnionByOptionAgregator(key);
           return res ? (ActionResult) Ok() : NotFound(key);
        }
    }
}