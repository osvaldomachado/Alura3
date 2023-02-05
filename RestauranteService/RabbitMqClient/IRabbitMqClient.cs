using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestauranteService.Dtos;

namespace RestauranteService.RabbitMqClient
{
    public interface IRabbitMqClient
    {
        void PublicaRestaurante(RestauranteReadDto restauranteReadDto);
    }
}