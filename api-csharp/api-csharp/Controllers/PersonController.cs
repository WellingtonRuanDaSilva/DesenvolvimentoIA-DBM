using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CsvImportApi.Data;
using CsvImportApi.Models;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvImportApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PersonController> _logger;

        public PersonController(ApplicationDbContext context, ILogger<PersonController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo não fornecido");

            try
            {
                // Primeiro, limpe a tabela existente
                _context.People.RemoveRange(_context.People);
                await _context.SaveChangesAsync();

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true,
                        HeaderValidated = null,
                        MissingFieldFound = null
                    };

                    using (var csv = new CsvReader(reader, config))
                    {
                        csv.Context.RegisterClassMap<PersonMap>();
                        var records = csv.GetRecords<PersonImport>()
                            .Select(p => new Person
                            {
                                Nome = p.Nome,
                                Idade = p.Idade,
                                Cidade = p.Cidade,
                                Profissao = p.Profissao
                            })
                            .ToList();

                        if (!records.Any())
                            return BadRequest("O arquivo CSV não contém registros válidos");

                        await _context.People.AddRangeAsync(records);
                        await _context.SaveChangesAsync();

                        return Ok($"Importados {records.Count} registros com sucesso");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao importar CSV");
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest($"Erro ao importar dados: {innerMessage}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _context.People.FindAsync(id);

            if (person == null)
                return NotFound();

            return person;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetAllPeople()
        {
            return await _context.People.ToListAsync();
        }
    }

    // Classe auxiliar para importação
    public class PersonImport
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
        public string Cidade { get; set; } = string.Empty;
        public string Profissao { get; set; } = string.Empty;
    }

    public sealed class PersonMap : ClassMap<PersonImport>
    {
        public PersonMap()
        {
            Map(m => m.Id);
            Map(m => m.Nome);
            Map(m => m.Idade);
            Map(m => m.Cidade);
            Map(m => m.Profissao);
        }
    }
}