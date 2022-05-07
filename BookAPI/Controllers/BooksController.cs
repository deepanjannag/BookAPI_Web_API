using BookAPI.Models;
using BookAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController] //Automatic model validation
    public class BooksController : ControllerBase
    {
        public readonly IBookRepository _bookRepository;
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _bookRepository.Get();
        }

        [HttpGet("{id}")]
        public async Task<Book> GetBooks(int id)
        {
            return await _bookRepository.Get(id);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBook([FromBody] Book book)
        {
            var newBook = await _bookRepository.Create(book);
            return CreatedAtAction(nameof(GetBooks), new { id = newBook.BookId }, newBook);   //Generates 201 status code
        }

        [HttpPut]
        public async Task<ActionResult> EditBook(int id, [FromBody] Book book)
        {
            if (id != book.BookId)
                return BadRequest();

            await _bookRepository.Update(book);

            return NoContent(); //204 Status Code

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var bookToDelete = await _bookRepository.Get(id);
            if (bookToDelete == null)
                return NotFound();
            await _bookRepository.Delete(bookToDelete.BookId);
            return NoContent(); //204 Status Code
        }
    }
}
