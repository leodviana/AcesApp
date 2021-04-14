using AcesApp.Models;
using AcesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AcesApp.Interfaces
{
    public interface IApiService
    {
        Task<Response> getUsuario(Usuario usuario);

        Task<Response> getEventos(Events evento);

        Task<Response> getEventosfree(Events evento);

        Task<Response> getprofessores(Events evento);

        Task<Response> saveHorarios(long id,Events evento);

        Task<Response> getRanking(Ranking ranking);
    }
}
