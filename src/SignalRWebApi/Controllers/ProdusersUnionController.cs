using AbstractProduser.Options;
using Microsoft.AspNetCore.Mvc;
using ProdusersMediator;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdusersUnionController : ControllerBase
    {
        private readonly ProdusersFactory _produsersFactory;



        #region ctor

        public ProdusersUnionController(ProdusersFactory produsersFactory)
        {
            _produsersFactory = produsersFactory;
        }

        #endregion




        //GET api/ProdusersUnion
        [HttpGet]
        public ActionResult<ProduserUnionOption> Get()
        {
            var agrOptions = _produsersFactory.GetProduserUnionOptionAgregator();
            return new JsonResult(agrOptions);
        }



        //POST api/ProdusersUnion
        [HttpPost]
        public ActionResult Post([FromBody] ProduserUnionOption value)
        {
            _produsersFactory.FillProduserUnionByOptionAgregator(value);
            return Ok();
        }


        //DELETE api/ProdusersUnion/key
        [HttpDelete("{key}")]
        public ActionResult Delete(string key)
        {
            var res = _produsersFactory.DeleteProduserUnionByOptionAgregator(key);
            return res ? (ActionResult)Ok() : NotFound(key);
        }
    }
}