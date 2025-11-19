using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM.Application.Commands.Products.CreateProduct
{
    public record CreateProductCommand(string Name, decimal Price) : IRequest<int>;

}
