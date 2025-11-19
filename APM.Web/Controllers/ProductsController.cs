using APM.Application.Commands.Products.CreateProduct;
using APM.Application.Queries.Products.GetAllProducts;
using APM.Application.Queries.Products.GetProductById;
using APM.Application.Commands.Products.UpdateProduct;
using APM.Application.Commands.Products.DeleteProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace APM.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _mediator.Send(new GetAllProductsQuery());
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, decimal price)
        {
            await _mediator.Send(new CreateProductCommand(name, price));
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });
            if (product == null)
            {
                return NotFound();
            }
            return View(new UpdateProductCommand(product.Id, product.Name, product.Price));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateProductCommand command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteProductCommand { Id = id });
            return RedirectToAction(nameof(Index));
        }
    }

}
