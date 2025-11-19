using APM.Application.DTOs;
using MediatR;

namespace APM.Application.Queries.Products.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
    }
}
