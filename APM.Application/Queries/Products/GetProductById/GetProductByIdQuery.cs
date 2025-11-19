using APM.Application.DTOs;
using MediatR;

namespace APM.Application.Queries.Products.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductDto?>
    {
        public int Id { get; set; }
    }
}
