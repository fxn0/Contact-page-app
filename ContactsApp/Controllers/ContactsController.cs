using ContactsApp.Data;
using ContactsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;



namespace ContactsApp.Controllers
{
    //klasa kontrolera obsługująca żądania dotyczące kontaktów
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ContactsContext _context;

        public ContactsController(ContactsContext context)
        {
            _context = context;
        }


        //metoda pozwalająca pobranie listy kontaktów
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            return await _context.Contacts.Include(c => c.Category).ToListAsync();
        }

        //metoda pozwalająca pobranie pojedyńczego kontaktu przy znanym id kontaktu
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _context.Contacts.Include(c => c.Category).FirstOrDefaultAsync(c => c.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            return contact;
        }

        //ze względu na wymaganie unikalnego maila możemy pobrać jeden kontakt przy znajomości maila
        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<Contact>> GetContactByEmail(string email)
        {
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Email == email);

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }


        //metoda pozwalająca dodanie nowego kontaktu
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.ContactCategories.FindAsync(contact.CategoryId);
            if (category==null)
            {
                return BadRequest("Invalid category.");
            }
            else
            {
                contact.Category = category;
            }

            _context.Contacts.Add(contact);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_Contacts_Email"))
                {
                    return Conflict(new { message = "Email address already in use." });
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }


        //metoda pozwalająca edycję istniejącego kontaktu
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, Contact contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }

            _context.Entry(contact).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Contacts.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        //metoda pozwalająca usunięcie istniejącego kontaktu
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }


    //klasa kontrolera obsługująca żądania dotyczące kategorii kontaktów
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ContactsContext _context;

        public CategoriesController(ContactsContext context)
        {
            _context = context;
        }

        //metoda pozwalająca pobranie listy kategorii kontaktów
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactCategory>>> GetCategories()
        {
            return await _context.ContactCategories.ToListAsync();
        }
    }


    //klasa odpowiedzialna za żądania logowania na stronie
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ContactsContext _context;

        public AuthController(ContactsContext context)
        {
            _context = context;
        }

        //metoda odpowiedzialna za logowanie oraz autoryzowanie dostępu poprzez weryfikację poprawności danych z bazą
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Contacts.SingleOrDefault(c => c.Email == request.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }else if(user.Password != request.Password) {
                return Unauthorized("Wrong password");
            }

            var token = "authToken";

            return Ok(new { Token = token });
        }
    }

    //prosta klasa pomocnicza zawierająca dane potrzebne do zalogowania
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

