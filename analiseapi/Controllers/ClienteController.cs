using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace analiseapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private static List<Cliente> clientes = new List<Cliente>
            {
                new Cliente {
                    Id = 1,
                    firstName = "Renato",
                    lastName = "Russo",
                    Address = "Rio de Janeiro"
                },

                new Cliente {
                    Id = 2,
                    firstName = "Kiko",
                    lastName = "Loureiro",
                    Address = "Belo Horizonte"
                }
            };
        private readonly DataContext _dataContext;

        public ClienteController(DataContext context)
        {
            _dataContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> Get()
        {           
            return Ok(await _dataContext.Clientes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> Get(int id)
        {
            var cliente = await _dataContext.Clientes.FindAsync(id);
            if(cliente == null)
                return BadRequest("Cliente não existe.");
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<List<Cliente>>> AddCliente(Cliente cliente)
        {
            _dataContext.Clientes.Add(cliente);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Clientes.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Cliente>>> UpdateCliente(Cliente request)
        {
            var dbcliente = clientes.Find(h => h.Id == request.Id);
            if (dbcliente == null)
                return BadRequest("Cliente não existe.");

            dbcliente.firstName = request.firstName;
            dbcliente.lastName = request.lastName;
            dbcliente.Address = request.Address;

            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Clientes.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Cliente>>> Delete(int id)
        {
            var cliente = clientes.Find(c => c.Id == id);
            if (cliente == null)
                return BadRequest("Cliente não existe.");

            clientes.Remove(cliente);
            return Ok(clientes);
        }
    }
}
