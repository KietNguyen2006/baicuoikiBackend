using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.Models;
using System.Threading.Tasks;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class SupplierController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SupplierController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Supplier
    [HttpGet]
    public async Task<IActionResult> GetSuppliers([FromQuery] string? name, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = _context.Suppliers.AsQueryable();
        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(s => s.Name.Contains(name));
        var total = await query.CountAsync();
        var suppliers = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return Ok(new { total, suppliers });
    }

    // GET: api/Supplier/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupplier(int id)
    {
        var supplier = await _context.Suppliers
            .Include(s => s.ProductSuppliers)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (supplier == null)
            return NotFound(new { message = "Supplier not found." });
        return Ok(supplier);
    }

    // POST: api/Supplier
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateSupplier([FromBody] Supplier supplier)
    {
        if (string.IsNullOrWhiteSpace(supplier.Name))
            return BadRequest(new { message = "Supplier name is required." });
        if (await _context.Suppliers.AnyAsync(s => s.Name == supplier.Name))
            return BadRequest(new { message = "Supplier name already exists." });

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSupplier), new { id = supplier.Id }, supplier);
    }

    // PUT: api/Supplier/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateSupplier(int id, [FromBody] Supplier supplier)
    {
        var existing = await _context.Suppliers.FindAsync(id);
        if (existing == null)
            return NotFound(new { message = "Supplier not found." });

        if (string.IsNullOrWhiteSpace(supplier.Name))
            return BadRequest(new { message = "Supplier name is required." });
        if (await _context.Suppliers.AnyAsync(s => s.Name == supplier.Name && s.Id != id))
            return BadRequest(new { message = "Supplier name already exists." });

        existing.Name = supplier.Name;
        existing.Description = supplier.Description;
        existing.ContactInfo = supplier.ContactInfo;
        existing.Address = supplier.Address;
        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    // DELETE: api/Supplier/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null)
            return NotFound(new { message = "Supplier not found." });

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}