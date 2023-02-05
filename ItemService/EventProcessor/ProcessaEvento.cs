using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ItemService.Data;
using ItemService.Dtos;
using ItemService.Models;

namespace ItemService.EventProcessor
{
    public class ProcessaEvento : IProcessaEvento
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public ProcessaEvento(IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }
        public void Processa(string mensagem)
        {
           using var scope = _scopeFactory.CreateScope();
           
           var itemRepsoitory = scope.ServiceProvider.GetRequiredService<IItemRepository>();

            var restauranteReadDto = JsonSerializer.Deserialize<RestauranteReadDto>(mensagem); 

            var restaurante = _mapper.Map<Restaurante>(restauranteReadDto); 

            if(!itemRepsoitory.ExisteRestauranteExterno(restaurante.Id))
            {
                itemRepsoitory.CreateRestaurante(restaurante);
                itemRepsoitory.SaveChanges();
            }                            
        }
    }
}