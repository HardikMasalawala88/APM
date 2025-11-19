using APM.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APM.Application.Commands.Products.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                // Handle not found scenario, e.g., throw an exception or return a specific result.
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
