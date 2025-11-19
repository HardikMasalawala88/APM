using MediatR;

namespace APM.Application.Commands.Products.UpdateProduct
{
    public class UpdateProductCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public UpdateProductCommand(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}
