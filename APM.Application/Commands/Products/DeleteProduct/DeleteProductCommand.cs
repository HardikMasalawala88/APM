using MediatR;

namespace APM.Application.Commands.Products.DeleteProduct
{
    public class DeleteProductCommand : IRequest
    {
        public int Id { get; set; }
    }
}
