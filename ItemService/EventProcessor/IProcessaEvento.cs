using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemService.EventProcessor
{
    public interface IProcessaEvento
    {
        void Processa(string mensagem);
    }
}