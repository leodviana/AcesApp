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
    }
}
